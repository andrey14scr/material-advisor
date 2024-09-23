import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { LanguageText } from '@shared/models/LanguageText';
import { LanguageEnum } from "@shared/types/LanguageEnum";
import { CookieStorageService } from './cookie-storage.service';

@Injectable({
  providedIn: 'root'
})
export class TranslationService {
  private currentLanguageCodeKey = 'currentLanguage';
  private defaultLanguageCode = 'en';
  private currentLanguageCode = new BehaviorSubject<string>(this.defaultLanguageCode);
  private translations = new BehaviorSubject<any>({});

  private languagesDictionary: { [id: string] : LanguageEnum; } = { 
    'en': LanguageEnum.English,
    'pl': LanguageEnum.Polish,
  };

  constructor(private http: HttpClient, private cookieStorageService: CookieStorageService) {
    const savedLanguage = this.cookieStorageService.getItem(this.currentLanguageCodeKey);
    if (savedLanguage) {
      console.log('Switching language...');
      this.currentLanguageCode.next(savedLanguage);
    }

    this.loadTranslations(this.currentLanguageCode.value);
  }

  changeLanguage(language: string) {
    this.currentLanguageCode.next(language);
    this.cookieStorageService.setItem(this.currentLanguageCodeKey, language);
    this.loadTranslations(language);
  }

  private loadTranslations(language: string) {
    this.http.get(`/assets/lang/${language}.json`).subscribe(
      (translations) => this.translations.next(translations)
    );
  }

  translate(key: string): string {
    const translations = this.translations.value;
    return key.split('.').reduce((acc, part) => acc && acc[part], translations) || key;;
  }

  getCurrentLanguageCode(): string {
    return this.currentLanguageCode.value;
  }

  translateText(languageTexts: LanguageText[]): string {
    const languageId = this.languagesDictionary[this.currentLanguageCode.getValue()];
    return languageTexts.find(lt => lt.languageId === languageId)?.text ?? languageTexts.sort((a,b) => a.languageId - b.languageId)[0].text;
  }
}