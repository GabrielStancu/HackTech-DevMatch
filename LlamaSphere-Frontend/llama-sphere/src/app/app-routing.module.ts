import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { RecruiterComponent } from './components/recruiter/recruiter.component';
import { DeveloperComponent } from './components/developer/developer.component';
import { DeveloperCvComponent } from './components/developer-cv/developer-cv.component';
import { RecruiterJobComponent } from './components/recruiter-job/recruiter-job.component';
import { FindDevsComponent } from './components/find-devs/find-devs.component';
import { FindJobComponent } from './components/find-job/find-job.component';

const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'recruiter', component: RecruiterComponent },
  { path: 'find-devs', component: FindDevsComponent },
  { path: 'find-jobs', component: FindJobComponent },
  { path: 'developer', component: DeveloperComponent },
  { path: 'cv-upload', component: DeveloperCvComponent },
  { path: 'job-upload', component: RecruiterJobComponent },
  { path: 'developer-cv', component: DeveloperCvComponent },
  { path: '', component: LoginComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
