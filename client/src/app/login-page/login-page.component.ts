import { Component } from '@angular/core';
import { AuthService } from './auth.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login-page',
  imports: [CommonModule],
  providers: [AuthService],
  templateUrl: './login-page.component.html',
  styleUrl: './login-page.component.scss'
})
export class LoginPageComponent {

  data: any;

  constructor(private authService: AuthService) {}

  ngOnInit() {
    this.authService.dummyMethod().subscribe((data) => {
      this.data = data;
    });
  }
}
