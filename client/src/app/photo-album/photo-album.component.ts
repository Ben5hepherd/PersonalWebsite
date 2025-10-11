import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatDialogModule } from '@angular/material/dialog';
import { PhotosService } from './services/photos.service';
import { Photo } from './models/photo.model';
import { MatDialog } from '@angular/material/dialog';
import { PhotoDialogComponent } from './photo-dialog.component';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';

@Component({
  selector: 'app-photo-album',
  imports: [MatCardModule, MatGridListModule, CommonModule, MatDialogModule],
  templateUrl: './photo-album.component.html',
})
export class PhotoAlbumComponent implements OnInit {
  photos: Photo[] = [];
  isMobile = false;

  constructor(
    private readonly photosService: PhotosService,
    private readonly dialog: MatDialog,
    public breakpointObserver: BreakpointObserver
  ) {}

  ngOnInit(): void {
    this.breakpointObserver
      .observe([Breakpoints.Small, Breakpoints.XSmall])
      .subscribe((result) => (this.isMobile = result.matches));

    this.photosService.getAllPhotos().subscribe((data) => {
      this.photos = data;
    });
  }

  openPhoto(photo: any): void {
    this.dialog.open(PhotoDialogComponent, {
      data: photo,
      maxWidth: '100vw',
      maxHeight: '100vh',
      height: '100%',
      width: '100%',
    });
  }
}
