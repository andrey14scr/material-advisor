import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { ReactiveFormsModule, FormGroup, FormArray, FormBuilder, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { TextsInputComponent } from '@shared/components/texts-input/texts-input.component';
import { TranslationService } from '@shared/services/translation.service';
import { QuestionEnum } from '@shared/types/QuestionEnum';

@Component({
  selector: 'questions-input',
  standalone: true,
  imports: [CommonModule, MatInputModule, MatButtonModule, MatFormFieldModule, MatSelectModule, ReactiveFormsModule, TextsInputComponent],
  templateUrl: './questions-input.component.html',
  styleUrl: './questions-input.component.scss'
})
export class QuestionsInputComponent {
  questionTypes = Object.values(QuestionEnum).filter(value => typeof value === String.name.toLowerCase());
  @Input() form!: FormGroup;
  @Input() questionsFormArray!: FormArray;

  constructor(private fb: FormBuilder, private translationService: TranslationService) {}
  
  ngOnInit(): void {
    
  }

  get textsFormArray() {
    return this.form.get('texts') as FormArray;
  }

  getTextsFormArray(index: number) {
    return this.questionsFormArray.controls[index].get('texts') as FormArray;
  }

  addQuestion(): void {
    const nextNumber = this.questionsFormArray.controls.length + 1;

    const questionGroup = this.fb.group({
      number: [nextNumber],
      points: ['', Validators.required],
      type: ['', Validators.required],
      texts: this.fb.array([]),
    });
    this.questionsFormArray.push(questionGroup);
  }

  removeQuestion(index: number): void {
    this.questionsFormArray.removeAt(index);
    this.updateQuestionNumbers();
  }

  updateQuestionNumbers(): void {
    for (let i = 0; i < this.questionsFormArray.length; i++) {
      this.questionsFormArray.controls[i].value.number = i + 1;
    }
  }

  toForm(control: any): FormGroup {
    return control as FormGroup;
  }

  translateQuestionType(questionType: string | QuestionEnum){
    return this.translationService.translate(`questionTypes.${questionType.toString()}`);
  }
}
