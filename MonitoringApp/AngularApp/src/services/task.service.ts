import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import {Observable, throwError} from 'rxjs';
import config from "../config.json";
import {TaskDto} from "../model/task-dto";
import {catchError} from "rxjs/operators";

@Injectable({
  providedIn: 'root'
})
export class TaskService {
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

  private getCurrentDateAndTime(): { currentDate: string, currentTime: string } {
    const now = new Date();

    let currentDate = now.toLocaleDateString('en-CA', this.dateOptions);
    currentDate = currentDate.replace(/\//g, ':');

    let currentTime = now.toLocaleTimeString('en-GB', this.timeOptions);

    return { currentDate, currentTime };
  }

  assignTask(managerUsername: string, employeeUsername: string, taskDescription: string): Observable<any> {
    let { currentDate, currentTime } = this.getCurrentDateAndTime();

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

  public getTasksForEmployee(employeeUsername: string): Observable<TaskDto[]> {
    const url = `${config.baseUrl}/api/tasks/${employeeUsername}`;
    return this.http.get<TaskDto[]>(url);
  }

  public updateTask(taskId: number, taskDescription: string): Observable<any> {
    const url = `${config.baseUrl}/api/tasks/${taskId}`;

    let { currentDate, currentTime } = this.getCurrentDateAndTime();

    return this.http.put(url, {
      Description: taskDescription,
      AssignedDate: currentDate,
      AssignedTime: currentTime
    });
  }

  public deleteTask(taskId: number): Observable<any> {
    const url = `${config.baseUrl}/api/tasks/${taskId}`;
    return this.http.delete(url);
  }

  public completeTask(taskId: number): Observable<any> {
    const url = `${config.baseUrl}/api/tasks/${taskId}/complete`;
    return this.http.put(url, null);
  }
}
