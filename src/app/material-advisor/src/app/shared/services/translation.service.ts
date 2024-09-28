import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, forkJoin, map, mergeMap, Observable, of, switchMap } from 'rxjs';
import { LanguageText } from '@shared/models/LanguageText';
import { LanguageEnum } from "@shared/types/LanguageEnum";
import { CookieStorageService } from './cookie-storage.service';
import { Language } from '@shared/types/Language';

@Injectable({
  providedIn: 'root'
})
export class TranslationService {
  private langPath = 'assets/lang';
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
    this.http.get(`/${this.langPath}/${language}.json`).subscribe(
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

  getLanguageId(key: string): LanguageEnum {
    return this.languagesDictionary[key];
  }

  translateText(languageTexts: LanguageText[]): string {
    const languageId = this.languagesDictionary[this.currentLanguageCode.getValue()];
    return languageTexts.find(lt => lt.languageId === languageId)?.text ?? languageTexts.sort((a,b) => a.languageId - b.languageId)[0].text;
  }

  getLanguages(): Observable<Language> {
    return this.http.get<string[]>(`${this.langPath}/languages.json`).pipe(
      mergeMap(langTags =>
        of(...langTags).pipe(
          mergeMap(langCode =>
            this.http.get<{ languageName: string }>(`${this.langPath}/${langCode}.json`).pipe(
              map(data => ({
                languageId: this.languagesDictionary[langCode],
                name: data.languageName,
                code: langCode,
                flag: `assets/img/${langCode}.png`
              }))
            )
          )
        )
      )
    );
  }
}