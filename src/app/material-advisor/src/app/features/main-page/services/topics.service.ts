import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { TopicListItemModel } from '../models/TopicListItem.model';
import { environment } from '@environments/environment';
import { GUID } from '@shared/types/GUID';

@Injectable({
  providedIn: 'root',
})
export class TopicsService {
  private apiRoot = `${environment.apiUrl}/api/topic`;

  constructor(private http: HttpClient) {}

  getTopics(): Observable<TopicListItemModel[]> {
    return this.http.get<TopicListItemModel[]>(this.apiRoot);
  }
}
