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

  constructor(private fb: FormBuilder, private translationService: TranslationService) {}
  
  ngOnInit(): void {
    
  }

  get textsFormArray() {
    return this.form.get('content') as FormArray;
  }

  getAnswersFormArray(index: number) {
    return this.answerGroupsFormArray.controls[index].get('answers') as FormArray;
  }

  getTextsFormArray(index: number) {
    return this.answerGroupsFormArray.controls[index].get('content') as FormArray;
  }

  addAnswerGroup(): void {
    const nextNumber = this.answerGroupsFormArray.controls.length + 1;

    const answerGroupGroup = this.fb.group({
      number: [nextNumber],
      content: this.fb.array([]),
      answers: this.fb.array([]),
    });
    this.answerGroupsFormArray.push(answerGroupGroup);
  }

  removeAnswerGroup(index: number): void {
    this.answerGroupsFormArray.removeAt(index);
    this.updateNumbers();
  }

  updateNumbers(): void {
    for (let i = 0; i < this.answerGroupsFormArray.length; i++) {
      this.answerGroupsFormArray.controls[i].value.number = i + 1;
    }
  }

  toForm(control: any): FormGroup {
    return control as FormGroup;
  }
}
