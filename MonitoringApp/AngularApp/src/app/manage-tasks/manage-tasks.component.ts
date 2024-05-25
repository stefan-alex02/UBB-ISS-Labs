import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";
import {TaskService} from "../../services/task.service";
import {TaskDto} from "../../model/task-dto";
import {NotificationService} from "../../services/notification.service";

@Component({
  selector: 'app-manage-tasks',
  templateUrl: './manage-tasks.component.html',
  styleUrl: './manage-tasks.component.css'
})
export class ManageTasksComponent implements OnInit {
  username: string | null = null;

  tasks: TaskDto[] = [];
  selectedRow: number | null = null;
  message: string = "Select a message";
  errorMessage: string | null = null
  taskDescription: string = '';

  constructor(private taskService: TaskService,
              private route: ActivatedRoute,
              private router: Router,
              private notificationService: NotificationService) { }

  ngOnInit(): void {
    this.username = this.route.snapshot.paramMap.get('username');
    this.displayTasks();
    this.notificationService.logoutNotification$.subscribe({
      next: (attendance) => {
        console.log('Logout notification received:', attendance);
        this.router.navigate(['/manager-dashboard']);
      }
    });
    this.notificationService.completeTaskNotification$.subscribe({
      next: (taskId) => {
        console.log('Task completed notification received:', taskId);
        this.displayTasks();
      }
    });
  }

  navigateBack() {
    this.router.navigate(['/manager-dashboard']);
  }

  private displayTasks() {
    // Display tasks for the user with this.username
    this.taskService.getTasksForEmployee(this.username ?? '').subscribe({
      next: (tasks) => {
        console.log('Received tasks:', tasks);
        this.tasks = tasks;
      },
      error: (error) => {
        console.error('Error:', error);
      }
    });
  }

  selectRow(i: number) {
    this.selectedRow = this.selectedRow === i ? null : i;
    this.taskDescription = this.tasks[i].description;
    this.message = "Select a message";
  }

  updateTask() {
    if (this.selectedRow !== undefined && this.selectedRow !== null) {
      const task = this.tasks[this.selectedRow];
      if (!this.taskDescription.trim() || this.taskDescription.trim() === '') {
        this.errorMessage = 'Task description cannot be empty';
        return;
      }
      if (this.taskDescription.trim() === task.description) {
        this.errorMessage = 'Task description is the same as the current one';
        return;
      }
      this.errorMessage = null;
      this.selectedRow = null;
      this.taskService.updateTask(task.id, this.taskDescription).subscribe({
        next: () => {
          this.message = 'Task updated with success';
          this.displayTasks();
        },
        error: (error) => {
          console.error('Error:', error);
          this.message = 'Error updating task: ' + (error.error ?? 'Unknown error');
        }
      });
    }
  }

  deleteTask() {
    if (this.selectedRow !== undefined && this.selectedRow !== null) {
      const task = this.tasks[this.selectedRow];
      this.selectedRow = null;
      this.taskService.deleteTask(task.id).subscribe({
        next: () => {
          this.message = 'Task deleted with success';
          this.displayTasks();
        },
        error: (error) => {
          console.error('Error:', error);
          this.message = 'Error deleting task: ' + (error.error ?? 'Unknown error');
        }
      });
    }
  }
}
