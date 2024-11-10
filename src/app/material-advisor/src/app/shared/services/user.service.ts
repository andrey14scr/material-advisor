import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "@environments/environment";
import { User } from "@shared/models/User";
import { UserSettings } from "@shared/models/UserSettings";
import { Observable } from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private apiRoot = `${environment.apiUrl}/api/user`;
  
  constructor(private http: HttpClient) {
  }

  updateUser(currentLanguage: string) {
    const body = {currentLanguage: currentLanguage}
    this.http.patch<any>(`${this.apiRoot}/settings`, body).subscribe({});
  }

  getUsers(page: number, pageSize: number): Observable<User[]> {
    let params = new HttpParams().set("page", page).set("pageSize", pageSize);
    return this.http.get<User[]>(`${this.apiRoot}`, { params: params });
  }

  getUserCurrentLanguage(): Observable<UserSettings> {
    return this.http.get<UserSettings>(`${this.apiRoot}/settings`);
  }
}
