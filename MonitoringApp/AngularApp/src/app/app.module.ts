import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {HTTP_INTERCEPTORS, HttpClientModule} from '@angular/common/http';
import { CookieService } from 'ngx-cookie-service';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './user-management/login/login.component';
import { ManagerDashboardComponent } from './manager-dashboard/manager-dashboard.component';
import { AttendPageComponent } from './attend-page/attend-page.component';
import { RedirectComponent } from './redirect/redirect.component';
import { EmployeeDashboardComponent } from './employee-dashboard/employee-dashboard.component';
import { RegisterUserComponent } from './user-management/register-user/register-user.component';
import {AuthInterceptor} from "../guards/auth.interceptor";
import { UserWrapperComponent } from './user-wrapper/user-wrapper.component';
import { ManageTasksComponent } from './manage-tasks/manage-tasks.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    ManagerDashboardComponent,
    AttendPageComponent,
    RedirectComponent,
    EmployeeDashboardComponent,
    RegisterUserComponent,
    UserWrapperComponent,
    ManageTasksComponent
  ],
    imports: [
        BrowserModule,
        AppRoutingModule,
        FormsModule,
        HttpClientModule,
        ReactiveFormsModule
    ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
