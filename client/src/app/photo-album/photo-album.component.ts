import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatGridListModule } from '@angular/material/grid-list';
import { PhotosService } from './services/photos.service';
import { Photo } from './models/photo.model';

@Component({
  selector: 'app-photo-album',
  imports: [MatCardModule, MatGridListModule, CommonModule],
  templateUrl: './photo-album.component.html',
})
export class PhotoAlbumComponent implements OnInit {
  photos: Photo[] = [];

  constructor(private readonly photosService: PhotosService) {}

  ngOnInit(): void {
    this.photosService.getAllPhotos().subscribe((data) => {
      this.photos = data;
    });
  }
}
