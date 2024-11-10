import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, forkJoin, map, mergeMap, Observable, of, switchMap, tap } from 'rxjs';
import { LanguageText } from '@shared/models/LanguageText';
import { CookieStorageService } from './cookie-storage.service';
import { Language } from '@shared/types/Language';
import { LanguageEnum } from '@shared/types/LanguageEnum';

@Injectable({
  providedIn: 'root'
})
export class TranslationService {
  private langPath = 'assets/lang';
  private currentLanguageCodeKey = 'currentLanguage';
  private defaultLanguageCode = 'en';
  private currentLanguageCode = new BehaviorSubject<string>(this.defaultLanguageCode);
  private translations = new BehaviorSubject<any>({});
  private languages: Language[] = [];

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

  getCurrentLanguage(): LanguageEnum {
    return this.languagesDictionary[this.getCurrentLanguageCode()];
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

  getLanguages(): Observable<Language[]> {
    if (this.languages.length) {
      return of(this.languages);
    }
    
    return this.http.get<string[]>(`${this.langPath}/languages.json`).pipe(
      switchMap((arrayOfStrings) => {
        const transformedObservables = arrayOfStrings.map((langCode) =>
          this.http.get<{ languageName: string }>(`${this.langPath}/${langCode}.json`).pipe(
            map((data) => ({
              languageId: this.languagesDictionary[langCode],
              name: data.languageName,
              code: langCode,
              flag: `assets/img/${langCode}.png`
            }))
          )
        );
        return forkJoin(transformedObservables);
      }),
      tap((result) => {
        this.languages = result;
      })
    );
  }
}