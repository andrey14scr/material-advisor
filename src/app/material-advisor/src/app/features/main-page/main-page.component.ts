import { Component } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { TranslationService } from '@shared/services/translation.service';
import { map } from 'rxjs';
import { LanguageText } from '@shared/models/LanguageText';
import { AuthService } from '@shared/services/auth.service';
import { Router } from '@angular/router';
import { GUID } from '@shared/types/GUID';
import { ConfirmDialogComponent } from '@shared/components/confirm-dialog/confirm-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { MaterialModule } from '@shared/modules/matetial/material.module';
import { TopicListItem } from '@models/topic/TopicListItem';
import { TopicService } from '@services/topic.service';
import { KnowledgeCheckListItem } from '@models/knowledge-check/KnowledgeCheckListItem';
import { sortByStartDate } from '@shared/services/sort-utils.service';
import { KnowledgeCheckService } from '@services/knowledge-check.service';
import { KnowledgeCheckConfirmDialogComponent } from './components/knowledge-check-confirm-dialog/knowledge-check-confirm-dialog.component';
import * as signalR from '@aspnet/signalr';
import { environment } from '@environments/environment';
import { MatSnackBar } from '@angular/material/snack-bar';
import { KnowledgeCheckTopicListItem } from '@models/knowledge-check/KnowledgeCheckTopicListItem';
import { KnowledgeCheckCreateDialogComponent } from '@features/knowledge-check-create-dialog/knowledge-check-create-dialog.component';
import { toFullTimeFormat } from '@shared/services/format-utils.service';
import { ReportGenerationDialogComponent } from './components/report-generation-dialog/report-generation-dialog.component';

@Component({
  selector: 'app-main-page',
  standalone: true,
  imports: [CommonModule, MaterialModule],
  providers: [DatePipe],
  templateUrl: './main-page.component.html',
  styleUrl: './main-page.component.scss'
})
export class MainPageComponent {
  topics: TopicListItem<KnowledgeCheckListItem>[] = [];
  ownedTopics: TopicListItem<KnowledgeCheckTopicListItem>[] = [];
  currentTag?: string;
  hubConnection!: signalR.HubConnection;

  MAX_MINUTES_FROM_UNLOADED_GENERATION = 15;
  
  constructor(private translationService: TranslationService, 
    private topicService: TopicService,
    private authService: AuthService,
    private dialog: MatDialog,
    private router: Router,
    private knowledgeCheckService: KnowledgeCheckService,
    private datePipe: DatePipe,
    private snackBar: MatSnackBar,
  ) {}

  ngOnInit() {
    this.initializeSignalRConnection();
    this.getTopicsList();
  }

  getTopicsList() {
    this.topicService.getTopics()
      .pipe(
        map(data => 
          data
            .filter(t => t.version > 0 || 
              t.generatedAt && ((new Date().getTime() - t.generatedAt.getTime() / 1000) / 60 < this.MAX_MINUTES_FROM_UNLOADED_GENERATION))
            .map(item => ({
              ...item,
              knowledgeChecks: sortByStartDate(item.knowledgeChecks)
            }))
        )
      )
      .subscribe(
        topics => {
          this.ownedTopics = topics;
        }
      );

    this.knowledgeCheckService.getKnowledgeChecks()
      .pipe(
        map(data => 
          data.map(item => ({
            ...item,
            knowledgeChecks: sortByStartDate(item.knowledgeChecks)
          }))
        )
      )
      .subscribe(
        topics => {
          this.topics = topics;
        }
      );
  }

