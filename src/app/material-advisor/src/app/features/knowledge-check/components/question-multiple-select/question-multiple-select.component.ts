import { CommonModule } from '@angular/common';
import { Component, EventEmitter, input, Input, OnChanges, OnInit, Output } from '@angular/core';
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
  selector: 'question-multiple-select',
  standalone: true,
  imports: [CommonModule, MaterialModule, FormsModule],
  templateUrl: './question-multiple-select.component.html',
  styleUrl: './question-multiple-select.component.scss'
})
export class QuestionMultipleSelectComponent implements OnInit {
  @Input() question!: KnowledgeCheckQuestion;
  @Input() attemptId!: GUID;
  @Input() attemptAnswers: AttemptAnswer[] = [];
  @Output() answered = new EventEmitter<AttemptAnswer>();
  
  isModified = false;
  isSaved = false;
  items: any[] = [];
  DELIMETER: string = ';';

  private inputSubject = new Subject<{value: string, answerGroupId: GUID}>();

  constructor(
    private translationService: TranslationService, 
    private attemptService: AttemptService, 
  ) { }

  ngOnInit() {
    const ids = this.attemptAnswer?.value?.split(this.DELIMETER) ?? [];
    this.items = this.question.answerGroups[0].answers.map(item => ({ ...item, checked: ids.filter(id => id === item.id).length !== 0 }));

    if (this.attemptAnswer) {
      this.isSaved = true;
    }

    this.inputSubject.pipe(debounceTime(1000)).subscribe(x => {
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
    const selectedItems = this.items.filter(item => item.checked).map(x => x.id).join(this.DELIMETER);
    this.inputSubject.next({
      value: selectedItems,
      answerGroupId: answerGroupId
    });
  }
}
