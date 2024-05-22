import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import {AttendanceService} from "./attendance.service";
import {UserRoles} from "../app/data/user-roles";

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  private hubConnection!: signalR.HubConnection

  public startConnection = () => {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:7082/notificationHub',
        { skipNegotiation: true,
          transport: signalR.HttpTransportType.WebSockets})
      .build();
    this.hubConnection
      .start()
      .then(() => console.log('Connection started'))
      .catch(err => console.log('Error while starting connection: ' + err))

    this.addReceiveNotificationListeners();
  }

  private addReceiveNotificationListeners = () => {
    this.hubConnection.on('NotifyAttendance', (attendance) => {
      console.log('Attendance notification received:', attendance);
      // Handle the attendance notification here
    });

    this.hubConnection.on('NotifyLogout', (attendance) => {
      console.log('Logout notification received:', attendance);
      // Handle the logout notification here
    });

    this.hubConnection.on('NotifyTask', (task) => {
      console.log('Task notification received:', task);
      // Handle the task notification here
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
}
