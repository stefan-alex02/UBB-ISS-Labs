import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import config from "../config.json";
import {TaskDto} from "../model/task-dto";

@Injectable({
  providedIn: 'root'
})
export class TasksService {
  private assignTaskUrl = config.baseUrl + '/api/tasks';

  private dateOptions: Intl.DateTimeFormatOptions = {
    year: 'numeric',
    month: '2-digit',
    day: '2-digit'
  };

  private timeOptions: Intl.DateTimeFormatOptions = {
    hour: '2-digit',
    minute: '2-digit',
    hour12: false
  };

  constructor(private http: HttpClient) { }

  assignTask(managerUsername: string, employeeUsername: string, taskDescription: string): Observable<any> {
    const now = new Date();

    let currentDate = now.toLocaleDateString('en-CA', this.dateOptions);
    currentDate = currentDate.replace(/\//g, ':');

    let currentTime = now.toLocaleTimeString('en-GB', this.timeOptions);

    console.log('Assigning task to', employeeUsername, 'with description:', taskDescription);
    console.log('Current date:', currentDate, 'Current time:', currentTime);

    return this.http.post(this.assignTaskUrl, {
      CreatedByUsername: managerUsername,
      AssignedToUsername: employeeUsername,
      Description: taskDescription,
      AssignedDate: currentDate,
      AssignedTime: currentTime
    });
  }

  public getTasksForEmployee(employeeId: number): Observable<TaskDto[]> {
    const url = `${config.baseUrl}/api/tasks/${employeeId}`;
    return this.http.get<TaskDto[]>(url);
  }
}
