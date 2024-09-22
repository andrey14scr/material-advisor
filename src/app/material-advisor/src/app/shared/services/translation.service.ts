import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { LanguageText } from '@shared/models/LanguageText';
import { LanguageEnum } from "@shared/types/LanguageEnum";

@Injectable({
  providedIn: 'root'
})
export class TranslationService {
  private currentLanguage = new BehaviorSubject<string>('en');
  private translations = new BehaviorSubject<any>({});
  currentLanguage$ = this.currentLanguage.asObservable();

  private languagesDictionary: { [id: string] : LanguageEnum; } = { 
    'en': LanguageEnum.English,
    'pl': LanguageEnum.Polish,
  };

  constructor(private http: HttpClient) {
    this.loadTranslations(this.currentLanguage.value);
  }

  changeLanguage(language: string) {
    this.currentLanguage.next(language);
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

  getCurrentLanguage(): Observable<string> {
    return this.currentLanguage.asObservable();
  }

  translateText(languageTexts: LanguageText[]): string {
    const languageId = this.languagesDictionary[this.currentLanguage.getValue()];
    return languageTexts.find(lt => lt.languageId === languageId)?.text ?? languageTexts.sort((a,b) => a.languageId - b.languageId)[0].text;
  }
}