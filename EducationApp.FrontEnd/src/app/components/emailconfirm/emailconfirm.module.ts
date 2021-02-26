import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { EmailconfirmComponent } from './emailconfirm.component';
import { EmailconfirmRoutingModule } from './emailconfirm-routing.module';

@NgModule({
    imports: [
        CommonModule,
        EmailconfirmRoutingModule,
    ],
    declarations: [EmailconfirmComponent]
})
export class EmailconfirmModule { }