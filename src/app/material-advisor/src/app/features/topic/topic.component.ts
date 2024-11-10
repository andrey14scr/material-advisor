import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { TextsInputComponent } from "@shared/components/texts-input/texts-input.component";
import { QuestionsInputComponent } from './components/questions-input/questions-input.component';
import * as signalR from '@aspnet/signalr';
import { CommonModule } from '@angular/common';
import { environment } from '@environments/environment';
import { AuthService } from '@shared/services/auth.service';
import { LineSeparatorComponent } from "../../shared/components/line-separator/line-separator.component";
import { ConfirmDialogComponent } from '@shared/components/confirm-dialog/confirm-dialog.component';
import { LoaderComponent } from "@shared/components/loader/loader.component";
import { MaterialModule } from '@shared/modules/matetial/material.module';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatDialog } from '@angular/material/dialog';
import { KnowledgeChecksComponent } from './components/knowledge-check-list/knowledge-check-list.component';
import { Topic } from '@models/topic/Topic';
import { TopicGenerationService } from '@services/topic-generation.service';
import { TopicService } from '@services/topic.service';
import { Language } from '@shared/types/Language';
import { TranslationService } from '@shared/services/translation.service';
import { LanguageEnum } from '@shared/types/LanguageEnum';
import { LanguageText } from '@shared/models/LanguageText';
import { QuestionsTemplateInputComponent } from "./components/questions-template-input/questions-template-input.component";
import { FileInputComponent } from "../../shared/components/file-input/file-input.component";

export enum TopicCreationMode {
  Generate,
  Create
}

@Component({
  selector: 'topic',  
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    TextsInputComponent,
    QuestionsInputComponent,
    LineSeparatorComponent,
    LoaderComponent,
    MaterialModule,
    KnowledgeChecksComponent,
    QuestionsTemplateInputComponent,
    FileInputComponent
],
  templateUrl: './topic.component.html',
  styleUrls: ['./topic.component.scss']
})
export class TopicComponent implements OnInit, OnDestroy {
  currentTopic: Topic = {
    id: null,
    version: 0,
    name: [],
    questions: []
  };

  form: FormGroup;
  private fb = inject(FormBuilder);
  private route = inject(ActivatedRoute);

  languages: Language[] = [];
  isSubmittingGeneration: boolean = false;
  hubConnection!: signalR.HubConnection;

  selectedMode = TopicCreationMode.Generate;
  readonly TopicCreationMode: typeof TopicCreationMode = TopicCreationMode;

  isLoading: boolean = true;

  constructor(
    private topicService: TopicService,
    private router: Router,
    private snackBar: MatSnackBar,
    private dialog: MatDialog,
    private authService: AuthService,
    private topicGenerationService: TopicGenerationService,
    private translationService: TranslationService,
  ) {
    this.form = this.fb.group({
      name: this.fb.array([]),
      questions: this.fb.array([]),
      questionsStructure: this.fb.array([]),
      defaultAnswersCount: [null],
      doesComplexityIncrease: [false],
      cultureContext: [null],
      languages: [[]],
      file: [null]
    });

    this.form.get('languages')?.disable();
  }

  ngOnInit() {
    this.translationService.getLanguages().subscribe(languages => {
      this.languages = languages;
      this.form.get('languages')?.enable();
    });

    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.getTopicById(id);
    }
    else {
      this.initializeSignalRConnection();
      this.isLoading = false;
    }
  }

  ngOnDestroy() {
    this.hubConnection?.stop();
  }

  get questionsFormArray() {
    return this.form.get('questions') as FormArray;
  }

  getTopicById(id: string) {
    this.topicService.getTopic(id).subscribe({
      next: (data) => {
        this.currentTopic = data;
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error fetching the topic', error);
      }
    });
  }

  onCreateSubmit() {
    let body: any = {
      name: this.form.value.name,
      questions: this.form.value.questions,
    };

    if (this.currentTopic?.id) {
      body.id = this.currentTopic.id;
      body.version = this.currentTopic.version;
    }

    this.topicService.postTopic(body).subscribe({
      next: (response) => {
        this.snackBar.open('', 'Close', { duration: 2000 });
        this.router.navigate([`/topic/${response.id}`]);
      },
      error: (error) => {
        console.error('Error updating topic', error);
      }
    });
  }

  initializeSignalRConnection() {
    const accessToken = this.authService.getAccessToken() ?? '';

    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${environment.apiUrl}/topic-generation-hub`, {
        accessTokenFactory: () => accessToken
      })
      .build();

    this.hubConnection.on('TopicGenerated', (topicId: string, status: any) => {
      this.isSubmittingGeneration = false;
      if (status === 'TopicGenerated') {
        this.router.navigate([`/topic/${topicId}`]);
        this.snackBar.open('', 'Close', { duration: 2000 });
      }
      else {
        this.snackBar.open('', 'Close', { duration: 2000 });
      }
    });

    this.hubConnection.start()
      .then(() => console.log('SignalR connection started'))
      .catch(err => console.error('Error establishing SignalR connection:', err));
  }

  onFileChange(event: any) {
    const file = (event.target as HTMLInputElement).files?.[0];
    this.form.patchValue({ file: file });
  }

  onGenerateSubmit() {
    this.isSubmittingGeneration = true;
    console.log('lol', this.form.value);

    const formData = new FormData();
    
    if (this.form.value.defaultAnswersCount) {
      formData.append('DefaultAnswersCount', this.form.value.defaultAnswersCount);
    }
    if (this.form.value.cultureContext) {
      formData.append('CultureContext', this.form.value.cultureContext);
    }

    formData.append('DoesComplexityIncrease', this.form.value.doesComplexityIncrease);
    formData.append('File', this.form.value.file);

    const topicNames = this.form.value.name;
    topicNames.forEach((item: any, index: number) => {
      formData.append(`TopicName[${index}].LanguageId`, item.languageId);
      formData.append(`TopicName[${index}].Text`, item.text);
    });

    const questionsStructure = this.form.value.questionsStructure;
    questionsStructure.forEach((item: any, index: number) => {
      formData.append(`QuestionsStructure[${index}].Count`, item.count);
      formData.append(`QuestionsStructure[${index}].QuestionType`, item.type);
      if (item.answersCount) {
        formData.append(`QuestionsStructure[${index}].AnswersCount`, item.answersCount);
      }
    });

    const languages = this.form.value.languages;
    languages.forEach((item: any, index: number) => {
      formData.append(`Languages[${index}]`, item);
    });

    this.topicGenerationService.generateTopic(formData).subscribe({
      next: (response) => {
        this.router.navigate([`/main-page`]);
        this.snackBar.open('', 'Close', { duration: 2000 });
      },
      error: (err) => {
        this.isSubmittingGeneration = false;
        this.snackBar.open('', 'Close', { duration: 2000 });
        console.error(err);
      }
    });
  }

  onDeleteSubmit() {
    if (!this.currentTopic.id) {
      return;
    }

    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: '300px',
      data: { message: 'Are you sure you want to proceed?' }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.topicService.deleteTopic(this.currentTopic.id!).subscribe({
          next: (response) => {
            if (response) {
              this.router.navigate([`/main-page`]);
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

  getLanguageName(langCode: string): string {
    return this.translationService.translate(`languages.${langCode}`);
  }

  isAdditioanlLanguageChosen(languageId: LanguageEnum): boolean {
    return (this.form.value.name as LanguageText[]).filter((x: LanguageText) => x.languageId === languageId).length !== 0;
  }
}