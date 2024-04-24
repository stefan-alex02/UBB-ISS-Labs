import { Component, OnInit } from '@angular/core';
import { AttendanceService } from '../services/attendance.service';
import { CookieService } from 'ngx-cookie-service';
import {Router} from "@angular/router";

@Component({
  selector: 'app-attend-page',
  templateUrl: './attend-page.component.html',
  styleUrls: ['./attend-page.component.css']
})
export class AttendPageComponent implements OnInit {
  attendanceTime: string = '';
  errorMessage: string | undefined = undefined;

  constructor(private attendanceService: AttendanceService,
              private cookieService: CookieService,
              private router: Router) { }

  ngOnInit(): void {
  }

  attend(): void {
    const userId = JSON.parse(this.cookieService.get('user')).id;
    this.attendanceService.postAttendance(userId, this.attendanceTime).subscribe(
      response => {
        console.log('Attendance added with success');
        this.attendanceTime = '';
        this.router.navigate(['/employee-dashboard']); // Navigate to employee-dashboard page
      },
      error => {
        if (error.status === 450) {
          this.errorMessage = error.error;
        } else if (error.status === 400) {
          this.errorMessage = 'Unknown error';
        }
      }
    );
  }
}
