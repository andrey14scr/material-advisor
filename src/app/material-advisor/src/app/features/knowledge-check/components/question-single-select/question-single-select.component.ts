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
  
  items: any[] = [];
  isModified = false;
  isSaved = false;
  selectedOption = '';

  private inputSubject = new Subject<{value: string, answerGroupId: GUID}>();

  constructor(
    private translationService: TranslationService, 
    private attemptService: AttemptService, 
  ) { }

  ngOnInit() {
    this.items = this.question.answerGroups[0].answers.map(item => ({ ...item, checked: this.attemptAnswer?.value === item.id }));
    this.selectedOption = this.items.find(i => i.checked)?.id;

    if (this.attemptAnswer) {
      this.isSaved = true;
    }

    this.inputSubject.pipe(debounceTime(500)).subscribe(x => {
      this.attemptService.submitAnswer({
        value: x.value, 
        questionId: x.answerGroupId,
        attemptId: this.attemptId,
      }).subscribe(submittedAnswer => {
        if (submittedAnswer && submittedAnswer.value) {
          this.isSaved = true;
        }
        else {
          this.isSaved = false;
        }

        this.answered.emit({
          answerGroupId: x.answerGroupId, 
          value: submittedAnswer.value
        });
        this.isModified = false;
      });
    });
  }

  translateText(texts: LanguageText[]): string {
    return this.translationService.translateText(texts);
  }

  translate(key: string){
    return this.translationService.translate(key);
  }

  onChange(answerGroupId: GUID, event: any) {
    this.isSaved = false;
    this.isModified = true;
    this.inputSubject.next({
      value: event.value,
      answerGroupId: answerGroupId
    });
  }
}
