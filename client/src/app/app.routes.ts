import { Routes } from '@angular/router';
import { LandingPageComponent } from './landing-page/landing-page.component';
import { LoginPageComponent } from './login-page/login-page.component';
import { ItemsPageComponent } from './items-page/items-page.component';
import { ForbiddenPageComponent } from './forbidden-page/forbidden-page.component';

export const routes: Routes = [
  { path: '', component: LandingPageComponent },
  { path: 'login', component: LoginPageComponent },
  { path: 'items', component: ItemsPageComponent },
  { path: 'forbidden', component: ForbiddenPageComponent },
];
