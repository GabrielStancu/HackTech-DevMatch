import { Component } from '@angular/core';
import { UploadService } from 'src/app/services/upload.service';

@Component({
  selector: 'app-developer-cv',
  templateUrl: './developer-cv.component.html',
  styleUrls: ['./developer-cv.component.css']
})
export class DeveloperCvComponent {

  constructor(private uploadService: UploadService) { }

  public receiveFile(file: File) {
    this.uploadService.uploadCv(file).subscribe(result =>
      alert("File Uploaded!")
    );
  }
}
