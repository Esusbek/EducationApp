import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {ReactiveFormsModule} from '@angular/forms';

import { ForgotpasswordComponent } from './forgotpassword.component';
import {ForgotpasswordRoutingModule} from './forgotpassword-routing.module';

@NgModule({
  declarations: [
    ForgotpasswordComponent
  ],
  imports: [
    CommonModule,
    ForgotpasswordRoutingModule,
    ReactiveFormsModule
  ]
})
export class ForgotpasswordModule { }
