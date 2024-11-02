import { CommonModule } from '@angular/common';
import { Component, Input, OnInit } from '@angular/core';
import { ReactiveFormsModule, FormGroup, FormArray, FormBuilder } from '@angular/forms';
import { TextsInputComponent } from '@shared/components/texts-input/texts-input.component';
import { TranslationService } from '@shared/services/translation.service';
import { AnswersInputComponent } from "../answers-input/answers-input.component";
import { LanguageText } from '@shared/models/LanguageText';
import { MaterialModule } from '@shared/modules/matetial/material.module';
import { AnswerGroup } from '@models/topic/AnswerGroup';
import { Answer } from '@models/topic/Answer';

@Component({
  selector: 'answer-groups-input',
  standalone: true,
  imports: [
    CommonModule,
    MaterialModule,
    ReactiveFormsModule,
    TextsInputComponent,
    AnswersInputComponent
  ],
  templateUrl: './answer-groups-input.component.html',
  styleUrl: './answer-groups-input.component.scss'
})
export class AnswerGroupsInputComponent implements OnInit {
  @Input() form!: FormGroup;
  @Input() answerGroupsFormArray!: FormArray;
  @Input() formData!: AnswerGroup[];
  
  constructor(private fb: FormBuilder, private translationService: TranslationService) {}
  
  ngOnInit() {
    if (this.formData && this.formData.length) {
      this.formData.forEach(answerGroup => this.addForm(answerGroup.number, answerGroup.isTechnical));
    }
  }

  isTechnicalForm(): boolean {
    return this.answerGroupsFormArray.controls.length === 1 && this.answerGroupsFormArray.controls[0].value.isTechnical;
  }

  resetAnswerGroups () {
    this.answerGroupsFormArray.clear();
  }

  getAnswers(index: number): Answer[] {
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

  addEmptyTechnicalForm() {
    const nextNumber = this.answerGroupsFormArray.controls.length + 1;
    this.addForm(nextNumber, true);
  }

  addForm(number: number, isTechnical: boolean = false) {
    const answerGroupGroup = this.fb.group({
      number: [number],
      isTechnical: [isTechnical],
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
