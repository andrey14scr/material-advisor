import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "@environments/environment";
import { Observable } from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private apiRoot = `${environment.apiUrl}/api/user`;
  
  constructor(private http: HttpClient) {
  }

  updateUser(currentLanguage: string): void {
    const body = {currentLanguage: currentLanguage}
    this.http.patch<any>(`${this.apiRoot}/settings`, body).subscribe({});
  }

  getUserCurrentLanguage(): Observable<string> {
    return this.http.get<string>(`${this.apiRoot}/language`);
  }
}
