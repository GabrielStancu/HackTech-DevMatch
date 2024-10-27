import { Component } from '@angular/core';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {

  public registerUser = {
    username: '',
    password: '',
    repeatedPassword: '',
    firstName: '',
    lastName: '',
    birthday: '',
    userType: ''
  };
  userTypes: any[] = [];

  ngOnInit(): void {
    this.userTypes = [
      {value: "Developer", viewValue: 'Developer'},
      {value: "Recruiter", viewValue: 'Recruiter'}
    ];
  }

  onRegisterClicked() {
    console.log(this.registerUser);
  }
}
