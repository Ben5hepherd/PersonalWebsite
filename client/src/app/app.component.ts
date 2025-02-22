import { Component, OnDestroy, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from './header/header.component';
import { FooterComponent } from './footer/footer.component';
import { ConfigService } from './config.service';
import { MatProgressSpinner } from '@angular/material/progress-spinner';
import { LoadingService } from './loading.service';
import { Subject, takeUntil } from 'rxjs';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, HeaderComponent, FooterComponent, MatProgressSpinner, CommonModule],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit, OnDestroy {

  isLoading: boolean = false;

  private destroy$ = new Subject<void>();

  constructor(
    private configService: ConfigService, 
    private loadingService: LoadingService) {}

  ngOnInit(): void {
    this.configService.initialise();
    this.loadingService.loading$
    .pipe(takeUntil(this.destroy$))
    .subscribe((isLoading) => {
      setTimeout(() => this.isLoading = isLoading);
    });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
