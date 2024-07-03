import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TranslationService {
  private currentLanguage = new BehaviorSubject<string>('en');
  private translations = new BehaviorSubject<any>({});

  constructor(private http: HttpClient) {
    this.loadTranslations('en');
  }

  changeLanguage(language: string) {
    this.currentLanguage.next(language);
    this.loadTranslations(language);
  }

  private loadTranslations(language: string) {
    this.http.get(`/assets/${language}.json`).subscribe(
      (translations) => this.translations.next(translations)
    );
  }

  getTranslation(key: string): string {
    const translations = this.translations.value;
    return key.split('.').reduce((acc, part) => acc && acc[part], translations) || key;;
  }
}