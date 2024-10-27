import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-app-accept-reject-dialog',
  templateUrl: './app-accept-reject-dialog.component.html',
  styleUrls: ['./app-accept-reject-dialog.component.css']
})
export class AppAcceptRejectDialogComponent {
  emailMessage: string;

  constructor(
    public dialogRef: MatDialogRef<AppAcceptRejectDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: string
  ) {
    // Initialize emailMessage with the response data
    this.emailMessage = data;
  }

  onCancel(): void {
    this.dialogRef.close();
  }

  onSend(): void {
    // Pass the edited message back to the calling component
    this.dialogRef.close(this.emailMessage);
  }
}
