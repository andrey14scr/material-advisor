import { Component, Input, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { ReactiveFormsModule } from '@angular/forms';
import { Language } from '@shared/types/Language';
import { TranslationService } from '@shared/services/translation.service';

@Component({
  selector: 'texts-input',
  standalone: true,
  imports: [CommonModule, MatInputModule, MatButtonModule, MatFormFieldModule, MatSelectModule, ReactiveFormsModule],
  templateUrl: './texts-input.component.html',
  styleUrl: './texts-input.component.scss'
})
export class TextsInputComponent implements OnInit {
  languages: Language[] = [];
  @Input() formName!: string;
  @Input() isOneRow: boolean = true;
  @Input() form!: FormGroup;
  @Input() textsFormArray!: FormArray;

  constructor(private fb: FormBuilder, private translationService: TranslationService) {}
  
  ngOnInit(): void {
    this.translationService.getLanguages().subscribe(lang => {
      this.languages.push(lang);
    });

    this.addText();
  }

  addText(): void {
    let defaultLanguage = null;
    if (this.textsFormArray.controls.length === 0) {
      defaultLanguage = this.translationService.getLanguageId(this.translationService.getCurrentLanguageCode());
    }

    const textGroup = this.fb.group({
      languageId: [defaultLanguage ?? '', Validators.required],
      text: ['', Validators.required],
    });
    this.textsFormArray.push(textGroup);
  }

  removeText(index: number): void {
    if (this.textsFormArray.controls.length === 1) {
      return;
    }

    this.textsFormArray.removeAt(index);
  }

  toForm(control: any): FormGroup {
    return control as FormGroup;
  }

  getLanguageName(langCode: string){
    return this.translationService.translate(`languages.${langCode}`);
  }
}
