import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ConfigService } from '../config.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http: HttpClient, private configService: ConfigService) {}

  dummyMethod(): Observable<any> {
      return this.http.get<any>(`${this.configService.apiUrl}/api/auth`);
  }
}
