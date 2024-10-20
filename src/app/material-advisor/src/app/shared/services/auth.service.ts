import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { SecurityService } from './security.service';
import { environment } from '@environments/environment';
import { CookieStorageService } from './cookie-storage.service';
import { User } from '@shared/models/User';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private accessTokenKey = 'accessToken';
  private refreshTokenKey = 'refreshToken';
  private currentUser = new BehaviorSubject<User | null>(null);
  private apiRoot = `${environment.apiUrl}/api/auth`;

  constructor(private http: HttpClient, 
    private securityService: SecurityService, 
    private cookieStorageService: CookieStorageService, 
    private router: Router) {}

  login(login: string, password: string): Observable<any> {
    const hashPassord = this.securityService.getHash(password);
    return this.http.post(`${this.apiRoot}/login`, { login: login, password: hashPassord }).pipe(
      map((tokens: any) => {
        this.storeTokens(tokens.accessToken, tokens.refreshToken);
        this.decodeToken();
        return tokens;
      }),
      catchError((error) => throwError(error))
    );
  }

  register(username: string, email: string, password: string): Observable<any> {
    const hashPassord = this.securityService.getHash(password);
    const userData = { userName: username, email: email, password: hashPassord };
    return this.http.post(`${this.apiRoot}/register`, userData).pipe(
      map((tokens: any) => {
        this.storeTokens(tokens.accessToken, tokens.refreshToken);
        return tokens;
      }),
      catchError((error) => throwError(error))
    );;
  }

  refreshAccessToken(): Observable<any> {
    const refreshToken = this.getRefreshToken();
    if (!refreshToken) {
      this.logout();
      return throwError('No refresh token found');
    }

    return this.http.post(`${this.apiRoot}/refresh`, `"${refreshToken}"`, {
      headers: { 'Content-Type': 'application/json' },
    }).pipe(
      map((tokens: any) => {
        this.storeTokens(tokens.accessToken, tokens.refreshToken);
        return tokens;
      }),
      catchError((error) => {
        this.logout();
        return throwError(error);
      })
    );
  }

  logout() {
    this.clearTokens();
    this.router.navigate(['/login']);
  }

  private storeTokens(accessToken: string, refreshToken: string) {
    this.cookieStorageService.setItem(this.accessTokenKey, accessToken);
    this.cookieStorageService.setItem(this.refreshTokenKey, refreshToken);
    this.decodeToken();
  }

  getAccessToken(): string | null {
    return this.cookieStorageService.getItem(this.accessTokenKey);
  }

  getRefreshToken(): string | null {
    return this.cookieStorageService.getItem(this.refreshTokenKey);
  }

  private clearTokens() {
    this.cookieStorageService.removeItem(this.accessTokenKey);
    this.cookieStorageService.removeItem(this.refreshTokenKey);
    this.currentUser.next(null);
  }

  decodeToken() {
    const token = this.getAccessToken();
    if (token) {
      const payload = JSON.parse(atob(token.split('.')[1]));
      const user = {
        id: '',
        name: payload.name,
        email: payload.email
      };
      this.currentUser.next(user);
    }
  }

  getCurrentUser(): User | null {
    return this.currentUser.value;
  }

  isAuthenticated(): boolean {
    const token = this.getAccessToken();
    return token ? !this.isTokenExpired(token) : false;
  }

  private isTokenExpired(token: string): boolean {
    const payload = JSON.parse(atob(token.split('.')[1]));
    return payload.exp * 1000 < Date.now();
  }
}