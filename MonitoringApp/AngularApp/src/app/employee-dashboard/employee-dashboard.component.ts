import { Component } from '@angular/core';
import {Router} from "@angular/router";
import {CookieService} from "ngx-cookie-service";

@Component({
  selector: 'app-employee-dashboard',
  templateUrl: './employee-dashboard.component.html',
  styleUrl: './employee-dashboard.component.css'
})
export class EmployeeDashboardComponent {
  constructor(private cookieService: CookieService, private router: Router) { }

  logout(): void {
    this.cookieService.delete('user');
    this.router.navigate(['/login']);
  }
}
