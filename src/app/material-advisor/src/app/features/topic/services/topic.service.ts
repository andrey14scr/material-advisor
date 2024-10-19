import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '@environments/environment';
import { TopicModel } from '../models/Topic';
import { GUID } from '@shared/types/GUID';

@Injectable({
  providedIn: 'root',
})
export class TopicService {
  private apiRoot = `${environment.apiUrl}/api/topic`;

  constructor(private http: HttpClient) {}

  getTopic(id: GUID): Observable<TopicModel> {
    return this.http.get<TopicModel>(`${this.apiRoot}/${id}`);
  }

  postTopic(topic: any): Observable<TopicModel> {
    return this.http.post<TopicModel>(this.apiRoot, topic);
  }

  deleteTopic(id: GUID): Observable<boolean> {
    return this.http.delete<boolean>(this.apiRoot, {
      params: {
        id: id,
      }
    });
  }
}