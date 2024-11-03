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

@Component({
  selector: 'answers-verification',
  standalone: true,
  imports: [CommonModule, MaterialModule, FormsModule],
  templateUrl: './answers-verification.component.html',
  styleUrl: './answers-verification.component.scss'
})
export class AnswersVerificationComponent implements OnInit {
  questionTypeRef = QuestionType;
  private route = inject(ActivatedRoute);

  items: {unverifiedAnswer: UnverifiedAnswer, score?: number}[] = [];
  
  constructor(
    private router: Router,
    private translationService: TranslationService,
    private verifyService: VerifyService,
    private snackBar: MatSnackBar,
  ){ }

  ngOnInit() {
    const knowledgeCheckId = this.route.snapshot.paramMap.get('id') as GUID;

    this.verifyService.getAnswersToVerify(knowledgeCheckId).subscribe(unverifiedAnswers => {
      this.items = unverifiedAnswers.map(a => ({
        unverifiedAnswer: a, 
        score: undefined,
      }));
    });
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

    const body = {
      answerGroupId: item.unverifiedAnswer.answerGroupId,
      attemptId: item.unverifiedAnswer.attemptId,
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
