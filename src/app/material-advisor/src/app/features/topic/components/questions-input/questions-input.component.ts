import { CommonModule } from '@angular/common';
import { Component, Input, OnInit } from '@angular/core';
import { ReactiveFormsModule, FormGroup, FormArray, FormBuilder, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { TextsInputComponent } from '@shared/components/texts-input/texts-input.component';
import { TranslationService } from '@shared/services/translation.service';
import { AnswerGroupsInputComponent } from "../answer-groups-input/answer-groups-input.component";
import { MatCardModule } from '@angular/material/card';
import { QuestionModel } from '@features/topic/models/Question';
import { AnswerGroupModel } from '@features/topic/models/AnswerGroup';
import { LanguageText } from '@shared/models/LanguageText';
import { QuestionType } from '@shared/types/QuestionEnum';

@Component({
  selector: 'questions-input',
  standalone: true,
  imports: [
    CommonModule, 
    MatInputModule, 
    MatButtonModule, 
    MatFormFieldModule, 
    MatSelectModule, 
    ReactiveFormsModule, 
    TextsInputComponent, 
    AnswerGroupsInputComponent, 
    MatCardModule
  ],
  templateUrl: './questions-input.component.html',
  styleUrl: './questions-input.component.scss'
})
export class QuestionsInputComponent implements OnInit {
  questionTypes = Object.keys(QuestionType)
    .filter(key => !isNaN(Number(key)))
    .map(key => ({
      key: Number(key),
      value: QuestionType[key as any] as string
    }));
  @Input() form!: FormGroup;
  @Input() questionsFormArray!: FormArray;
  @Input() formData!: QuestionModel[];

  constructor(private fb: FormBuilder, private translationService: TranslationService) {}
  
  ngOnInit() {
    if (this.formData && this.formData.length) {
      this.formData.forEach(question => this.addForm(question.number, question.points, question.type));
    }
  }

  getAnswerGroups(index: number): AnswerGroupModel[] {
    return this.formData[index]?.answerGroups ?? [];
  }

  getAnswerGroupsFormArray(index: number): FormArray {
    return this.questionsFormArray.controls[index].get('answerGroups') as FormArray;
  }

  getContent(index: number): LanguageText[] {
    return this.formData[index]?.content ?? [];
  }

  addEmptyForm() {
    const nextNumber = this.questionsFormArray.controls.length + 1;
    this.addForm(nextNumber, 0, QuestionType.SingleSelect,
    );
  }

  addForm(number: number, points: number, type: QuestionType) {
    const questionGroup = this.fb.group({
      number: [number],
      points: [points, Validators.required],
      type: [type, Validators.required],
      content: this.fb.array([]),
      answerGroups: this.fb.array([]),
    });
    this.questionsFormArray.push(questionGroup);
  }

  removeQuestion(index: number): void {
    this.questionsFormArray.removeAt(index);
    this.updateNumbers();
  }

  updateNumbers(): void {
    for (let i = 0; i < this.questionsFormArray.length; i++) {
      this.questionsFormArray.controls[i].value.number = i + 1;
    }
  }

  toForm(control: any): FormGroup {
    return control as FormGroup;
  }

  translateQuestionType(questionType: string | QuestionType){
    return this.translationService.translate(`questionTypes.${questionType.toString()}`);
  }
}
