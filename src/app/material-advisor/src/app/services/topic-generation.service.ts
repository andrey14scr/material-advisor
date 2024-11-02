import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "@environments/environment";
import { Topic } from "@models/topic/Topic";
import { Observable } from "rxjs";

@Injectable({
  providedIn: 'root',
})
export class TopicGenerationService {
  private apiRoot = `${environment.apiUrl}/api/TopicGeneration`;

  constructor(private http: HttpClient) { }

  generateTopic(form: any): Observable<Topic> {
    return this.http.post<Topic>(this.apiRoot, form);
  }
}