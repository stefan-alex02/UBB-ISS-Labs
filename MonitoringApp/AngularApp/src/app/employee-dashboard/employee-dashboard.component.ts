import {Component, OnInit} from '@angular/core';
import {Router} from "@angular/router";
import {AuthService} from "../../services/auth.service";
import {NotificationService} from "../../services/notification.service";
import {TaskService} from "../../services/task.service";
import {TaskDto} from "../../model/task-dto";
import {AttendanceDto} from "../../model/attendance-dto";

@Component({
  selector: 'app-employee-dashboard',
  templateUrl: './employee-dashboard.component.html',
  styleUrl: './employee-dashboard.component.css'
})
export class EmployeeDashboardComponent implements OnInit{
  tasks: TaskDto[] = [];
  selectedRow: number | null = null;
  notificationType: string | null = null;
  notificationMessage: string | null = null;
  private notificationTimeoutId: any;

  constructor(private authService: AuthService,
              private tasksService: TaskService,
              private notificationService: NotificationService,
              private router: Router) { }

  ngOnInit(): void {
    this.displayTasks();
    this.notificationService.newTaskNotification$.subscribe({
      next: (task) => {
        console.log('Task notification received:', task);
        this.notifyNewTask(task);
        this.displayTasks();
      }
    });
    this.notificationService.updateTaskNotification$.subscribe({
      next: (task) => {
        console.log('Task update notification received:', task);
        this.notifyUpdateTask(task);
        this.displayTasks();
      }
    });
    this.notificationService.deleteTaskNotification$.subscribe({
      next: (taskId) => {
        console.log('Task delete notification received:', taskId);
        this.notifyDeleteTask(taskId);
        this.displayTasks();
      }
    });
  }

  completeTask(task: any) {
    this.tasksService.completeTask(task.id).subscribe({
      next: () => {
        this.displayTasks();
      },
      error: (error) => {
        console.error('Error:', error);
      }
    });
  }

  private displayTasks() {
    this.tasksService.getTasksForEmployee(this.authService.getUsername()).subscribe({
      next: (data) => {
        console.log('Received tasks:', data);
        this.tasks = data;
      },
      error: (error) => {
        console.error('Error:', error);
      }
    });
  }

  notifyNewTask(task: TaskDto) {
    this.notificationMessage = 'Info: New task added';
    this.notificationType = 'add-task';

    clearTimeout(this.notificationTimeoutId);
    this.notificationTimeoutId = setTimeout(() => {
      this.notificationMessage = null;
    }, 5000);
  }

  notifyUpdateTask(task: TaskDto) {
    this.notificationMessage = 'Info: Task updated';
    this.notificationType = 'update-task';

    clearTimeout(this.notificationTimeoutId);
    this.notificationTimeoutId = setTimeout(() => {
      this.notificationMessage = null;
    }, 5000);
  }

  notifyDeleteTask(taskId: number) {
    this.notificationMessage = 'Info: Task deleted';
    this.notificationType = 'delete-task';

    clearTimeout(this.notificationTimeoutId);
    this.notificationTimeoutId = setTimeout(() => {
      this.notificationMessage = null;
    }, 5000);
  }
}
