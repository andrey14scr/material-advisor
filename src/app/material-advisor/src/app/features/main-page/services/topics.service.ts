import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { TopicListItem } from '../models/TopicListItem';
import { environment } from '@environments/environment';

@Injectable({
  providedIn: 'root',
})
export class TopicsService {
  private apiRoot = `${environment.apiUrl}/api/topic`;

  constructor(private http: HttpClient) {}

  getTopics(): Observable<TopicListItem[]> {
    return this.http.get<TopicListItem[]>(this.apiRoot);
  }
}
