import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "@environments/environment";
import { Attempt } from "@models/knowledge-check/Attempt";
import { StartedAttempt } from "@models/knowledge-check/StartedAttempt";
import { SubmittedAnswer } from "@models/knowledge-check/SubmittedAnswer";
import { GUID } from "@shared/types/GUID";
import { Observable } from "rxjs";

@Injectable({
  providedIn: 'root',
})
export class AttemptService {
  private apiRoot = `${environment.apiUrl}/api/Attempt`;

  constructor(private http: HttpClient) { }

  getLastAttempt(knowledgeCheckId: GUID): Observable<StartedAttempt> {
    return this.http.get<StartedAttempt>(this.apiRoot, {
      params: {
        knowledgeCheckId: knowledgeCheckId,
      }
    });
  }

  startAttempt(body: any): Observable<Attempt> {
    return this.http.post<Attempt>(`${this.apiRoot}/start`, body);
  }

  submitAnswer(body: any): Observable<SubmittedAnswer> {
    return this.http.post<SubmittedAnswer>(`${this.apiRoot}/submit-answer`, body);
  }

  submitAttempt(id: GUID): Observable<boolean> {
    return this.http.post<boolean>(`${this.apiRoot}/submit`, null, {
      params: {
        id: id,
      }
    });
  }
}