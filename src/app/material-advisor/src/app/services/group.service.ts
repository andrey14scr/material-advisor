import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "@environments/environment";
import { Group } from "@models/user/Group";
import { Observable } from "rxjs";

@Injectable({
  providedIn: 'root',
})
export class GroupService {
  private apiRoot = `${environment.apiUrl}/api/Group`;

  constructor(private http: HttpClient) { }

  getGroupsAsOwner(): Observable<Group[]> {
    return this.http.get<Group[]>(`${this.apiRoot}/owner`);
  }

  getGroupsAsMember(): Observable<Group[]> {
    return this.http.get<Group[]>(`${this.apiRoot}/member`);
  }

  postGroup(group: any): Observable<Group> {
    return this.http.post<Group>(this.apiRoot, group);
  }
}