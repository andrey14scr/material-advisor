import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { TopicModel } from './models/Topic.model';
import { MatCardModule } from '@angular/material/card';
import { TopicService } from './services/topic.service';
import { TextsInputComponent } from "@shared/components/texts-input/texts-input.component";
import { QuestionsInputComponent } from './components/questions-input/questions-input.component';
import * as signalR from '@aspnet/signalr';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatOptionModule } from '@angular/material/core';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { CommonModule } from '@angular/common';
import { environment } from '@environments/environment';
import { AuthService } from '@shared/services/auth.service';
import { LineSeparatorComponent } from "../../shared/components/line-separator/line-separator.component";
import { Language } from '@shared/types/Language';
import { TranslationService } from '@shared/services/translation.service';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { TopicGenerationService } from './services/topicGeneration.service';

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
    MatInputModule,
    MatButtonModule,
    MatFormFieldModule,
    MatCardModule,
    MatOptionModule,
    MatProgressSpinnerModule,
    MatSelectModule,
    MatButtonToggleModule,
    TextsInputComponent,
    QuestionsInputComponent,
    LineSeparatorComponent
],
  templateUrl: './topic.component.html',
  styleUrls: ['./topic.component.scss']
})
export class TopicComponent implements OnInit, OnDestroy {
  currentTopic: TopicModel = new TopicModel(null, 0, 0, [], []);
  form: FormGroup;
  private fb = inject(FormBuilder);
  private route = inject(ActivatedRoute);

  isSubmittingGeneration: boolean = false;
  hubConnection!: signalR.HubConnection;
  allLanguages: Language[] = [];

  selectedMode = TopicCreationMode.Generate;
  readonly TopicCreationMode: typeof TopicCreationMode = TopicCreationMode;

  constructor(private topicService: TopicService, 
    private router: Router, 
    private snackBar: MatSnackBar, 
    private authService: AuthService,
    private translationService: TranslationService,
    private topicGenerationService: TopicGenerationService) {
    this.form = this.fb.group({
      name: this.fb.array([]),
      questions: this.fb.array([]),
      maxQuestionsCount: [null],
      languages: [[]],
      file: [null]
    });

    this.form.valueChanges.subscribe((formValues) => {
      this.currentTopic.name = formValues.name;
      this.currentTopic.questions = formValues.questions;
    });
  }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.getTopicById(id);
    }

    this.translationService.getLanguages().subscribe(lang => {
      this.allLanguages.push(lang);
    });

    this.initializeSignalRConnection();
  }

  ngOnDestroy() {
    this.hubConnection.stop();
  }

  get textsFormArray() {
    return this.form.get('name') as FormArray;
  }

  get questionsFormArray() {
    return this.form.get('questions') as FormArray;
  }

  getTopicById(id: string): void {
    this.topicService.getTopic(id).subscribe({
      next: (data) => {
        this.currentTopic = data;
        this.form.patchValue({
          name: this.currentTopic.name,
        });
      },
      error: (error) => {
        console.error('Error fetching the topic', error);
      }
    });
  }

  onCreateSubmit(): void {
    this.currentTopic.number = this.form.value.number;

    const { id, ...withoutId } = this.currentTopic;

    const body = this.currentTopic.id ? this.currentTopic : withoutId;

    console.log(body);

    // this.topicService.postTopic(body).subscribe({
    //   next: () => {
    //     this.router.navigate(['/main-page']);
    //   },
    //   error: (error) => {
    //     console.error('Error updating topic', error);
    //   }
    // });
  }

  ///////////////////////////////////////////////////////////////////////////////////////

  initializeSignalRConnection() {
    const accessToken = this.authService.getAccessToken() ?? '';

    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${environment.apiUrl}/topic-generation-hub`, {
        accessTokenFactory: () => accessToken
      })
      .build();

    this.hubConnection.on('TopicGenerated', (topicId: string, status: any) => {
      console.log('Received:', topicId, status);
      this.isSubmittingGeneration = false;
      this.snackBar.open('', 'Close', { duration: 2000 });
    });

    this.hubConnection.start()
      .then(() => console.log('SignalR connection started'))
      .catch(err => console.log('Error establishing SignalR connection:', err));
  }

  onFileChange(event: any) {
    const file = (event.target as HTMLInputElement).files?.[0];
    this.form.patchValue({ file: file });
  }

  getLanguageName(langCode: string){
    return this.translationService.translate(`languages.${langCode}`);
  }

  onGenerateSubmit() {
    const formData = new FormData();
    formData.append('MaxQuestionsCount', this.form.value.maxQuestionsCount);
    formData.append('File', this.form.value.file);

    const topicNames = this.form.get('name')?.value;
    topicNames.forEach((item: any, index: number) => {
      formData.append(`TopicName[${index}].LanguageId`, item.languageId.toString());
      formData.append(`TopicName[${index}].Text`, item.text);
    });

    const languages = this.form.get('languages')?.value;
    languages.forEach((item: any, index: number) => {
      formData.append(`Languages[]`, item.toString());
    });

    this.topicGenerationService.generateTopic(formData).subscribe({
      next: () => {
        this.isSubmittingGeneration = true;
        console.log('Form submitted successfully');
      },
      error: (err) => {
        this.isSubmittingGeneration = false;
        this.snackBar.open('', 'Close', { duration: 2000 });
        console.error(err);
      }
    });;
  }
}