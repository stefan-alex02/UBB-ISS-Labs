import { Component } from '@angular/core';
import {AuthService} from "../../services/auth.service";
import {Router} from "@angular/router";
import {NotificationService} from "../../services/notification.service";
import {UserRoles} from "../../model/data/user-roles";

@Component({
  selector: 'app-user-wrapper',
  templateUrl: './user-wrapper.component.html',
  styleUrl: './user-wrapper.component.css'
})
export class UserWrapperComponent {
  userName : string = '';

  constructor(private notificationService: NotificationService,
              protected authService: AuthService,
              private router: Router) {
  }

  ngOnInit(): void {
    this.notificationService.startConnection();
    if (this.authService.isLoggedIn()) {
      this.userName = this.authService.getName();
    }
  }

  logout(): void {
    this.authService.logout().subscribe(() => {
      console.log('Logged out');
      this.router.navigate(['/login']);
    });
  }

  protected readonly UserRoles = UserRoles;

  getUserRoleString() {
    return this.authService.getUserRole() == UserRoles.Employee ? "employee" : "manager";
  }

  getBackgroundImage() {
    if (this.authService.getUserRole() === UserRoles.Employee) {
      return 'url(https://us.123rf.com/450wm/khongkitwiriyachan/khongkitwiriyachan1601/khongkitwiriyachan160100063/51430007-blurred-modern-office-interior-as-background-image.jpg?ver=6)';
    } else {
      return 'url(https://images.pond5.com/blur-background-modern-office-interior-photo-233506781_iconl_nowm.jpeg)';
    }
  }
}
