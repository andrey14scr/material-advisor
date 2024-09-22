import { Routes } from '@angular/router';
import { MainPageComponent } from '@features/main-page/main-page.component';
import { AccountComponent } from '@features/account/account.component';
import { LoginComponent } from '@shared/components/login/login.component';
import { AuthGuard } from '@shared/attributes/auth-guard';
import { TopicComponent } from '@features/topic/topic.component';
import { KnowledgeCheckComponent } from '@features/knowledge-check/knowledge-check.component';

export const routes: Routes = [
  { path: 'main-page', component: MainPageComponent, canActivate: [AuthGuard] },
  { path: 'account', component: AccountComponent, canActivate: [AuthGuard] },
  { path: 'topic/:id', component: TopicComponent, canActivate: [AuthGuard]},
  { path: 'knowledge-check/:id', component: KnowledgeCheckComponent, canActivate: [AuthGuard]},
  { path: 'login', component: LoginComponent },
  { path: '', redirectTo: '/main-page', pathMatch: 'full' },
  { path: '**', redirectTo: '/login' }
];