import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ConfigService } from '../config.service';

@Injectable({
  providedIn: 'root'
})
export class ItemService {

  constructor(private http: HttpClient, private configService: ConfigService) {}

  getItems(): Observable<string[]> {
    return this.http.get<string[]>(`${this.configService.apiUrl}/api/items/`);
  }
}
