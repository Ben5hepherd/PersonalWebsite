import { Component } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatTabsModule } from '@angular/material/tabs';
import { SideProjectsComponent } from '../side-projects/side-projects.component';
import { CurriculumVitaeComponent } from '../curriculum-vitae/curriculum-vitae.component';

@Component({
  selector: 'app-landing-page',
  imports: [MatCardModule, MatButtonModule, SideProjectsComponent, MatTabsModule, CurriculumVitaeComponent],
  templateUrl: './landing-page.component.html',
  styleUrl: './landing-page.component.scss'
})
export class LandingPageComponent {

}
