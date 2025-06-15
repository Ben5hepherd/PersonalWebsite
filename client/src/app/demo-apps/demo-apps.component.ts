import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';

@Component({
  selector: 'app-demo-apps',
  imports: [MatCardModule, MatButtonModule],
  templateUrl: './demo-apps.component.html',
})
export class DemoAppsComponent {}
