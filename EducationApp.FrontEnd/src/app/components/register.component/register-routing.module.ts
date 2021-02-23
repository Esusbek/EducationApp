import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { RegisterComponent } from './register.component';
import {AuthGuard} from '../../guards/auth-guard.guard';

const routes: Routes = [
    { path: 'register', component: RegisterComponent, canActivate: [AuthGuard]  }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
  })
  export class RegisterRoutingModule { }