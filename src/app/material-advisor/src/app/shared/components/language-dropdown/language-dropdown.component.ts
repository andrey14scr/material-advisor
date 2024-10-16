import { NgFor } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { TranslationService } from '../../services/translation.service';
import { Language } from '@shared/types/Language';
import { UserService } from '@shared/services/user.service';
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
  defaultLanguageId = LanguageEnum.English;

  constructor (private translationService: TranslationService, private userService: UserService){}

  ngOnInit(): void {
    let currentCode = this.translationService.getCurrentLanguageCode();

    if (!currentCode) {
      this.userService.getUserCurrentLanguage().subscribe(lang => {
        currentCode = lang;
      });
    }

    this.translationService.getLanguages().subscribe(lang => {
      this.languages.push(lang);

      if (!currentCode && lang.languageId === this.defaultLanguageId || currentCode && lang.code === currentCode) {
        currentCode = lang.code;
        this.selectedLanguage = lang;
      }
    });
  }

  languageChanged(selectedLanguage: Language) {
    this.selectedLanguage = selectedLanguage;
    this.translationService.changeLanguage(selectedLanguage.code);
    this.userService.updateUser(selectedLanguage.code);
  }
}
