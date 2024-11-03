import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "@environments/environment";
import { UnverifiedAnswer } from "@models/knowledge-check/UnverifiedAnswer";
import { GUID } from "@shared/types/GUID";
import { Observable } from "rxjs";

@Injectable({
  providedIn: 'root',
})
export class VerifyService {
  private apiRoot = `${environment.apiUrl}/api/Verify`;

  constructor(private http: HttpClient) { }

  getAnswersToVerify(knowledgeCheckId: GUID): Observable<UnverifiedAnswer[]> {
    return this.http.get<UnverifiedAnswer[]>(this.apiRoot, {
      params: {
        knowledgeCheckId: knowledgeCheckId,
      }
    });
  }

  postVerifyAnswer(body: any): Observable<boolean> {
    return this.http.post<boolean>(this.apiRoot, body);
  }
}