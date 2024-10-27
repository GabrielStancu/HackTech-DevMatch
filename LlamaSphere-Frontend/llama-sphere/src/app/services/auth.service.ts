import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private apiUrl = 'http://192.168.56.163:5100/api/Account';

  constructor(private http: HttpClient) { }

  login(username: string, password: string): Observable<any> {
    const url = `${this.apiUrl}/login`;

    return this.http.post(url, {
      username: username,
      password: password
    });
  }

  register(username: string, password: string, firstName: string, lastName: string, role: string): Observable<any> {
    const url = `${this.apiUrl}/register`;

    return this.http.post(url, {
      username: username,
      password: password,
      firstName: firstName,
      lastName: lastName,
      role: role
    });
  }
}
