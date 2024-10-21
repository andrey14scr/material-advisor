import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '@environments/environment';
import { GUID } from '@shared/types/GUID';
import { Topic } from '@models/topic/Topic';
import { TopicListItem } from '@models/topic/TopicListItem';

@Injectable({
  providedIn: 'root',
})
export class TopicService {
  private apiRoot = `${environment.apiUrl}/api/topic`;

  constructor(private http: HttpClient) {}

  getTopic(id: GUID): Observable<Topic> {
    return this.http.get<Topic>(`${this.apiRoot}/${id}`);
  }

  getTopics(): Observable<TopicListItem[]> {
    return this.http.get<TopicListItem[]>(this.apiRoot);
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