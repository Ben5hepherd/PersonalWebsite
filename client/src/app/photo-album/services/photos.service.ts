import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ConfigService } from '../../shared/config.service';
import { Photo } from '../models/photo.model';

@Injectable({
  providedIn: 'root',
})
export class PhotosService {
  constructor(private http: HttpClient, private configService: ConfigService) {}

  getAllPhotos(): Observable<Photo[]> {
    return this.http.get<Photo[]>(`${this.configService.apiUrl}/api/photos`);
  }
}
