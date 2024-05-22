import { Component, OnInit } from '@angular/core';
import { AttendanceService } from '../../services/attendance.service';
import { TasksService } from '../../services/task.service';
import {CookieService} from "ngx-cookie-service";
import {Router} from "@angular/router";
import {NotificationService} from "../../services/notification.service";

@Component({
  selector: 'app-manager-dashboard',
  templateUrl: './manager-dashboard.component.html',
  styleUrls: ['./manager-dashboard.component.css']
})
export class ManagerDashboardComponent implements OnInit {
  attendances: any[] = [];
  selectedRow: number | null = null;
  successMessage: string | null = null;
  taskDescription: string = '';

  constructor(private attendanceService: AttendanceService,
              private tasksService: TasksService,
              private cookieService: CookieService,
              private router: Router,
              private notificationService: NotificationService) { }

  ngOnInit(): void {
    this.updateAttendances();
    this.notificationService.startConnection();
  }

  updateAttendances(): void {
    this.attendanceService.getAttendances().subscribe(
      data => {
        console.log('Attendances:', data)
        this.attendances = data;
      },
      error => {
        console.error('Error:', error);
      }
    );
  }

  assignTask(): void {
    if (this.selectedRow !== undefined && this.selectedRow !== null) {
      const employeeId = this.attendances[this.selectedRow].markedById;
      this.tasksService.assignTask(employeeId, this.taskDescription).subscribe(
        response => {
          this.successMessage = 'Task added with success';
          setTimeout(() => this.successMessage = null, 5000);
          this.taskDescription = '';
          this.selectedRow = null;
        },
        error => {
          console.error('Error:', error);
        }
      );
    }
  }

  logout(): void {
    this.cookieService.delete('user');
    this.router.navigate(['/login']);
  }
}
