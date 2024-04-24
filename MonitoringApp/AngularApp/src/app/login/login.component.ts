import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import {AuthService} from "../services/auth.service";
import {UserRoles} from "../data/user-roles";
import {NotificationService} from "../services/notification.service";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  username = '';
  password = '';
  errorMessage: string | undefined = undefined;

  constructor(
    private router: Router,
    private cookieService: CookieService,
    private authService: AuthService,
    private notificationService: NotificationService
  ) {
  }

  login() {
    this.authService.login(this.username, this.password)
      .subscribe(
        (user: any) => {
          this.cookieService.set('user', JSON.stringify(user));
          console.log(user);
          if (user.userRole === UserRoles.Manager) {
            this.notificationService.startConnection();
            this.router.navigate(['/manager-dashboard']);
          } else if (user.userRole === UserRoles.Employee) {
            console.log('Employee');
            this.router.navigate(['/attend-page']);
          }
        },
        (error: any) => {
          if (error.status === 450) {
            this.errorMessage = error.error;
          } else if (error.status === 400) {
            this.errorMessage = 'Unknown error';
          }
        }
      );
  }
}
