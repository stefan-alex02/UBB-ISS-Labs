import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './user-management/login/login.component';
import { ManagerDashboardComponent } from './manager-dashboard/manager-dashboard.component';
import { AttendPageComponent } from './attend-page/attend-page.component';
import { RedirectComponent } from './redirect/redirect.component';
import { RoleGuard } from '../guards/role.guard';
import { UserRoles } from '../model/data/user-roles';
import {EmployeeDashboardComponent} from "./employee-dashboard/employee-dashboard.component";
import {RegisterUserComponent} from "./user-management/register-user/register-user.component";
import {UserWrapperComponent} from "./user-wrapper/user-wrapper.component";
import {ManageTasksComponent} from "./manage-tasks/manage-tasks.component";

const routes: Routes = [
  { path: '', component: UserWrapperComponent,
    children: [
      { path: '', component: RedirectComponent},
      { path: 'manager-dashboard', component: ManagerDashboardComponent,
        canActivate: [RoleGuard],
        data: { role: UserRoles.Manager } },
      { path: 'manage-tasks/:username', component: ManageTasksComponent,
        canActivate: [RoleGuard],
        data: { role: UserRoles.Manager } },
      { path: 'attend-page', component: AttendPageComponent,
        canActivate: [RoleGuard],
        data: { role: UserRoles.Employee } },
      { path: 'employee-dashboard', component: EmployeeDashboardComponent,
        canActivate: [RoleGuard],
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
