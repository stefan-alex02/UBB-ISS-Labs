import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {Observable, tap} from 'rxjs';
import config from '../config.json';
import {AttendanceDto} from "../model/attendance-dto";

@Injectable({
  providedIn: 'root'
})
export class AttendanceService {
  private getUnfinishedAttendancesUrl = config.baseUrl + '/api/attendances';
  private postAttendanceUrl = config.baseUrl + '/api/attendances';

  constructor(private http: HttpClient) { }

  getUnfinishedAttendances(): Observable<AttendanceDto[]> {
    return this.http.get<AttendanceDto[]>(this.getUnfinishedAttendancesUrl);
  }

  postAttendance(employeeUsername: string, time: string): Observable<any> {
    const url = `${this.postAttendanceUrl}`;

    return this.http.post(url, {
      Username: employeeUsername,
      StartTime: time
    });
  }
}
