import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "@environments/environment";
import { Observable } from "rxjs";
import { Group } from "../models/Group";

@Injectable({
  providedIn: 'root',
})
export class GroupService {
  private apiRoot = `${environment.apiUrl}/api/Group`;

  constructor(private http: HttpClient) { }

  getGroupsAsOwner(): Observable<Group[]> {
    return this.http.get<Group[]>(`${this.apiRoot}/owner`);
  }
}