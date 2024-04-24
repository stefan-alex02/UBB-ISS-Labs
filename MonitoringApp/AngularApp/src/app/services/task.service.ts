import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import {CookieService} from "ngx-cookie-service";

@Injectable({
  providedIn: 'root'
})
export class TasksService {
  private apiUrl = 'https://localhost:7082/api/task'; // Replace with your API URL

  constructor(private cookieService: CookieService, private http: HttpClient) { }

  assignTask(employeeId: number, taskDescription: string): Observable<any> {
    const url = `${this.apiUrl}`;

    console.log({
      Description: taskDescription,
      AssignedToId: employeeId,
      CreatedById: JSON.parse(this.cookieService.get('user')).id,
    });
    return this.http.post(url, {
      Description: taskDescription,
      AssignedToId: employeeId,
      CreatedById: JSON.parse(this.cookieService.get('user')).id,
    });
  }
}
