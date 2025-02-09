import { ComponentFixture, TestBed } from '@angular/core/testing';
import { LoginPageComponent } from './login-page.component';
import { AuthService } from './auth.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { of } from 'rxjs';
import { ReactiveFormsModule } from '@angular/forms';
import { User } from './user.model';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

describe('LoginPageComponent', () => {
  let component: LoginPageComponent;
  let fixture: ComponentFixture<LoginPageComponent>;
  let authServiceMock: jasmine.SpyObj<AuthService>;
  let snackBarMock: jasmine.SpyObj<MatSnackBar>;

  beforeEach(async () => {
    authServiceMock = jasmine.createSpyObj('AuthService', ['register', 'login']);
    snackBarMock = jasmine.createSpyObj('MatSnackBar', ['open']);

    await TestBed.configureTestingModule({
      imports: [ReactiveFormsModule, BrowserAnimationsModule],
      providers: [
        { provide: AuthService, useValue: authServiceMock },
        { provide: MatSnackBar, useValue: snackBarMock }
      ]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LoginPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  describe('register', () => {
    it('should mark form as touched if invalid', () => {
      spyOn(component.registerForm, 'markAllAsTouched');
      component.registerForm.setValue({ username: '', password: '', passwordConfirmation: '' });
      component.register();
      expect(component.registerForm.markAllAsTouched).toHaveBeenCalled();
    });

    it('should call AuthService.register and reset form on success', () => {
      const mockUser: User = { id: 'id', username: 'testuser', passwordHash: 'passwordHash' };
      authServiceMock.register.and.returnValue(of(mockUser));
      spyOn(component.registerForm, 'reset');

      component.registerForm.setValue({ username: 'testuser', password: 'password', passwordConfirmation: 'password' });
      component.register();

      expect(authServiceMock.register).toHaveBeenCalledWith({ username: 'testuser', password: 'password' });
      expect(component.registerForm.reset).toHaveBeenCalled();
      expect(component.hasRegistered).toBeTrue();
      expect(snackBarMock.open).toHaveBeenCalledWith('Account successfully registered for testuser', 'Close', jasmine.any(Object));
    });
  });

  describe('login', () => {
    it('should mark form as touched if invalid', () => {
      spyOn(component.loginForm, 'markAllAsTouched');
      component.loginForm.setValue({ username: '', password: '' });
      component.login();
      expect(component.loginForm.markAllAsTouched).toHaveBeenCalled();
    });

    it('should call AuthService.login and set token on success', () => {
      authServiceMock.login.and.returnValue(of('mockToken'));
      spyOn(localStorage, 'setItem');
      
      component.loginForm.setValue({ username: 'testuser', password: 'password' });
      component.login();

      expect(authServiceMock.login).toHaveBeenCalledWith({ username: 'testuser', password: 'password' });
      expect(localStorage.setItem).toHaveBeenCalledWith('bearer-token', 'mockToken');
    });
  });

  describe('passwordsMismatchValidator', () => {
    it('should return null if form is not defined', () => {
      component.registerForm = null as any;
      const validator = component.passwordsMismatchValidator();
      expect(validator({ value: 'test' } as any)).toBeNull();
    });

    it('should return error if passwords do not match', () => {
      component.registerForm.controls.password.setValue('password1');
      component.registerForm.controls.passwordConfirmation.setValue('password2');
      const validator = component.passwordsMismatchValidator();
      expect(validator(component.registerForm.controls.passwordConfirmation)).toEqual({ passwordMismatch: { value: 'password2' } });
    });

    it('should return null if passwords match', () => {
      component.registerForm.controls.password.setValue('password');
      component.registerForm.controls.passwordConfirmation.setValue('password');
      const validator = component.passwordsMismatchValidator();
      expect(validator(component.registerForm.controls.passwordConfirmation)).toBeNull();
    });
  });
});