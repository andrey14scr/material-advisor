import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TranslationService } from '@shared/services/translation.service';
import { Router } from '@angular/router';
import { UnverifiedAnswer } from '@models/knowledge-check/UnverifiedAnswer';
import { VerifyService } from '@services/verify.service';
import { GUID } from '@shared/types/GUID';
import { CommonModule } from '@angular/common';
import { MaterialModule } from '@shared/modules/matetial/material.module';
import { LanguageText } from '@shared/models/LanguageText';
import { FormsModule } from '@angular/forms';
import { QuestionType } from '@shared/types/QuestionEnum';
import { MatSnackBar } from '@angular/material/snack-bar';
import * as signalR from '@aspnet/signalr';
import { environment } from '@environments/environment';
import { AuthService } from '@shared/services/auth.service';
import { VerifiedAIAnswer } from '@models/signalR/VerifiedAIAnswer';
import { VerifiedAnswer } from '@models/knowledge-check/VerifiedAnswer';

@Component({
  selector: 'answers-verification',
  standalone: true,
  imports: [CommonModule, MaterialModule, FormsModule],
  templateUrl: './answers-verification.component.html',
  styleUrl: './answers-verification.component.scss'
})
export class AnswersVerificationComponent implements OnInit {
  questionTypeRef = QuestionType;
  hubConnection!: signalR.HubConnection;
  items: {unverifiedAnswer: UnverifiedAnswer, score?: number, comment?: string}[] = [];

  private route = inject(ActivatedRoute);
  
  constructor(
    private router: Router,
    private authService: AuthService,
    private translationService: TranslationService,
    private verifyService: VerifyService,
    private snackBar: MatSnackBar,
  ){ }

  ngOnInit() {
    this.initializeSignalRConnection();
    const knowledgeCheckId = this.route.snapshot.paramMap.get('id') as GUID;

    this.verifyService.getAnswersToVerify(knowledgeCheckId).subscribe(unverifiedAnswers => {
      this.items = unverifiedAnswers.map(a => ({
        unverifiedAnswer: a, 
        score: a.verifiedAnswers.length ? a.verifiedAnswers[0].score : undefined,
        comment: a.verifiedAnswers.length ? a.verifiedAnswers[0].comment : undefined,
      }));
    });
  }

  initializeSignalRConnection() {
    const accessToken = this.authService.getAccessToken() ?? '';

    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${environment.apiUrl}/answer-verification-hub`, {
        accessTokenFactory: () => accessToken
      })
      .build();

    this.hubConnection.on('AnswersVerified', (answers: VerifiedAIAnswer[]) => {
      console.log
      answers.forEach(answer => {
        const existingAnswer = this.items.find(i => i.unverifiedAnswer.submittedAnswer.answerGroupId === answer.answerGroupId && 
          i.unverifiedAnswer.submittedAnswer.attemptId === answer.attemptId);
        if (existingAnswer) {
          existingAnswer.score = answer.score;
          existingAnswer.comment = answer.comment;
        }
      });
      this.snackBar.open('', 'Close', { duration: 2000 });
    });

    this.hubConnection.start()
      .then(() => console.log('SignalR connection started'))
      .catch(err => console.error('Error establishing SignalR connection:', err));
  }

  translate(key: string){
    return this.translationService.translate(key);
  }

  translateText(texts: LanguageText[]): string {
    return this.translationService.translateText(texts);
  }

  onSubmit(item: {unverifiedAnswer: UnverifiedAnswer, score?: number}) {
    if (item.score == null || item.score > item.unverifiedAnswer.topic.questions[0].points) {
      return;
    }

    const body: VerifiedAnswer = {
      answerGroupId: item.unverifiedAnswer.submittedAnswer.answerGroupId,
      attemptId: item.unverifiedAnswer.submittedAnswer.attemptId,
      score: item.score
    };

    this.verifyService.postVerifyAnswer(body).subscribe(isSuccess => {
      if (isSuccess) {
        this.snackBar.open('', 'Close', { duration: 2000 });
        this.items = this.items.filter(i => i !== item);
      }
      else {
        this.snackBar.open('', 'Close', { duration: 2000 });
        console.error('Answer was not verified', body);
      }
    });
  }
}
