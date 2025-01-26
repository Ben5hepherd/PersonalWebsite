import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ConfigService {
  apiUrl: string = '';

  constructor(private http: HttpClient) {}

  initialise() {
    if (environment.production) {
      this.http.get<{ apiUrl: string }>('/nginx/config').subscribe((config) => {
        this.apiUrl = config.apiUrl;
      });
    }
    else {
      this.apiUrl = environment.config.apiUrl;
    }
  }
}
