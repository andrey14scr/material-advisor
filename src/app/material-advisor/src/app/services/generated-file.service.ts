import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environments/environment';
import { Observable } from 'rxjs';
import { GUID } from '@shared/types/GUID';
import { GeneratedFile } from '@models/knowledge-check/GeneratedFile';

@Injectable({
  providedIn: 'root',
})
export class GeneratedFileService {
  private apiRoot = `${environment.apiUrl}/api/GeneratedFile`;

  constructor(private http: HttpClient) {}

  getByKnowledgeCheckId(knowledgeCheckId: GUID): Observable<GeneratedFile[]> {
    return this.http.get<GeneratedFile[]>(`${this.apiRoot}/knowledge-check/${knowledgeCheckId}`);
  }

  get(id: GUID): Observable<GeneratedFile> {
    return this.http.get<GeneratedFile>(`${this.apiRoot}/${id}`);
  }
}