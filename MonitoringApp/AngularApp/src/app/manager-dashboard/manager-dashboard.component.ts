import { Component, OnInit } from '@angular/core';
import { AttendanceService } from '../../services/attendance.service';
import { TaskService } from '../../services/task.service';
import {Router} from "@angular/router";
import {NotificationService} from "../../services/notification.service";
import {AuthService} from "../../services/auth.service";
import {AttendanceDto} from "../../model/attendance-dto";
import {UserRoles} from "../../model/data/user-roles";

@Component({
  selector: 'app-manager-dashboard',
  templateUrl: './manager-dashboard.component.html',
  styleUrls: ['./manager-dashboard.component.css']
})
export class ManagerDashboardComponent implements OnInit {
  showAssignTaskEditor: boolean = false;

  attendances: AttendanceDto[] = [];
  selectedRow: number | null = null;
  message: string = "Select a message";
  taskDescription: string = '';

  constructor(private authService: AuthService,
              private attendanceService: AttendanceService,
              private tasksService: TaskService,
              private router: Router,
              private notificationService: NotificationService) { }

  ngOnInit(): void {
    this.displayAttendances();
    this.notificationService.newAttendanceNotification$.subscribe({
      next: (attendance) => {
        console.log('Attendance notification received:', attendance);
        this.displayAttendances();
      }
    });
    this.notificationService.logoutNotification$.subscribe({
      next: (attendance) => {
        if (attendance.userRole === UserRoles.Employee) {
          console.log('Employee logout notification received:', attendance);
          this.displayAttendances();
        }
        else {
          console.log('Received logout notification, but the user is not an employee.');
        }
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

  selectRow(i: number) {
    this.selectedRow = this.selectedRow === i ? null : i;
    this.showAssignTaskEditor = false;
    this.message = "Select a message";
  }

  navigateToManageTasks(): void {
    if (this.selectedRow !== undefined && this.selectedRow !== null) {
      this.router.navigate(['/manage-tasks',
        this.attendances[this.selectedRow].username]);
    }
  }

  assignTask(): void {
    if (this.selectedRow !== undefined && this.selectedRow !== null) {
      const managerUsername = this.authService.getUsername();
      const employeeUsername = this.attendances[this.selectedRow].username;

      this.tasksService.assignTask(managerUsername, employeeUsername, this.taskDescription).subscribe({
        next: (data) => {
          this.message = 'Task added with success';
          this.taskDescription = '';
          this.selectedRow = null;
        },
        error: (error) => {
          console.error('Error:', error);
        }
      });
    }
  }

  navigateToRegister() {
    this.router.navigate(['/register']);
  }
}
