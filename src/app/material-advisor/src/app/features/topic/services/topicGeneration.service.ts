import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "@environments/environment";
import { Observable } from "rxjs";
import { TopicModel } from "../models/Topic.model";


@Injectable({
  providedIn: 'root',
})
export class TopicGenerationService {
  private apiRoot = `${environment.apiUrl}/api/TopicGeneration`;

  constructor(private http: HttpClient) { }

  generateTopic(form: any): Observable<TopicModel> {
    return this.http.post<TopicModel>(this.apiRoot, form);
  }
}