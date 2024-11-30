import { CommonModule } from '@angular/common';
import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { GeneratedFile } from '@models/knowledge-check/GeneratedFile';
import { KnowledgeCheckTopicListItem } from '@models/knowledge-check/KnowledgeCheckTopicListItem';
import { FileService } from "@services/file.service";
import { GeneratedFileService } from '@services/generated-file.service';
import { ReportGenerationService } from '@services/report-generation.service';
import { MaterialModule } from '@shared/modules/matetial/material.module';
import * as signalR from '@aspnet/signalr';
import { AuthService } from '@shared/services/auth.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { environment } from '@environments/environment';
import { GUID } from '@shared/types/GUID';
import { LoaderComponent } from "@shared/components/loader/loader.component";
import { TranslationService } from '@shared/services/translation.service';

@Component({
  selector: 'report-generation-dialog',
  standalone: true,
  imports: [CommonModule, MaterialModule, LoaderComponent],
  templateUrl: './report-generation-dialog.component.html',
  styleUrl: './report-generation-dialog.component.scss'
})
export class ReportGenerationDialogComponent implements OnInit {
  hubConnection!: signalR.HubConnection;

  isLoading: boolean;
  items: {file: GeneratedFile, isLoading: boolean}[] = [];
  knowledgeCheck: KnowledgeCheckTopicListItem;

  constructor(
    public dialogRef: MatDialogRef<ReportGenerationDialogComponent>,
    private fileService: FileService,
    private translationService: TranslationService,
    private generatedFileService: GeneratedFileService,
    private reportGenerationService: ReportGenerationService,
    private authService: AuthService,
    private snackBar: MatSnackBar,
    @Inject(MAT_DIALOG_DATA) public data: KnowledgeCheckTopicListItem,
  ) {
    this.knowledgeCheck = data;
    this.isLoading = true;
  }

  ngOnInit() {
    this.initializeSignalRConnection();
    this.getGeneratedFiles();
  }

  initializeSignalRConnection() {
    const accessToken = this.authService.getAccessToken() ?? '';

    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${environment.apiUrl}/report-generation-hub`, {
        accessTokenFactory: () => accessToken
      })
      .build();

    this.hubConnection.on('ReportGenerated', (ids: GUID[], status: any) => {
      this.getGeneratedFiles();
      this.snackBar.open(this.t('popupNotifications.reportGenerated'), 'Close', { duration: 2000 });
    });

    this.hubConnection.start()
      .then(() => console.log('SignalR connection started'))
      .catch(err => console.error('Error establishing SignalR connection:', err));
  }

  getGeneratedFiles() {
    this.generatedFileService.getByKnowledgeCheckId(this.knowledgeCheck.id).subscribe({
      next: data => {
        this.items = data.map(x => ({file: x, isLoading: false}));
        this.isLoading = false;
      }
    });
  }

  onDownload(item: {file: GeneratedFile, isLoading: boolean}) {
    item.isLoading = true;
    this.fileService.download(item.file.file!).subscribe({
      next: (blob) => {
        const fileName = blob.headers.get('x-file-name');
        if (!fileName) {
          console.error('Download failed. File name was empty');
          return;
        }

        const url = window.URL.createObjectURL(blob.body!);
        const a = document.createElement('a');
        a.href = url;
        a.download = fileName;
        a.click();
        window.URL.revokeObjectURL(url);

        item.isLoading = false;
      },
      error: (err) => {
        console.error('Download failed', err);
      }
    });
  }

  onCancel() {
    this.dialogRef.close(false);
  }

  onCreateNew() {
    this.reportGenerationService.generateReport({
      knowledgeCheckId: this.knowledgeCheck.id
    }).subscribe({
      next: x =>{
        this.items.unshift({file: x, isLoading: false});
      },
      error: (err) => {
        console.error('Report generation failed', err);
      }
    });
  }

  t(key: string): string {
    return this.translationService.translate(key);
  }
}
