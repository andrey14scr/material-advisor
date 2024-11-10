import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { TranslationService } from '../../services/translation.service';
import { Language } from '@shared/types/Language';
import { UserService } from '@shared/services/user.service';
import { LanguageEnum } from '@shared/types/LanguageEnum';

@Component({
  selector: 'app-language-dropdown',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './language-dropdown.component.html',
  styleUrl: './language-dropdown.component.scss'
})
export class LanguageDropdownComponent implements OnInit {
  languages: Language[] = [];
  selectedLanguage!: Language;
  defaultLanguageId = LanguageEnum.English;

  constructor (private translationService: TranslationService, private userService: UserService){}

  ngOnInit() {
    this.translationService.getLanguages().subscribe(languages => {
      this.languages = languages;
      this.userService.getUserCurrentLanguage().subscribe(settings => {
        if (settings.currentLanguage) {
          this.selectedLanguage = this.languages.find(x => x.code === settings.currentLanguage)!;
        }
        else {
          const selectedLanguage = this.languages.find(x => x.languageId === this.defaultLanguageId)!;
          this.languageChanged(selectedLanguage);
        }
      });
    });
  }

  languageChanged(selectedLanguage: Language) {
    this.selectedLanguage = selectedLanguage;
    this.translationService.changeLanguage(selectedLanguage.code);
    this.userService.updateUser(selectedLanguage.code);
  }
}
