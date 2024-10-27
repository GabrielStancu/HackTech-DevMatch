import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {

  constructor(private router: Router, private authService : AuthService) { }

  public loginUser = {
    username: '',
    password: ''
  }

  public onLoginClicked() {
    this.authService.login(this.loginUser.username, this.loginUser.password).subscribe(data => {
      localStorage.setItem('llm-token', data.token);
      localStorage.setItem('llm-id', data.externalId);
      localStorage.setItem('llm-userType', data.role);
      localStorage.setItem('llm-user', data.username);
      localStorage.setItem('llm-firstName', data.firstName);
      localStorage.setItem('llm-lastName', data.lastName);

      if (data.role === 'developer') {
        this.router.navigate(['/developer']);
      } else if (data.role === 'recruiter') {
        this.router.navigate(['/recruiter']);
      }
    });
  }
}
