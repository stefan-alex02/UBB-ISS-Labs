import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { UserRoles } from '../model/data/user-roles';
import { Location } from '@angular/common';
import {AuthService} from "../services/auth.service";

@Injectable({
  providedIn: 'root'
})
export class RoleGuard implements CanActivate {
  constructor(private authService: AuthService,
              private router: Router,
              private location: Location) { }

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    if (!this.authService.isLoggedIn()) {
      console.log('Role guard: User is not logged in');
      this.router.navigate(['/login'])
        .then(r => console.log('Redirected to login page:', r));
      return false;
    }

    const requiredRole = route.data['role'] as UserRoles;
    if (this.authService.getUserRole() === requiredRole) {
      console.log('Role guard: User has required role: ', requiredRole);
      return true;
    }
    // this.router.navigate(['/']);
    console.log('Role guard: User does not have required role: ', requiredRole);
    this.location.back();
    return false;
  }

}
