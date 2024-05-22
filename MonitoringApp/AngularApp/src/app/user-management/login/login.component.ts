import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import {AuthService} from "../../../services/auth.service";
import {NotificationService} from "../../../services/notification.service";

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
    this.authService.login(this.username, this.password).subscribe(
      (res: any) => {
        this.router.navigate(['/']);
      },
      (error: any) => {
        this.errorMessage = error.error.message;
      }
    );
  }
}
