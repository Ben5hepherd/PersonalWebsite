import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { MatCardModule } from '@angular/material/card';

@Component({
    selector: 'app-cv-item',
    imports: [MatCardModule, CommonModule],
    templateUrl: './cv-item.component.html',
})
export class CVItemComponent {
    @Input() header!: string;
    @Input() mainContent!: string;
    @Input() items!: string[];
}