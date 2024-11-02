import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { TranslationService } from './shared/services/translation.service';
import { LanguageDropdownComponent } from './shared/components/language-dropdown/language-dropdown.component';
import { AuthService } from '@shared/services/auth.service';
import { MaterialModule } from '@shared/modules/matetial/material.module';

@Component({
    selector: 'app-root',
    standalone: true,
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.scss'],
    imports: [RouterOutlet, CommonModule, RouterModule, LanguageDropdownComponent, MaterialModule]
})
export class AppComponent implements OnInit {
  constructor(public translationService: TranslationService, private authService: AuthService) {}
  title = 'material-advisor';

  translate(key: string){
    return this.translationService.translate(key);
  }

  ngOnInit() {
    this.authService.decodeToken();
  }

  onLogout() {
    this.authService.logout();
  }
}