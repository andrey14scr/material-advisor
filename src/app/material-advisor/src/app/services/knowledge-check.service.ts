import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environments/environment';
import { map, Observable } from 'rxjs';
import { GUID } from '@shared/types/GUID';
import { sortByStartDate } from '@shared/services/sort-utils.service';
import { KnowledgeCheckListItem } from '@models/knowledge-check/KnowledgeCheckListItem';
import { KnowledgeCheck } from '@models/knowledge-check/KnowledgeCheck';
import { TopicListItem } from '@models/topic/TopicListItem';

@Injectable({
  providedIn: 'root',
})
export class KnowledgeCheckService {
  private apiRoot = `${environment.apiUrl}/api/KnowledgeCheck`;

  constructor(private http: HttpClient) {}

  getByTopicId(topicId: GUID): Observable<KnowledgeCheckListItem[]> {
    return this.http.get<KnowledgeCheckListItem[]>(`${this.apiRoot}/topic/${topicId}`).pipe(
      map((data: KnowledgeCheckListItem[]) => sortByStartDate(data))
    );
  }

  getKnowledgeChecks(): Observable<TopicListItem<KnowledgeCheckListItem>[]> {
    return this.http.get<TopicListItem<KnowledgeCheckListItem>[]>(this.apiRoot);
  }

  getKnowledgeCheck(id: GUID): Observable<KnowledgeCheck> {
    return this.http.get<KnowledgeCheck>(`${this.apiRoot}/${id}`);
  }

  postKnowledgeCheck(model: KnowledgeCheck): Observable<KnowledgeCheck> {
    return this.http.post<KnowledgeCheck>(this.apiRoot, model);
  }

  deleteKnowledgeCheck(id: GUID): Observable<boolean> {
    return this.http.delete<boolean>(this.apiRoot, {
      params: {
        id: id,
      }
    });
  }
}