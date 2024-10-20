import { CommonModule } from '@angular/common';
import { Component, Input, OnInit } from '@angular/core';
import { ReactiveFormsModule, FormGroup, FormArray, FormBuilder, Validators } from '@angular/forms';
import { AnswerModel } from '@features/topic/models/Answer';
import { TextsInputComponent } from '@shared/components/texts-input/texts-input.component';
import { LanguageText } from '@shared/models/LanguageText';
import { MaterialModule } from '@shared/modules/matetial/material.module';
import { TranslationService } from '@shared/services/translation.service';

@Component({
  selector: 'answers-input',
  standalone: true,
  imports: [
    CommonModule, 
    MaterialModule, 
    ReactiveFormsModule, 
    TextsInputComponent,
  ],
  templateUrl: './answers-input.component.html',
  styleUrl: './answers-input.component.scss'
})
export class AnswersInputComponent implements OnInit {
  @Input() form!: FormGroup;
  @Input() answersFormArray!: FormArray;
  @Input() formData!: AnswerModel[];

  constructor(private fb: FormBuilder, private translationService: TranslationService) {}
  
  ngOnInit() {
    if (this.formData && this.formData.length) {
      this.formData.forEach(answer => this.addForm(answer.number, answer.points));
    }
  }

  getTextsFormArray(index: number) {
    return this.answersFormArray.controls[index].get('content') as FormArray;
  }

  getContent(index: number): LanguageText[] {
    return this.formData[index]?.content ?? [];
  }

  addEmptyForm() {
    const nextNumber = this.answersFormArray.controls.length + 1;
    this.addForm(nextNumber, 0);
  }

  addForm(number: number, points: number) {
    const answerGroup = this.fb.group({
      number: [number],
      points: [points, Validators.required],
      content: this.fb.array([]),
    });
    this.answersFormArray.push(answerGroup);
  }

  removeAnswer(index: number) {
    this.answersFormArray.removeAt(index);
    this.updateNumbers();
  }

  updateNumbers() {
    for (let i = 0; i < this.answersFormArray.length; i++) {
      this.answersFormArray.controls[i].value.number = i + 1;
    }
  }

  toForm(control: any): FormGroup {
    return control as FormGroup;
  }
}