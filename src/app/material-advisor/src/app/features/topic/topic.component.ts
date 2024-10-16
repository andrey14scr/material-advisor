import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { TopicModel } from './models/Topic';
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
import { MatSelectModule } from '@angular/material/select';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { TopicGenerationService } from './services/topicGeneration.service';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { ConfirmDialogComponent } from '@shared/components/confirm-dialog/confirm-dialog.component';
import { LoaderComponent } from "@shared/components/loader/loader.component";

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
    LineSeparatorComponent,
    MatDialogModule,
    MatButtonModule,
    LoaderComponent
],
  templateUrl: './topic.component.html',
  styleUrls: ['./topic.component.scss']
})
export class TopicComponent implements OnInit, OnDestroy {
  currentTopic: TopicModel = {
    id: null,
    version: 0,
    name: [],
    questions: []
  };
  form: FormGroup;
  private fb = inject(FormBuilder);
  private route = inject(ActivatedRoute);

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
    private topicGenerationService: TopicGenerationService
  ) {
    this.form = this.fb.group({
      name: this.fb.array([]),
      questions: this.fb.array([]),
      maxQuestionsCount: [null],
      file: [null]
    });
  }

  ngOnInit() {
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
      version: this.currentTopic.version,
      name: this.form.value.name,
      questions: this.form.value.questions,
    };

    if (this.currentTopic?.id) {
      body.id = this.currentTopic.id;
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
      console.log('TopicGenerated fired')
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

  onGenerateSubmit() {
    this.isSubmittingGeneration = true;

    const formData = new FormData();
    if (this.form.value.maxQuestionsCount) {
      formData.append('MaxQuestionsCount', this.form.value.maxQuestionsCount);
    }
    formData.append('File', this.form.value.file);

    const topicNames = this.form.get('name')?.value;
    topicNames.forEach((item: any, index: number) => {
      formData.append(`TopicName[${index}].LanguageId`, item.languageId.toString());
      formData.append(`TopicName[${index}].Text`, item.text);
    });

    this.topicGenerationService.generateTopic(formData).subscribe({
      next: (response) => {
        this.snackBar.open('', 'Close', { duration: 2000 });
        this.router.navigate([`/topic/${response.id}`]);
      },
      error: (err) => {
        this.isSubmittingGeneration = false;
        this.snackBar.open('', 'Close', { duration: 2000 });
        console.error(err);
      }
    });;
  }

  onDeleteSubmit(): void {
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
}