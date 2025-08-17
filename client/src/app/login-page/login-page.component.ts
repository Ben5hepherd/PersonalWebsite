import { Component } from '@angular/core';
import { AuthService, RegisterResultDto } from './services/auth.service';
import { CommonModule } from '@angular/common';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import {
  AbstractControl,
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  ValidationErrors,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatCardModule } from '@angular/material/card';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login-page',
  imports: [
    CommonModule,
    MatSelectModule,
    MatInputModule,
    MatFormFieldModule,
    FormsModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatCardModule,
  ],
  templateUrl: './login-page.component.html',
  styleUrl: './login-page.component.scss',
})
export class LoginPageComponent {
  hasRegistered: boolean = false;
  registerError: string | null = null;

  registerForm = new FormGroup({
    username: new FormControl('', [Validators.required]),
    password: new FormControl('', [
      Validators.required,
      this.passwordsMismatchValidator(),
    ]),
    passwordConfirmation: new FormControl('', [
      Validators.required,
      this.passwordsMismatchValidator(),
    ]),
  });

  loginForm = new FormGroup({
    username: new FormControl('', [Validators.required]),
    password: new FormControl('', [Validators.required]),
  });

  constructor(
    private authService: AuthService,
    private snackBar: MatSnackBar,
    private router: Router
  ) {}

  register(): void {
    this.registerError = null;

    if (this.registerForm.valid === false) {
      this.registerForm.markAllAsTouched();
      return;
    }

    this.authService
      .register({
        username: this.registerForm.controls.username.value ?? '',
        password: this.registerForm.controls.password.value ?? '',
      })
      .subscribe((result: RegisterResultDto) => {
        if (result.success && result.user) {
          this.hasRegistered = true;
          this.registerForm.reset();
          this.openAccountRegisteredSnackBar(result.user.username);
        } else {
          this.registerError = result.errorMessage || 'Registration failed';
        }
      });
  }

  login(): void {
    if (this.loginForm.valid === false) {
      this.loginForm.markAllAsTouched();
      return;
    }

    this.authService
      .login({
        username: this.loginForm.controls.username.value ?? '',
        password: this.loginForm.controls.password.value ?? '',
      })
      .subscribe((token) => {
        localStorage.setItem('bearer-token', token);
        this.router.navigate(['/items']);
      });
  }

  openAccountRegisteredSnackBar(username: string): void {
    this.snackBar.open(
      `Account successfully registered for ${username}`,
      'Close',
      {
        duration: 2000,
        horizontalPosition: 'center',
        verticalPosition: 'bottom',
      }
    );
  }

  passwordsMismatchValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      if (!this.registerForm) return null;

      const passwordValue = this.registerForm.controls.password.value;
      const passwordConfirmationValue =
        this.registerForm.controls.passwordConfirmation.value;

      if (!passwordValue || !passwordConfirmationValue) return null;

      this.registerForm.controls.password.setErrors(null);
      this.registerForm.controls.passwordConfirmation.setErrors(null);

      const passwordMismatch = passwordValue !== passwordConfirmationValue;

      return passwordMismatch
        ? { passwordMismatch: { value: control.value } }
        : null;
    };
  }
}
