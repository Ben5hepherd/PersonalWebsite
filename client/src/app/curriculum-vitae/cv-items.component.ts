import { Component } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { CVItemComponent } from './cv-item.component';
import { CVStaticData } from './cv-static-data';
import { CommonModule } from '@angular/common';

@Component({
    selector: 'app-cv-items',
    imports: [MatCardModule, CVItemComponent, CommonModule],
    template: `<app-cv-item 
                    *ngFor="let item of items" 
                    [header]="item.header" 
                    [mainContent]="item.mainContent" 
                    [items]="item.bullets">
                </app-cv-item>`,
    styleUrl: './curriculum-vitae.component.scss'
})
export class CVItemsComponent {

    items: CVItemModel[] = [];

    constructor() {
        this.items.push(
            new CVItemModel(
                CVStaticData.baillieGiffordHeader,
                CVStaticData.baillieGiffordMainContent,
                CVStaticData.baillieGiffordBullets),
            new CVItemModel(
                CVStaticData.omnicellHeader,
                CVStaticData.omnicellMainContent,
                CVStaticData.omnicellBullets),
            new CVItemModel(
                CVStaticData.lifetimeHeader,
                CVStaticData.lifetimeMainContent,
                CVStaticData.lifetimeBullets),
            new CVItemModel(
                CVStaticData.rsmHeader,
                CVStaticData.rsmMainContent,
                CVStaticData.rsmBullets),
            new CVItemModel(
                CVStaticData.jpHeader,
                CVStaticData.jpMainContent,
                CVStaticData.jpBullets)
        );
    }
}

class CVItemModel {
    header: string;
    mainContent: string;
    bullets: string[];

    constructor(header: string, mainContent: string, bullets: string[]){
        this.header = header;
        this.mainContent = mainContent;
        this.bullets = bullets;
    }
}
