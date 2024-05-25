import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable, tap} from "rxjs";
import config from '../config.json';
import {jwtDecode} from "jwt-decode";
import moment from 'moment';
import {NotificationService} from "./notification.service";
import {UserRoles} from "../model/data/user-roles";

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private loginUrl = config.baseUrl +  '/api/users/login';
  private logoutUrl = config.baseUrl + '/api/users/logout';
  private registerUrl = config.baseUrl + '/api/users/register';

  constructor(private http: HttpClient,
              private notificationService: NotificationService) { }

  public login(username: string | null,
               password: string | null): Observable<any> {
    return this.http.post(this.loginUrl,
      { Username: username, Password: password},
      { withCredentials: true }).pipe(tap(
        (res: any) => {
          console.log('Received response: ', res);
          this.saveJwtToken(res.token);
    }));
  }

  public logout(): Observable<any> {
    console.log('Logging out');
    return this.http.post(this.logoutUrl, {}, { withCredentials: true }).pipe(
      tap((res: any) => {
        console.log('Received response: ', res);
        this.clearJwtToken();
        this.notificationService.stopConnection();
      })
    );
  }

  public register(username: string | null,
                  name: string | null,
                  password: string | null): Observable<any> {
    return this.http.post(this.registerUrl,
      { Username: username, Name: name, Password: password}).pipe(
      tap(() => {
        console.log('User registered');
      }
    ));
  }

  public saveJwtToken(token: string) {
    const decodedToken: any = jwtDecode(token);
    console.log('Decoded token: ', decodedToken);

    localStorage.setItem('token', token);
    localStorage.setItem('id', decodedToken.user_id);
    localStorage.setItem('username', decodedToken.username);
    localStorage.setItem('name', decodedToken.name);
    localStorage.setItem('user_role', decodedToken.user_role);
    localStorage.setItem('expires_at', decodedToken.exp);

    console.log('Token saved');
  }

  public clearJwtToken() {
    localStorage.removeItem('token');
    localStorage.removeItem('id');
    localStorage.removeItem('username');
    localStorage.removeItem('name');
    localStorage.removeItem('user_role');
    localStorage.removeItem('expires_at');

    localStorage.removeItem('has_attended');

    console.log('Token removed');
  }

  public getToken() {
    return localStorage.getItem('token');
  }

  public isLoggedIn(): boolean {
    if (this.getToken() === null) {
      return false;
    }
    if (this.isTokenExpired()) {
      this.clearJwtToken();
      return false;
    }
    return true;
  }

  isTokenExpired(): boolean {
    const expiration = localStorage.getItem("expires_at");
    const expiresAt = JSON.parse(expiration!);
    console.log('Expires at: ', moment.unix(expiresAt).format('YYYY-MM-DD HH:mm:ss'));
    return moment().isAfter(moment.unix(expiresAt));
  }

  public getUsername(): string {
    return localStorage.getItem('username') ?? '';
  }

    public getUserId(): number {
      return Number(localStorage.getItem('id') ?? -1);
    }

    public getUserRole(): UserRoles {
      return Number(localStorage.getItem('user_role'));
    }

    public getName(): string {
      return localStorage.getItem('name') ?? '';
    }

  public hasAttended(): boolean {
    return localStorage.getItem('has_attended') === "1";
  }

  public setHasAttended(hasAttended: boolean) {
    if (this.getUserRole() !== UserRoles.Employee) {
      return;
    }
    localStorage.setItem('has_attended', hasAttended ? "1" : "0");
  }
}
