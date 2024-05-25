import { Component, OnInit } from '@angular/core';
import { AttendanceService } from '../../services/attendance.service';
import {Router} from "@angular/router";
import {AuthService} from "../../services/auth.service";

@Component({
  selector: 'app-attend-page',
  templateUrl: './attend-page.component.html',
  styleUrls: ['./attend-page.component.css']
})
export class AttendPageComponent implements OnInit {
  attendanceTime: string = '';
  errorMessage: string | undefined = undefined;

  constructor(private authService: AuthService,
              private attendanceService: AttendanceService,
              private router: Router) { }

  ngOnInit(): void {
    if (this.authService.hasAttended()) {
      console.log('User has already attended today');
      this.router.navigate(['/employee-dashboard']);
    }
  }

  attend(): void {
    this.attendanceService.postAttendance(this.authService.getUsername(), this.attendanceTime).subscribe({
      next: (response) => {
        console.log('Attendance added with success');
        this.attendanceTime = '';
        this.authService.setHasAttended(true);
        this.router.navigate(['/employee-dashboard']);
      },
      error: (error) => {
        if (error.status === 450) {
          this.errorMessage = error.error;
        } else if (error.status === 400) {
          this.errorMessage = 'Unknown error';
        }
      }
    });
  }

  logout() {
    this.authService.clearJwtToken();
    this.authService.logout().subscribe(
      response => {
        this.router.navigate(['/login']);
      },
      error => {
        console.error('Error:', error);
      }
    )
  }
}
