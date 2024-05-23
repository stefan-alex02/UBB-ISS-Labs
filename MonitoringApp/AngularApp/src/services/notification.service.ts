import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import config from '../config.json';
import {Subject} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  private hubConnection!: signalR.HubConnection

  private newAttendanceNotificationSubject = new Subject<any>();
  public newAttendanceNotification$ = this.newAttendanceNotificationSubject.asObservable();

  private newTaskNotificationSubject = new Subject<any>();
  public newTaskNotification$ = this.newTaskNotificationSubject.asObservable();


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
      // Handle the logout notification here
    });

    this.hubConnection.on('NotifyTask', (task) => {
      console.log('Task notification received:', task);
      this.newTaskNotificationSubject.next(task);
    });

    this.hubConnection.on('NotifyTaskUpdate', (task) => {
      console.log('Task update notification received:', task);
      // Handle the task update notification here
    });

    this.hubConnection.on('NotifyTaskDeletion', (taskId) => {
      console.log('Task deletion notification received:', taskId);
      // Handle the task deletion notification here
    });
  }

  private removeReceiveNotificationListeners = () => {
    this.hubConnection.off('NotifyAttendance');
    this.hubConnection.off('NotifyLogout');
    this.hubConnection.off('NotifyTask');
    this.hubConnection.off('NotifyTaskUpdate');
    this.hubConnection.off('NotifyTaskDeletion');
  }
}
