import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environments/environment';
import { Observable } from 'rxjs';
import { KnowledgeCheck } from '../models/KnowledgeCheck';
import { GUID } from '@shared/types/GUID';

@Injectable({
  providedIn: 'root',
})
export class KnowledgeCheckService {
  private apiRoot = `${environment.apiUrl}/api/KnowledgeCheck`;

  constructor(private http: HttpClient) {}

  getKnowledgeChecks(topicId: GUID): Observable<KnowledgeCheck[]> {
    return this.http.get<KnowledgeCheck[]>(`${this.apiRoot}/topic/${topicId}`);
  }

  getKnowledgeCheck(id: GUID): Observable<KnowledgeCheck> {
    return this.http.get<KnowledgeCheck>(`${this.apiRoot}/${id}`);
  }

  postKnowledgeCheck(model: KnowledgeCheck): Observable<KnowledgeCheck> {
    return this.http.post<KnowledgeCheck>(this.apiRoot, model);
  }
}