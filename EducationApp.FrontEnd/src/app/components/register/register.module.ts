import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RegisterComponent } from './register.component';
import {RegisterRoutingModule} from './register-routing.module';
import { AuthGuard } from 'src/app/guards/auth-guard.guard';
import { ReactiveFormsModule } from '@angular/forms';



@NgModule({
  declarations: [RegisterComponent],
  imports: [
    CommonModule,
    RegisterRoutingModule,
    ReactiveFormsModule
  ],
  providers: [AuthGuard]
})
export class RegisterModule { }
