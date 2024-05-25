import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import config from '../config.json';
import {Subject} from "rxjs";
import {AttendanceDto} from "../model/attendance-dto";
import {TaskDto} from "../model/task-dto";

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  private hubConnection!: signalR.HubConnection

  private newAttendanceNotificationSubject = new Subject<AttendanceDto>();
  public newAttendanceNotification$ = this.newAttendanceNotificationSubject.asObservable();

  private logoutNotificationSubject = new Subject<AttendanceDto>();
  public logoutNotification$ = this.logoutNotificationSubject.asObservable();

  private newTaskNotificationSubject = new Subject<TaskDto>();
  public newTaskNotification$ = this.newTaskNotificationSubject.asObservable();

  private updateTaskNotificationSubject = new Subject<TaskDto>();
  public updateTaskNotification$ = this.updateTaskNotificationSubject.asObservable();

  private deleteTaskNotificationSubject = new Subject<number>();
  public deleteTaskNotification$ = this.deleteTaskNotificationSubject.asObservable();

  private completeTaskNotificationSubject = new Subject<number>();
  public completeTaskNotification$ = this.completeTaskNotificationSubject.asObservable();

  public startConnection = () => {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(config.notificationHubUrl, { skipNegotiation: true,
          transport: signalR.HttpTransportType.WebSockets})
      .build();
    this.hubConnection.start()
      .then(
        () => console.log('Connection started'),
        (error) => console.error('Error while starting connection:', error)
      );

    this.addReceiveNotificationListeners();
  }

  public stopConnection = () => {
    this.removeReceiveNotificationListeners();
    this.hubConnection.stop()
      .then(
        () => console.log('Connection stopped'),
        (error) => console.error('Error while stopping connection:', error)
      );
  }

  private addReceiveNotificationListeners = () => {
    this.hubConnection.on('NotifyAttendance', (attendance) => {
      console.log('Attendance notification received:', attendance);
      this.newAttendanceNotificationSubject.next(attendance);
    });

    this.hubConnection.on('NotifyLogout', (attendance) => {
      console.log('Logout notification received:', attendance);
      this.logoutNotificationSubject.next(attendance);
    });

    this.hubConnection.on('NotifyTask', (task) => {
      console.log('Task notification received:', task);
      this.newTaskNotificationSubject.next(task);
    });

    this.hubConnection.on('NotifyTaskUpdate', (task) => {
      console.log('Task update notification received:', task);
      this.updateTaskNotificationSubject.next(task);
    });

    this.hubConnection.on('NotifyTaskDeletion', (taskId) => {
      console.log('Task deletion notification received:', taskId);
      this.deleteTaskNotificationSubject.next(taskId);
    });

    this.hubConnection.on('NotifyTaskCompletion', (taskId) => {
      console.log('Task completion notification received:', taskId);
      this.completeTaskNotificationSubject.next(taskId);
    });
  }

  private removeReceiveNotificationListeners = () => {
    this.hubConnection.off('NotifyAttendance');
    this.hubConnection.off('NotifyLogout');
    this.hubConnection.off('NotifyTask');
    this.hubConnection.off('NotifyTaskUpdate');
    this.hubConnection.off('NotifyTaskDeletion');
    this.hubConnection.off('NotifyTaskCompletion');
  }
}
