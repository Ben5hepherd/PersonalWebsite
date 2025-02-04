import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ConfigService } from '../config.service';
import { User } from './user.model';
import { UserDto } from './user.dto';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http: HttpClient, private configService: ConfigService) {}

  register(user: UserDto): Observable<User> {
    return this.http.post<any>(`${this.configService.apiUrl}/api/auth/register`, user);
  }

  login(user: UserDto): Observable<string> {
    return this.http.post<string>(`${this.configService.apiUrl}/api/auth/login`, user, { responseType: 'text' as 'json' });
  }
}
