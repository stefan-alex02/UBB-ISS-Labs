import {Component, OnInit} from '@angular/core';
import {Router} from "@angular/router";
import {AuthService} from "../../services/auth.service";
import {AttendanceDto} from "../../model/attendance-dto";
import {NotificationService} from "../../services/notification.service";
import {TasksService} from "../../services/task.service";
import {TaskDto} from "../../model/task-dto";

@Component({
  selector: 'app-employee-dashboard',
  templateUrl: './employee-dashboard.component.html',
  styleUrl: './employee-dashboard.component.css'
})
export class EmployeeDashboardComponent implements OnInit{
  tasks: TaskDto[] = [];
  selectedRow: number | null = null;

  constructor(private authService: AuthService,
              private tasksService: TasksService,
              private notificationService: NotificationService,
              private router: Router) { }

  ngOnInit(): void {
    this.displayTasks();
    this.notificationService.startConnection();
    this.notificationService.newTaskNotification$.subscribe({
      next: (task) => {
        console.log('Task notification received:', task);
        this.displayTasks();
      }
    });
  }

  logout(): void {
    this.authService.logout().subscribe({
      next: () => {
        this.router.navigate(['/login']);
      },
      error: (error) => {
        console.error('Error:', error);
      }
    });
  }

  completeTask(task: any) {

  }

  private displayTasks() {
    this.tasksService.getTasksForEmployee(this.authService.getUserId()).subscribe({
      next: (data) => {
        console.log('Received tasks:', data);
        this.tasks = data;
      },
      error: (error) => {
        console.error('Error:', error);
      }
    });
  }
}
