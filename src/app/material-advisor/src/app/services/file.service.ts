import { HttpClient, HttpResponse } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "@environments/environment";
import { Observable } from "rxjs";

@Injectable({
  providedIn: 'root',
})
export class FileService {
  private apiRoot = `${environment.apiUrl}/api/File`;

  constructor(private http: HttpClient) { }

  download(file: string): Observable<HttpResponse<Blob>> {
    return this.http.get<Blob>(this.apiRoot, {
      observe: 'response',
      responseType: 'blob' as 'json',
      params: {
        file: file,
      }
    });
  }
}