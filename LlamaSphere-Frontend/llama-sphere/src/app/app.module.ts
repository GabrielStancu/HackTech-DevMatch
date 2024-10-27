import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatListModule } from '@angular/material/list';
import { MatSelectModule } from '@angular/material/select';
import { MatNativeDateModule } from '@angular/material/core';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { DeveloperComponent } from './components/developer/developer.component';
import { RecruiterComponent } from './components/recruiter/recruiter.component';
import { DndDirective } from './directives/dnd.directive';
import { FileUploadComponent } from './components/shared/file-upload/file-upload.component';
import { ProgressComponent } from './components/shared/progress/progress.component';
import { DeveloperCvComponent } from './components/developer-cv/developer-cv.component';
import { RecruiterJobComponent } from './components/recruiter-job/recruiter-job.component';
import { NavigationComponent } from './components/shared/navigation/navigation.component';
import { TopbarComponent } from './components/shared/topbar/topbar.component';
import { MatchingService } from './services/matching.service';
import { HttpClientModule } from '@angular/common/http';
import { MatTableModule } from '@angular/material/table';
import { FindJobComponent } from './components/find-job/find-job.component';
import { FindDevsComponent } from './components/find-devs/find-devs.component';
import { MatDialogModule } from '@angular/material/dialog';
import { AppAcceptRejectDialogComponent } from './components/app-accept-reject-dialog/app-accept-reject-dialog.component';
import { UploadService } from './services/upload.service';
import { AuthService } from './services/auth.service';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    RegisterComponent,
    DeveloperComponent,
    RecruiterComponent,
    DndDirective,
    FileUploadComponent,
    ProgressComponent,
    DeveloperCvComponent,
    RecruiterJobComponent,
    NavigationComponent,
    TopbarComponent,
    FindJobComponent,
    FindDevsComponent,
    AppAcceptRejectDialogComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    BrowserAnimationsModule,
    HttpClientModule,
    MatTableModule,
    MatDialogModule,

    MatDatepickerModule,
    MatFormFieldModule,
    MatListModule,
    MatSelectModule,
    MatNativeDateModule,
    MatInputModule,
    MatIconModule
  ],
  providers: [MatchingService, UploadService, AuthService],
  bootstrap: [AppComponent]
})
export class AppModule { }
