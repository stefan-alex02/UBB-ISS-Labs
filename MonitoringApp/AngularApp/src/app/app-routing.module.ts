import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './user-management/login/login.component';
import { ManagerDashboardComponent } from './manager-dashboard/manager-dashboard.component';
import { AttendPageComponent } from './attend-page/attend-page.component';
import { HomeComponent } from './home/home.component';
import { AuthGuard } from '../guards/auth.guard';
import { RoleGuard } from '../guards/role.guard';
import { UserRoles } from './data/user-roles';
import {EmployeeDashboardComponent} from "./employee-dashboard/employee-dashboard.component";
import {RegisterUserComponent} from "./user-management/register-user/register-user.component";

const routes: Routes = [
  { path: '', component: HomeComponent,
    children: [
      { path: 'manager-dashboard', component: ManagerDashboardComponent,
        canActivate: [AuthGuard, RoleGuard],
        data: { role: UserRoles.Manager } },
      { path: 'attend-page', component: AttendPageComponent,
        canActivate: [AuthGuard, RoleGuard],
        data: { role: UserRoles.Employee } },
      { path: 'employee-dashboard', component: EmployeeDashboardComponent,
        canActivate: [AuthGuard, RoleGuard],
        data: { role: UserRoles.Employee }
      }
    ]
  },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterUserComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
