import { Routes } from '@angular/router';
import { LandingPageComponent } from './landing-page/landing-page.component';
import { LoginPageComponent } from './login-page/login-page.component';
import { ForbiddenPageComponent } from './forbidden-page/forbidden-page.component';
import { PhotoAlbumComponent } from './photo-album/photo-album.component';

export const routes: Routes = [
  { path: '', component: LandingPageComponent },
  { path: 'login', component: LoginPageComponent },
  { path: 'photos', component: PhotoAlbumComponent },
  { path: 'forbidden', component: ForbiddenPageComponent },
];
