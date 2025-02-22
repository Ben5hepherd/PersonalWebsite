import { Component, OnDestroy, OnInit } from '@angular/core';
import { ItemService } from './items.service';
import { Subject, takeUntil } from 'rxjs';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-items-page',
  imports: [CommonModule],
  templateUrl: './items-page.component.html',
})
export class ItemsPageComponent implements OnInit, OnDestroy {
  items: string[] = [];

  private destroy$ = new Subject<void>();
  
  constructor(private itemService: ItemService) {}

  ngOnInit(): void {
    this.itemService.getItems()
    .pipe(takeUntil(this.destroy$))
    .subscribe((items: string[]) => {
      this.items = items;
    });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
