import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { UserRoles } from '../data/user-roles';
import {NotificationService} from "../services/notification.service";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  constructor(private cookieService: CookieService, private router: Router,
              public notificationService: NotificationService) { }



  ngOnInit(): void {
    const user = this.cookieService.get('user');
    if (user) {
      const userRole = JSON.parse(user).userRole;
      if (userRole === UserRoles.Manager) {
        this.router.navigate(['/manager-dashboard'])
          .then(r => console.log(r));
      } else if (userRole === UserRoles.Employee) {
        this.router.navigate(['/attend-page'])
          .then(r => console.log(r));
      }
    } else {
      this.router.navigate(['/login'])
        .then(r => console.log(r));
    }
  }

}
