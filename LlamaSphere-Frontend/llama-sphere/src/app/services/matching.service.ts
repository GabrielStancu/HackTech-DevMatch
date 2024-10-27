import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MatchingService {

  private apiUrl = 'http://192.168.56.163:5045/api';
  private faqUrl = 'http://127.0.0.1:8000';
  private reasoningUrl = 'http://192.168.1.7:5000';

  constructor(private http: HttpClient) { }

  getAllJobs(): Observable<any[]> {
    // const url = `${this.apiUrl}/developer/all-jobs`;
    // return this.http.get<any[]>(url);
    // Hardcoded job data
    // const jobs = [
    //   {
    //     id: '1',
    //     title: 'Senior Software Engineer',
    //     score: 95,
    //     explanation: 'Matched skills: Java, Angular; Industry experience: Banking'
    //   },
    //   {
    //     id: '2',
    //     title: 'Full Stack Developer',
    //     score: 88,
    //     explanation: 'Matched skills: Node.js, React; Industry experience: Healthcare'
    //   },
    //   {
    //     id: '3',
    //     title: 'Backend Developer',
    //     score: 80,
    //     explanation: 'Matched skills: Python, Django; Industry experience: E-commerce'
    //   }
    // ];
    const jobs = [
      {
        id: '11a1349b-6096-4aac-ae9b-11f198e8b43b',
        title: 'Junior Backend Developer',
      },
      {
        id: '518da9ea-2fe2-4641-be17-115e29d8474f',
        title: 'Senior Project Manager',
      },
      {
        id: '6213eef2-0a9a-4072-816a-8071c5eae82f',
        title: 'Machine Learning Engineer',
      }
    ];

    // Return the jobs as an Observable
    return of(jobs);
  }

  getAllDevelopers(): Observable<any> {
    // return this.http.get(`${this.apiUrl}/developers`);
    const candidates = [
      {
        name: 'Vulsan Bianca',
      },
      {
        name: 'Muscalagiu Anca',
      },
      {
        name: 'Toma Maria',
      }
    ];

    return of(candidates);
  }

  getMatchedJobs(data: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/Jobs`, data);
    // const jobs = [
    //   {
    //     id: '1',
    //     title: 'Senior Software Engineer1',
    //     score: 95,
    //     explanation: 'Matched skills: Java, Angular; Industry experience: Banking'
    //   },
    //   {
    //     id: '2',
    //     title: 'Full Stack Developer1',
    //     score: 88,
    //     explanation: 'Matched skills: Node.js, React; Industry experience: Healthcare'
    //   },
    //   {
    //     id: '3',
    //     title: 'Backend Developer1',
    //     score: 80,
    //     explanation: 'Matched skills: Python, Django; Industry experience: E-commerce'
    //   }
    // ];

    // Return the jobs as an Observable
    // return of(jobs);
  }

  getMatchedCandidates(data: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/Cvs`, data);
  }

  sendFaqQuestion(user_question: string): Observable<any> {
    const url = `${this.faqUrl}/ask-question`;
    return this.http.post<any>(url, { user_question });
  }

  sendEmail(data: any): Observable<any> {
    const url = `${this.apiUrl}/Respond`;
    return this.http.post<any>(url, data );
    // return of('send email response');
  }

  getEmailTemplate(data: any): Observable<any> {
    const url = `${this.reasoningUrl}/email_response`;
    return this.http.post<any>(url, data );
    // return of('Email text for accepting the candidate.');
  }

  getFinalReasoning(data: any): Observable<any> {
    const url = `${this.reasoningUrl}/reasoning`;
    return this.http.post<any>(url, data );
    // return of('Email text for accepting the candidate.');
  }
}
