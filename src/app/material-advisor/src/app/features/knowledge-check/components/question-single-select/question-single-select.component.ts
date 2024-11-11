import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AttemptAnswer } from '@models/knowledge-check/AttemptAnswer';
import { KnowledgeCheckQuestion } from '@models/knowledge-check/topic/KnowledgeCheckQuestion';
import { AttemptService } from '@services/attempt.service';
import { LanguageText } from '@shared/models/LanguageText';
import { MaterialModule } from '@shared/modules/matetial/material.module';
import { TranslationService } from '@shared/services/translation.service';
import { GUID } from '@shared/types/GUID';
import { debounceTime, Subject } from 'rxjs';

@Component({
  selector: 'question-single-select',
  standalone: true,
  imports: [CommonModule, MaterialModule, FormsModule],
  templateUrl: './question-single-select.component.html',
  styleUrl: './question-single-select.component.scss'
})
export class QuestionSingleSelectComponent implements OnInit {
  @Input() question!: KnowledgeCheckQuestion;
  @Input() attemptId!: GUID;
  @Input() attemptAnswers: AttemptAnswer[] = [];
  @Output() answered = new EventEmitter<AttemptAnswer>();

  private inputSubject = new Subject<{values: string[], answerGroupId: GUID}>();
  private modifying: GUID[] = [];
  private saved: GUID[] = [];

  constructor(
    private translationService: TranslationService, 
    private attemptService: AttemptService, 
  ) { }

  ngOnInit() {
    this.saved = this.attemptAnswers.filter(a => a.values.length > 0).map(a => a.answerGroupId);

    this.inputSubject.pipe(debounceTime(500)).subscribe(x => {
      this.attemptService.submitAnswer({
        values: x.values, 
        answerGroupId: x.answerGroupId,
        attemptId: this.attemptId,
      }).subscribe(submittedAnswer => {
        this.modifying = this.modifying.filter(x => x !== submittedAnswer.answerGroupId);
        if (submittedAnswer.values.length > 0) {
          this.saved.push(submittedAnswer.answerGroupId);
        }

        this.answered.emit({
          answerGroupId: submittedAnswer.answerGroupId, 
          values: submittedAnswer.values
        });
      });
    });
  }

  tlt(texts: LanguageText[]): string {
    return this.translationService.translateLanguageText(texts);
  }

  t(key: string){
    return this.translationService.translate(key);
  }

  isChecked(answerGroupId: GUID, answerId: GUID): boolean {
    return this.attemptAnswers.find(a => a.answerGroupId === answerGroupId && a.values.includes(answerId)) != null;
  }

  isSaved(answerGroupId: GUID): boolean {
    return this.saved.find(x => x === answerGroupId) != null;
  }

  isModified(answerGroupId: GUID): boolean {
    return this.modifying.find(x => x === answerGroupId) != null;
  }

  onChange(answerGroupId: GUID, answerId: GUID) {
    this.saved = this.saved.filter(x => x !== answerGroupId);
    this.modifying.push(answerGroupId);

    this.inputSubject.next({
      values: [answerId],
      answerGroupId: answerGroupId
    });
  }
}
