import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { CookieService } from 'ngx-cookie-service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private cookieService: CookieService, private router: Router) { }

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    const user = this.cookieService.get('user');
    if (user) {
      // Update the 'user' cookie to expire in 10 minutes
      this.cookieService.set('user', user, 5 / 60 / 24); // The third parameter is the number of days until the cookie expires. 10/60 converts 10 minutes to a fraction of a day.
      return true;
    } else {
      this.router.navigate(['/login']);
      return false;
    }
  }

}
