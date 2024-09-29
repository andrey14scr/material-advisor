import { CommonModule } from '@angular/common';
import { Component, Input, OnInit } from '@angular/core';
import { ReactiveFormsModule, FormGroup, FormArray, FormBuilder, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { TextsInputComponent } from '@shared/components/texts-input/texts-input.component';
import { TranslationService } from '@shared/services/translation.service';

@Component({
  selector: 'answers-input',
  standalone: true,
  imports: [
    CommonModule, 
    MatInputModule, 
    MatButtonModule, 
    MatFormFieldModule, 
    MatSelectModule, 
    ReactiveFormsModule, 
    TextsInputComponent, 
    MatCardModule
  ],
  templateUrl: './answers-input.component.html',
  styleUrl: './answers-input.component.scss'
})
export class AnswersInputComponent implements OnInit {
  @Input() form!: FormGroup;
  @Input() answersFormArray!: FormArray;

  constructor(private fb: FormBuilder, private translationService: TranslationService) {}
  
  ngOnInit(): void {
    
  }

  get textsFormArray() {
    return this.form.get('texts') as FormArray;
  }

  getTextsFormArray(index: number) {
    return this.answersFormArray.controls[index].get('texts') as FormArray;
  }

  addAnswer(): void {
    const nextNumber = this.answersFormArray.controls.length + 1;

    const answerGroup = this.fb.group({
      number: [nextNumber],
      points: ['', Validators.required],
      texts: this.fb.array([]),
    });
    this.answersFormArray.push(answerGroup);
  }

  removeAnswer(index: number): void {
    this.answersFormArray.removeAt(index);
    this.updateNumbers();
  }

  updateNumbers(): void {
    for (let i = 0; i < this.answersFormArray.length; i++) {
      this.answersFormArray.controls[i].value.number = i + 1;
    }
  }

  toForm(control: any): FormGroup {
    return control as FormGroup;
  }
}