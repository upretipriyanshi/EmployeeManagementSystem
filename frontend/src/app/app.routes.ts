import { Routes } from '@angular/router';

import { Login } from './pages/login/login';
import { Dashboard } from './pages/dashboard/dashboard';
import { Projects } from './pages/projects/projects';
import { Tasks } from './pages/tasks/tasks';
import { Employees } from './pages/employees/employees';
import { ProjectDetails } from './pages/project-details/project-details';
import { MainLayout } from './layout/main-layout/main-layout';
import { ReportsComponent } from './pages/reports/reports';
import { SetupAdmin } from './pages/setup-admin/setup-admin';

import { authGuard } from './guards/auth.guard';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full',
  },
  {
    path: 'login',
    component: Login,
  },
  {
    path: 'setup',
    component: SetupAdmin,
  },
  {
    path: '',
    component: MainLayout,
    canActivate: [authGuard],
    children: [
      {
        path: 'dashboard',
        component: Dashboard,
      },
      {
        path: 'projects',
        component: Projects,
      },
      {
        path: 'tasks',
        component: Tasks,
      },
      {
        path: 'employees',
        component: Employees,
      },
      {
        path: 'projects/:id',
        component: ProjectDetails,
      },
      {
        path: 'reports',
        component: ReportsComponent,
      },
    ],
  },
];
