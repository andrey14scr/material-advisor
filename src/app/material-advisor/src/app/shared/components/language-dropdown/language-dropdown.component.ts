import { NgFor } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { TranslationService } from '../../services/translation.service';
import { Language } from '@shared/types/Language';
import { LanguageEnum } from '@shared/types/LanguageEnum';

@Component({
  selector: 'app-language-dropdown',
  standalone: true,
  imports: [NgFor],
  templateUrl: './language-dropdown.component.html',
  styleUrl: './language-dropdown.component.scss'
})
export class LanguageDropdownComponent implements OnInit {
  languages: Language[] = [];
  selectedLanguage: Language | null = null;

  constructor (private translationService: TranslationService){}

  ngOnInit(): void {
    const currentCode = this.translationService.getCurrentLanguageCode();

    this.translationService.getLanguages().subscribe(lang => {
      this.languages.push(lang);

      if (!this.selectedLanguage && lang.languageId === LanguageEnum.English) {
        this.selectedLanguage = lang;
      }
      else if (lang.code == currentCode) {
        this.selectedLanguage = lang;
      }
    });
  }

  languageChanged(selectedLanguage: Language) {
    this.selectedLanguage = selectedLanguage;
    this.translationService.changeLanguage(selectedLanguage.code);
  }
}
