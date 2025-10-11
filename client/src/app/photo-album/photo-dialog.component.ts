import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-photo-dialog',
  template: `
    <div class="photo-container" (click)="close()">
      <img [src]="data.url" [alt]="data.title" />
    </div>
  `,
  styles: [
    `
      .photo-container {
        display: flex;
        align-items: center;
        justify-content: center;
        background: rgba(0, 0, 0, 0.9);
        width: 100%;
        height: 100%;
        cursor: zoom-out;
      }
      img {
        max-width: 95%;
        max-height: 95%;
        border-radius: 8px;
        box-shadow: 0 0 20px rgba(0, 0, 0, 0.5);
      }
    `,
  ],
})
export class PhotoDialogComponent {
  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    private dialogRef: MatDialogRef<PhotoDialogComponent>
  ) {}

  close(): void {
    this.dialogRef.close();
  }
}
