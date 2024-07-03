import { Routes } from '@angular/router';
import { MainPageComponent } from './main-page/main-page.component';
import { TestConstructorComponent } from './test-constructor/test-constructor.component';
import { InformationComponent } from './information/information.component';
import { AccountComponent } from './account/account.component';

export const routes: Routes = [
  { path: 'main-page', component: MainPageComponent },
  { path: 'test-constructor', component: TestConstructorComponent },
  { path: 'information', component: InformationComponent },
  { path: 'account', component: AccountComponent },
  { path: '', redirectTo: '/main-page', pathMatch: 'full' },
  { path: '**', redirectTo: '/main-page' }
];