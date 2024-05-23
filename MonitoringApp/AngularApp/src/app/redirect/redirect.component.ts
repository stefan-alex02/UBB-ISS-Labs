import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserRoles } from '../../model/data/user-roles';
import {NotificationService} from "../../services/notification.service";
import {AuthService} from "../../services/auth.service";

@Component({
  selector: 'app-home',
  templateUrl: './redirect.component.html',
  styleUrls: ['./redirect.component.css']
})
export class RedirectComponent implements OnInit {

  constructor(private authService: AuthService, private router: Router,
              public notificationService: NotificationService) { }

  ngOnInit(): void {
    if (this.authService.isLoggedIn()) {
      console.log('User is logged in, user role:', this.authService.getUserRole());
      if (this.authService.getUserRole() === UserRoles.Manager) {
        this.router.navigate(['/manager-dashboard'])
          .then(r => console.log('Redirected to manager dashboard page:', r));
      } else if (this.authService.getUserRole() === UserRoles.Employee) {
        this.router.navigate(['/attend-page'])
          .then(r => console.log('Redirected to attend page:', r));
      }
    } else {
      console.log('User is not logged in');
      this.router.navigate(['/login'])
        .then(r => console.log('Redirected to login page:', r));
    }
  }

}
