import { Component, Input, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { Language } from '@shared/types/Language';
import { TranslationService } from '@shared/services/translation.service';
import { LanguageText } from '@shared/models/LanguageText';
import { LanguageEnum } from '@shared/types/LanguageEnum';
import { MaterialModule } from '@shared/modules/matetial/material.module';

@Component({
  selector: 'texts-input',
  standalone: true,
  imports: [CommonModule, MaterialModule, ReactiveFormsModule],
  templateUrl: './texts-input.component.html',
  styleUrl: './texts-input.component.scss'
})
export class TextsInputComponent implements OnInit {
  languages: Language[] = [];
  @Input() formName!: string;
  @Input() isOneRow: boolean = true;
  @Input() form!: FormGroup;
  @Input() formData!: LanguageText[];

  constructor(private fb: FormBuilder, private translationService: TranslationService) { }
  
  ngOnInit() {
    this.translationService.getLanguages().subscribe(lang => {
      this.languages.push(lang);
    });

    if (this.formData && this.formData.length) {
      this.formData.forEach(x => this.addForm(x.languageId, x.text));
    }
    else {
      this.addEmptyForm();
    }
  }

  get textsFormArray(): FormArray {
    return this.form.get(this.formName) as FormArray;
  }

  addEmptyForm() {
    const defaultLanguage = this.translationService.getLanguageId(this.translationService.getCurrentLanguageCode());
    this.addForm(defaultLanguage, '');
  }

  addForm(languageId: LanguageEnum, text: string) {
    const textGroup = this.fb.group({
      languageId: [languageId, Validators.required],
      text: [text, Validators.required],
    });
    this.textsFormArray.push(textGroup);
  }

  removeText(index: number) {
    if (this.textsFormArray.controls.length === 1) {
      return;
    }

    this.textsFormArray.removeAt(index);
  }

  isLanguageChosen(languageId: LanguageEnum): boolean {
    return this.textsFormArray.value.filter((x: LanguageText) => x.languageId === languageId).length !== 0;
  }

  getLanguageName(langCode: string){
    return this.translationService.translate(`languages.${langCode}`);
  }
}
