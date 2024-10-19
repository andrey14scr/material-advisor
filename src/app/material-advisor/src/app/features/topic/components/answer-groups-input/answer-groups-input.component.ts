import { CommonModule } from '@angular/common';
import { Component, Input, OnInit } from '@angular/core';
import { ReactiveFormsModule, FormGroup, FormArray, FormBuilder } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { TextsInputComponent } from '@shared/components/texts-input/texts-input.component';
import { TranslationService } from '@shared/services/translation.service';
import { AnswersInputComponent } from "../answers-input/answers-input.component";
import { MatCardModule } from '@angular/material/card';
import { AnswerGroupModel } from '@features/topic/models/AnswerGroup';
import { LanguageText } from '@shared/models/LanguageText';
import { AnswerModel } from '@features/topic/models/Answer';

@Component({
  selector: 'answer-groups-input',
  standalone: true,
  imports: [
    CommonModule,
    MatInputModule,
    MatButtonModule,
    MatFormFieldModule,
    MatSelectModule,
    ReactiveFormsModule,
    TextsInputComponent,
    MatCardModule,
    AnswersInputComponent
  ],
  templateUrl: './answer-groups-input.component.html',
  styleUrl: './answer-groups-input.component.scss'
})
export class AnswerGroupsInputComponent implements OnInit {
  @Input() form!: FormGroup;
  @Input() answerGroupsFormArray!: FormArray;
  @Input() formData!: AnswerGroupModel[];

  constructor(private fb: FormBuilder, private translationService: TranslationService) {}
  
  ngOnInit() {
    if (this.formData && this.formData.length) {
      this.formData.forEach(answerGroup => this.addForm(answerGroup.number));
    }
  }

  getAnswers(index: number): AnswerModel[] {
    return this.formData[index]?.answers ?? [];
  }

  getContent(index: number): LanguageText[] {
    return this.formData[index]?.content ?? [];
  }

  getAnswersFormArray(index: number) {
    return this.answerGroupsFormArray.controls[index].get('answers') as FormArray;
  }

  addEmptyForm() {
    const nextNumber = this.answerGroupsFormArray.controls.length + 1;
    this.addForm(nextNumber);
  }

  addForm(number: number) {
    const answerGroupGroup = this.fb.group({
      number: [number],
      content: this.fb.array([]),
      answers: this.fb.array([]),
    });
    this.answerGroupsFormArray.push(answerGroupGroup);
  }

  removeAnswerGroup(index: number) {
    this.answerGroupsFormArray.removeAt(index);
    this.updateNumbers();
  }

  updateNumbers() {
    for (let i = 0; i < this.answerGroupsFormArray.length; i++) {
      this.answerGroupsFormArray.controls[i].value.number = i + 1;
    }
  }

  toForm(control: any): FormGroup {
    return control as FormGroup;
  }
}
