import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import {AttendanceService} from "./attendance.service";
import {UserRoles} from "../data/user-roles";

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
  }

  public addReceiveNotificationListener = (update: () => void) => {
    this.hubConnection.on('ReceiveNotification', (user, message) => {
      console.log(message);
        update();
    });
  }
}
