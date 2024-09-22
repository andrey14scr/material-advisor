import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';

@Injectable({
  providedIn: 'root',
})
export class CookieStorageService {
  constructor(private cookieService: CookieService) {}

  setItem(key: string, value: string, days: number = 1): void {
    this.cookieService.set(key, value, { expires: days, path: '/' });
  }

  getItem(key: string): string | null {
    return this.cookieService.get(key) || null;
  }

  removeItem(key: string): void {
    this.cookieService.delete(key, '/');
  }
}