import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ResetPasswordComponent } from './resetpassword.component';
import { ResetPasswordRoutingModule } from './resetpassword-routing.module';
import { ReactiveFormsModule } from '@angular/forms';

@NgModule({
  imports: [
    CommonModule,
    ResetPasswordRoutingModule,
    ReactiveFormsModule
  ],
  declarations: [ResetPasswordComponent]
})
export class ResetPasswordModule { }
