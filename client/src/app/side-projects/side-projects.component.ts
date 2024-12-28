import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';

@Component({
  selector: 'app-side-projects',
  imports: [MatCardModule, MatButtonModule,],
  templateUrl: './side-projects.component.html',
  styleUrl: './side-projects.component.scss'
})
export class SideProjectsComponent {

}
