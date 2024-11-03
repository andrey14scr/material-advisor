import { CommonModule } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { KnowledgeCheck } from '@models/knowledge-check/KnowledgeCheck';
import { KnowledgeCheckService } from '@services/knowledge-check.service';
import { MaterialModule } from '@shared/modules/matetial/material.module';
import { AuthService } from '@shared/services/auth.service';
import { interval } from 'rxjs';
import { QuestionSingleSelectComponent } from "./components/question-single-select/question-single-select.component";
import { QuestionMultipleSelectComponent } from "./components/question-multiple-select/question-multiple-select.component";
import { QuestionOpenComponent } from "./components/question-open/question-open.component";
import { QuestionType } from '@shared/types/QuestionEnum';
import { KnowledgeCheckTopic } from '@models/knowledge-check/topic/KnowledgeCheckTopic';
import { TopicService } from '@services/topic.service';
import { GUID } from '@shared/types/GUID';
import { AttemptService } from '@services/attempt.service';
import { LoaderComponent } from "@shared/components/loader/loader.component";
import { LanguageText } from '@shared/models/LanguageText';
import { TranslationService } from '@shared/services/translation.service';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmDialogComponent } from '@shared/components/confirm-dialog/confirm-dialog.component';
import { HttpErrorResponse } from '@angular/common/http';
import { StartedAttempt } from '@models/knowledge-check/StartedAttempt';
import { AttemptAnswer } from '@models/knowledge-check/AttemptAnswer';
import { KnowledgeCheckQuestion } from '@models/knowledge-check/topic/KnowledgeCheckQuestion';
import { toFullTimeFormat } from '@shared/services/format-utils.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-knowledge-check',
  standalone: true,
  imports: [CommonModule, MaterialModule, QuestionSingleSelectComponent, QuestionMultipleSelectComponent, QuestionOpenComponent, LoaderComponent],
  templateUrl: './knowledge-check.component.html',
  styleUrl: './knowledge-check.component.scss'
})
export class KnowledgeCheckComponent implements OnInit, OnDestroy {
  questionTypeRef = QuestionType;
  
  isLoading = true;
  attempt!: StartedAttempt;
  knowledgeCheck!: KnowledgeCheck;
  topic!: KnowledgeCheckTopic;
  remainingTime!: number;

  MIN_TIME_TO_FINISH = 60;

  constructor(private route: ActivatedRoute, 
    private dialog: MatDialog,
    private router: Router,
    private knowledgeCheckService: KnowledgeCheckService, 
    private topicService: TopicService, 
    private attemptService: AttemptService, 
    private translationService: TranslationService, 
    private snackBar: MatSnackBar,
    private authService: AuthService,
  ) { }

  ngOnDestroy(): void {
    if (this.remainingTime < this.MIN_TIME_TO_FINISH) {
      this.submitAttempt();
    }
  }

  ngOnInit() {
    this.initializeKnowledgeCheck();
  }

  initializeKnowledgeCheck() {
    const id = this.route.snapshot.paramMap.get('id')!;
    
    this.knowledgeCheckService.getKnowledgeCheck(id).subscribe((knowledgeCheck) => {
      this.knowledgeCheck = knowledgeCheck;
      this.initializeTopic(knowledgeCheck);
    });
  }

  initializeTopic(knowledgeCheck: KnowledgeCheck) {
    this.topicService.getKnowledgeCheckTopic(knowledgeCheck.topicId).subscribe((topic) => {
      this.topic = topic;
      this.startAtttempt(knowledgeCheck.id);
    });
  }

  startAtttempt(knowledgeCheckId: GUID) {
    this.attemptService.getLastAttempt(knowledgeCheckId).subscribe({
      next: (startedAttempt) => {
        this.attempt = startedAttempt;
        let seconds = ((new Date()).getTime() - new Date(startedAttempt.startDate).getTime()) / 1000;
        this.remainingTime = this.knowledgeCheck.time - seconds;
        this.startTimer();
        this.isLoading = false;
      },
      error: (error: HttpErrorResponse) => {
        if (error.status === 404) {
          let body = { knowledgeCheckId: knowledgeCheckId};
          this.attemptService.startAttempt(body).subscribe((attempt) => {
            this.attempt =  {...attempt, submittedAnswers: []};
            this.remainingTime = this.knowledgeCheck.time;
            this.startTimer();
            this.isLoading = false;
          })
        }
        else {
          console.error('Unhandled error', error);
        }
      }
    });
  }

  getTimeLabel(time: number): string {
    return toFullTimeFormat(time);
  }

  startTimer() {
    const wholeSeconds = Math.floor(this.remainingTime);
    const fractionalPart = this.remainingTime - wholeSeconds;

    setTimeout(() => { 
      this.remainingTime = wholeSeconds; 
    }, fractionalPart);

    const timer$ = interval(1000);
    timer$.subscribe(() => {
      if (this.remainingTime > 0) {
        this.remainingTime--;
      }
      else {
        this.submitAttempt();
      }
    });
  }

  submitAll() {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: '300px',
      data: { message: 'Are you sure you want to proceed?' }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.submitAttempt();
      }
    });
  }

  submitAttempt() {
    this.attemptService.submitAttempt(this.attempt.id).subscribe({          
      next: (response) => {
        if (response) {
          this.closeAttempt();
        }
        else {
          console.error('Attempt was not submitted');
        }
      },
      error: (error) => {
        console.error('Error submitting attempt', error);
      }
    });
  }

  closeAttempt() {
    this.snackBar.open('', 'Close', { duration: 2000 });
    this.router.navigate([`/main-page`]);
  }

  translateText(texts: LanguageText[]): string {
    return this.translationService.translateText(texts);
  }

  translate(key: string){
    return this.translationService.translate(key);
  }

  getAttemptAnswers(question : KnowledgeCheckQuestion): AttemptAnswer[] {
    return this.attempt.submittedAnswers.filter(sa => question.answerGroups.map(ag => ag.id).includes(sa.answerGroupId));
  }

  onAnswer(event: AttemptAnswer) {
    const submittedAnswer = this.attempt.submittedAnswers.find(sa => sa.answerGroupId === event.answerGroupId);

    if (submittedAnswer) {
      submittedAnswer.values = event.values;
    }
    else {
      this.attempt.submittedAnswers.push(event);
    }
  }

  areAllAnswered(): boolean {
    return this.attempt.submittedAnswers.filter(sa => sa.values.length > 0).length === this.topic.questions.flatMap(q => q.answerGroups).length;
  }
}