import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '@environments/environment';
import { GUID } from '@shared/types/GUID';
import { Topic } from '@models/topic/Topic';
import { TopicListItem } from '@models/topic/TopicListItem';
import { KnowledgeCheckTopic } from '@models/knowledge-check/topic/KnowledgeCheckTopic';
import { KnowledgeCheckListItem } from '@models/knowledge-check/KnowledgeCheckListItem';
import { KnowledgeCheckTopicListItem } from '@models/knowledge-check/KnowledgeCheckTopicListItem';

@Injectable({
  providedIn: 'root',
})
export class TopicService {
  private apiRoot = `${environment.apiUrl}/api/topic`;

  constructor(private http: HttpClient) {}

  getTopic(id: GUID): Observable<Topic> {
    return this.http.get<Topic>(`${this.apiRoot}/${id}`);
  }

  getTopicListItem(id: GUID): Observable<TopicListItem<KnowledgeCheckListItem>> {
    return this.http.get<TopicListItem<KnowledgeCheckListItem>>(`${this.apiRoot}/list-item/${id}`);
  }

  getKnowledgeCheckTopic(id: GUID): Observable<KnowledgeCheckTopic> {
    return this.http.get<KnowledgeCheckTopic>(`${this.apiRoot}/${id}/knowledge-check-topic`);
  }

  getTopics(): Observable<TopicListItem<KnowledgeCheckTopicListItem>[]> {
    return this.http.get<TopicListItem<KnowledgeCheckTopicListItem>[]>(this.apiRoot);
  }

  postTopic(topic: any): Observable<Topic> {
    return this.http.post<Topic>(this.apiRoot, topic);
  }

  deleteTopic(id: GUID): Observable<boolean> {
    return this.http.delete<boolean>(this.apiRoot, {
      params: {
        id: id,
      }
    });
  }
}