import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ConfigService } from '../../shared/config.service';
import { User } from '../models/user.model';
import { UserDto } from '../dtos/user.dto';

export interface RegisterResultDto {
  success: boolean;
  errorMessage?: string;
  user?: User;
}

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  constructor(private http: HttpClient, private configService: ConfigService) {}

  register(user: UserDto): Observable<RegisterResultDto> {
    return this.http.post<RegisterResultDto>(
      `${this.configService.apiUrl}/api/auth/register`,
      user
    );
  }

  login(user: UserDto): Observable<string> {
    return this.http.post<string>(
      `${this.configService.apiUrl}/api/auth/login`,
      user,
      { responseType: 'text' as 'json' }
    );
  }
}
