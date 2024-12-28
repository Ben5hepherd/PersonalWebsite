import { Component } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { CVItemsComponent } from './cv-items.component';

@Component({
  selector: 'app-curriculum-vitae',
  imports: [MatCardModule, CVItemsComponent],
  templateUrl: './curriculum-vitae.component.html',
  styleUrl: './curriculum-vitae.component.scss'
})
export class CurriculumVitaeComponent {

}
