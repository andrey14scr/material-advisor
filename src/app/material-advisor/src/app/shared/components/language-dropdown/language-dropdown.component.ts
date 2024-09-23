import { NgFor } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { TranslationService } from '../../services/translation.service';

interface Language {
  name: string;
  code: string;
  flag: string;
}

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

  constructor (private translationService: TranslationService, private http: HttpClient){}

  ngOnInit(): void {
    this.loadLanguages();
    this.setCurrentLanguage();
  }

  loadLanguages(): void {
    const langPath = 'assets/lang/';
    this.http.get<string[]>(`${langPath}languages.json`).subscribe(langTags => {
      langTags.forEach(langTag => {
        this.http.get<{ languageName: string }>(`${langPath}${langTag}.json`).subscribe(data => {
          this.languages.push({
            name: data.languageName,
            code: langTag,
            flag: `assets/img/${langTag}.png`
          });
        });
      });
    });
  }

  setCurrentLanguage(): void {
    const currentCode = this.translationService.getCurrentLanguageCode();
    const currentLanguage = this.languages.find(language => language.code === currentCode);
    if (currentLanguage) {
      this.selectedLanguage = currentLanguage;
    }
  }

  languageChanged(selectedLanguage: Language) {
    this.selectedLanguage = selectedLanguage;
    this.translationService.changeLanguage(selectedLanguage.code);
  }
}
