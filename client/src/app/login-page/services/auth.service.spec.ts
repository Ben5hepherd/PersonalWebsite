import { TestBed } from '@angular/core/testing';
import { AuthService } from './auth.service';
import {
  HttpClientTestingModule,
  HttpTestingController,
} from '@angular/common/http/testing';
import { ConfigService } from '../../shared/config.service';
import { User } from '../models/user.model';
import { UserDto } from '../dtos/user.dto';

describe('AuthService', () => {
  let service: AuthService;
  let httpMock: HttpTestingController;
  let configServiceMock: jasmine.SpyObj<ConfigService>;

  beforeEach(() => {
    configServiceMock = jasmine.createSpyObj('ConfigService', ['apiUrl']);
    configServiceMock.apiUrl = 'http://mock-api.com';

    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        AuthService,
        { provide: ConfigService, useValue: configServiceMock },
      ],
    });

    service = TestBed.inject(AuthService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  describe('register', () => {
    it('should send a POST request and return a User', () => {
      const mockUserDto: UserDto = {
        username: 'testuser',
        password: 'password',
      };
      const mockUser: User = {
        id: '1',
        username: 'testuser',
        passwordHash: 'hash',
      };

      service.register(mockUserDto).subscribe((user) => {
        expect(user).toEqual(mockUser);
      });

      const req = httpMock.expectOne(
        `${configServiceMock.apiUrl}/api/auth/register`
      );
      expect(req.request.method).toBe('POST');
      expect(req.request.body).toEqual(mockUserDto);
      req.flush(mockUser);
    });
  });

  describe('login', () => {
    it('should send a POST request and return a token', () => {
      const mockUserDto: UserDto = {
        username: 'testuser',
        password: 'password',
      };
      const mockToken = 'mockToken';

      service.login(mockUserDto).subscribe((token) => {
        expect(token).toBe(mockToken);
      });

      const req = httpMock.expectOne(
        `${configServiceMock.apiUrl}/api/auth/login`
      );
      expect(req.request.method).toBe('POST');
      expect(req.request.body).toEqual(mockUserDto);
      req.flush(mockToken);
    });
  });
});
