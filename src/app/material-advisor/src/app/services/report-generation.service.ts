import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "@environments/environment";
import { GeneratedFile } from "@models/knowledge-check/GeneratedFile";
import { GUID } from "@shared/types/GUID";
import { Observable } from "rxjs";

@Injectable({
  providedIn: 'root',
})
export class ReportGenerationService {
  private apiRoot = `${environment.apiUrl}/api/ReportGeneration`;

  constructor(private http: HttpClient) { }

  generateReport(form: any): Observable<GeneratedFile> {
    return this.http.post<GeneratedFile>(this.apiRoot, form);
  }
}