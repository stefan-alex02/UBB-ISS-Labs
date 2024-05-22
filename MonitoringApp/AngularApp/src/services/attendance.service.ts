import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AttendanceService {
  private apiUrl = 'https:/localhost:7082/api/attendance';

  constructor(private http: HttpClient) { }

  getAttendances(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl);
  }

  postAttendance(userId: number, time: string): Observable<any> {
    const url = `${this.apiUrl}`;

    return this.http.post(url, {
      MarkedById: userId,
      Time: time
    });
  }
}
