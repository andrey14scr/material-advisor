import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { FormGroup, FormBuilder, FormArray, Validators, ReactiveFormsModule } from '@angular/forms';
import { MaterialModule } from '@shared/modules/matetial/material.module';
import { TranslationService } from '@shared/services/translation.service';
import { QuestionType } from '@shared/types/QuestionEnum';

@Component({
  selector: 'questions-template-input',
  standalone: true,
  imports: [CommonModule, MaterialModule, ReactiveFormsModule],
  templateUrl: './questions-template-input.component.html',
  styleUrl: './questions-template-input.component.scss'
})
export class QuestionsTemplateInputComponent {
  @Input() formName!: string;
  @Input() isOneRow: boolean = true;
  @Input() form!: FormGroup;

  questionTypes = Object.keys(QuestionType)
    .filter(key => !isNaN(Number(key)))
    .map(key => ({
      key: Number(key),
      value: QuestionType[key as any] as string
    }));

  constructor(private fb: FormBuilder, private translationService: TranslationService) { }
  
  ngOnInit() {
    this.addEmptyForm();
  }

  get questionsStructureFormArray(): FormArray {
    return this.form.get(this.formName) as FormArray;
  }

  addEmptyForm() {
    this.addForm(1, QuestionType.SingleSelect, undefined);
  }

  addForm(count: number, type: QuestionType, answersCount?: number) {
    const questionsTemplate = this.fb.group({
      count: [count, Validators.required],
      type: [type, Validators.required],
      answersCount: [answersCount],
    });
    this.questionsStructureFormArray.push(questionsTemplate);
  }

  removeForm(index: number) {
    if (this.questionsStructureFormArray.controls.length === 1) {
      return;
    }

    this.questionsStructureFormArray.removeAt(index);
  }

  translateQuestionType(questionType: string | QuestionType){
    return this.translationService.translate(`questionTypes.${questionType.toString()}`);
  }

  t(key: string): string {
    return this.translationService.translate(key);
  }
}