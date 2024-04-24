import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { CookieService } from 'ngx-cookie-service';
import { UserRoles } from '../data/user-roles';
import { Location } from '@angular/common';

@Injectable({
  providedIn: 'root'
})
export class RoleGuard implements CanActivate {

  constructor(private cookieService: CookieService,
              private router: Router,
              private location: Location) { }

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    const user = this.cookieService.get('user');
    if (user) {
      const userRole = JSON.parse(user).userRole;
      const requiredRole = route.data['role'] as UserRoles;
      if (userRole === requiredRole) {
        return true;
      }
    }
    // this.router.navigate(['/']);
    this.location.back();
    return false;
  }

}