  initializeSignalRConnection() {
    const accessToken = this.authService.getAccessToken() ?? '';

    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${environment.apiUrl}/topic-generation-hub`, {
        accessTokenFactory: () => accessToken
      })
      .build();

    this.hubConnection.on('TopicGenerated', (topicId: string, status: any) => {
      this.updateTopic(topicId);
      this.snackBar.open('', 'Close', { duration: 2000 });
    });

    this.hubConnection.start()
      .then(() => console.log('SignalR connection started'))
      .catch(err => console.error('Error establishing SignalR connection:', err));
  }

  updateTopic(id: GUID) {
    this.topicService.getTopicListItem(id).subscribe(
      data => {
        const topicToUpdate = this.topics.find(t => t.id === id);
        if (topicToUpdate) {
          topicToUpdate.name = data.name;
          topicToUpdate.version = data.version;
        }
      }
    );
  }

  translate(key: string){
    return this.translationService.translate(key);
  }

  translateText(texts: LanguageText[]): string {
    return this.translationService.translateText(texts);
  }

  navigateToTopicDetails(topicId: GUID) {
    this.router.navigate([`/topic/${topicId}`]);
  }

  navigateToKnowledgeCheckDetails(knowledgeCheckId: GUID) {
    this.router.navigate([`/knowledge-check/${knowledgeCheckId}`]);
  }

  isKnowledgeCheckActive(model: KnowledgeCheckListItem): boolean {
    const now = new Date();
    return model.startDate >= now 
      && (!model.endDate || model.endDate >= now) 
      && (!model.maxAttempts || (model.usedAttempts ?? 0) < model.maxAttempts);
  }

  deleteTopic(id: GUID) {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: '300px',
      data: { message: 'Are you sure you want to proceed?' }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.topicService.deleteTopic(id).subscribe({
          next: (response) => {
            if (response) {
              this.topics = this.topics.filter(topic => topic.id !== id);
            }
            else {
              console.error('Topic was not deleted');
            }
          },
          error: (error) => {
            console.error('Error deleting topic', error);
          }
        });
      }
    });
  }

  openKnowledgeCheckDialog(topic: TopicListItem<KnowledgeCheckTopicListItem>, id: GUID) {
    const dialogRef = this.dialog.open(KnowledgeCheckCreateDialogComponent, {
      width: '600px',
      data: { id },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        result.topicId = topic.id;
        this.knowledgeCheckService.postKnowledgeCheck(result).subscribe((knowledgeCheck) => {
          if (knowledgeCheck) {
            this.getTopicsList();
          }
        });
      }
    });
  }

  deleteKnowledgeCheck(topic: TopicListItem<KnowledgeCheckTopicListItem>, id: GUID) {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: '300px',
      data: { message: 'Are you sure you want to proceed?' }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.knowledgeCheckService.deleteKnowledgeCheck(id).subscribe({
          next: (response) => {
            if (response) {
              topic.knowledgeChecks = topic.knowledgeChecks.filter(knowledgeCheck => knowledgeCheck.id !== id);
            }
            else {
              console.error('Topic was not deleted');
            }
          },
          error: (error) => {
            console.error('Error deleting topic', error);
          }
        });
      }
    });
  }

  isActual(knowledgeCheck: KnowledgeCheckListItem): boolean{
    const now = new Date();
    const startDate = new Date(knowledgeCheck.startDate);
    const endDate = knowledgeCheck.endDate ? new Date(knowledgeCheck.endDate) : null;

    const isActualDate = startDate <= now && (!endDate || endDate >= now);

    const usedAttempts = knowledgeCheck.usedAttempts ?? 0;
    const maxAttempts = knowledgeCheck.maxAttempts;

    const isActualAttempts = !maxAttempts ? true : usedAttempts < maxAttempts;

    return isActualDate && isActualAttempts;
  }

  getDateLabel(knowledgeCheck: KnowledgeCheckListItem): string {
    const now = new Date();
    const startDate = new Date(knowledgeCheck.startDate);
    const endDate = knowledgeCheck.endDate ? new Date(knowledgeCheck.endDate) : null;

    const dateTimeFormat = 'dd.MM.yyyy HH:mm';

    if (startDate > now) {
      return `Starts on: ${this.datePipe.transform(startDate, dateTimeFormat)}`;
    }
    else {
      if (endDate && endDate >= now) {
        return `Ends on: ${this.datePipe.transform(endDate, dateTimeFormat)}`;
      }
      else if (endDate) {
        return `Ended on: ${this.datePipe.transform(endDate, dateTimeFormat)}`;
      }
      else {
        return 'Without date bounds';
      }
    }
  }

  getAttemptsLabel(knowledgeCheck: KnowledgeCheckListItem): string {
    const usedAttempts = knowledgeCheck.usedAttempts ?? 0;
    const maxAttempts = knowledgeCheck.maxAttempts;

    const now = new Date();
    const startDate = new Date(knowledgeCheck.startDate);
    const endDate = knowledgeCheck.endDate ? new Date(knowledgeCheck.endDate) : null;

    if (endDate && endDate < now) {
      if (usedAttempts) {
        return `Used attempts: ${usedAttempts}`;
      }
      else {
        return 'Not passed';
      }
    }

    if (startDate > now && maxAttempts) {
      return `Attempts: ${maxAttempts}`;
    }
    
    if (maxAttempts) {
      if (maxAttempts === usedAttempts) {
        return 'No attempts left';
      }
      return `Attempts left: ${maxAttempts - usedAttempts}/${maxAttempts}`;
    }

    if (usedAttempts) {
      return `Used attempts: ${usedAttempts}`;
    }

    return 'Not passed yet';
  }

  getTimeLabel(time: number): string {
    const formattedTime = toFullTimeFormat(time);
    if (formattedTime) {
      return `allotted time: ${formattedTime}`;
    }

    return 'without time limits';
  }

  startKnowledgeCheck(topic: TopicListItem<KnowledgeCheckListItem>, knowledgeCheck: KnowledgeCheckListItem) {
    const dialogRef = this.dialog.open(KnowledgeCheckConfirmDialogComponent, {
      width: '500px',
      data: { topic, knowledgeCheck }
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.router.navigate([`knowledge-check/${knowledgeCheck.id}`]);
      }
    });
  }

  isLoadingTopic(topic: TopicListItem<KnowledgeCheckTopicListItem>): boolean {
    return !topic.version;
  }

  openKnowledgeCheckVerifyPage(knowledgeCheckId: GUID) {
    this.router.navigate([`answer-verification/${knowledgeCheckId}`]);
  }
  
  downloadReport(knowledgeCheck: KnowledgeCheckTopicListItem) {
    const data = knowledgeCheck;
    const dialogRef = this.dialog.open(ReportGenerationDialogComponent, {
      width: '500px',
      data: data
    });
  }
}
