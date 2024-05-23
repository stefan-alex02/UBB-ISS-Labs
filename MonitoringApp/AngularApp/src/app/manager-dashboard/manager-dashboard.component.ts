import { Component, OnInit } from '@angular/core';
import { AttendanceService } from '../../services/attendance.service';
import { TasksService } from '../../services/task.service';
import {Router} from "@angular/router";
import {NotificationService} from "../../services/notification.service";
import {AuthService} from "../../services/auth.service";
import {tap} from "rxjs";
import {AttendanceDto} from "../../model/attendance-dto";

@Component({
  selector: 'app-manager-dashboard',
  templateUrl: './manager-dashboard.component.html',
  styleUrls: ['./manager-dashboard.component.css']
})
export class ManagerDashboardComponent implements OnInit {
  attendances: AttendanceDto[] = [];
  selectedRow: number | null = null;
  successMessage: string | null = null;
  taskDescription: string = '';

  constructor(private authService: AuthService,
              private attendanceService: AttendanceService,
              private tasksService: TasksService,
              private router: Router,
              private notificationService: NotificationService) { }

  ngOnInit(): void {
    this.displayAttendances();
    this.notificationService.startConnection();
    this.notificationService.newAttendanceNotification$.subscribe({
      next: (attendance) => {
        console.log('Attendance notification received:', attendance);
        this.displayAttendances();
      }
    });
  }

  displayAttendances(): void {
    this.attendanceService.getUnfinishedAttendances().subscribe({
      next: (data) => {
        console.log('Received unfinished attendances:', data);
        this.attendances = data;
      },
      error: (error) => {
        console.error('Error:', error);
      }
    });
  }

  assignTask(): void {
    if (this.selectedRow !== undefined && this.selectedRow !== null) {
      const managerUsername = this.authService.getUsername();
      const employeeUsername = this.attendances[this.selectedRow].username;

      this.tasksService.assignTask(managerUsername, employeeUsername, this.taskDescription).subscribe({
        next: (data) => {
          this.successMessage = 'Task added with success';
          setTimeout(() => this.successMessage = null, 5000);
          this.taskDescription = '';
          this.selectedRow = null;
        },
        error: (error) => {
          console.error('Error:', error);
        }
      });
    }
  }

  logout(): void {
    this.authService.logout().subscribe(() => {
      console.log('Logged out');
      this.router.navigate(['/login']);
    });
  }
}
