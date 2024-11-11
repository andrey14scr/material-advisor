import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
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
  selector: 'question-open',
  standalone: true,
  imports: [CommonModule, MaterialModule, FormsModule],
  templateUrl: './question-open.component.html',
  styleUrl: './question-open.component.scss'
})
export class QuestionOpenComponent implements OnInit {
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

    this.inputSubject.pipe(debounceTime(1000)).subscribe(x => {
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

  getValue(answerGroupId: GUID): string {
    return this.attemptAnswers.find(a => a.answerGroupId === answerGroupId && a.values.length > 0)?.values[0] ?? '';
  }

  isSaved(answerGroupId: GUID): boolean {
    return this.saved.find(x => x === answerGroupId) != null;
  }

  isModified(answerGroupId: GUID): boolean {
    return this.modifying.find(x => x === answerGroupId) != null;
  }

  onChange(answerGroupId: GUID, event: any) {
    const inputValue = event.target.value;

    this.saved = this.saved.filter(x => x !== answerGroupId);
    this.modifying.push(answerGroupId);

    this.inputSubject.next({
      values: [inputValue],
      answerGroupId: answerGroupId
    });
  }
}