import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { EmailactivatedComponent } from './emailactivated.component';
import { EmailactivatedRoutingModule } from './emailactivated-routing.module';

@NgModule({
    imports: [
        CommonModule,
        EmailactivatedRoutingModule,
    ],
    declarations: [EmailactivatedComponent]
})
export class EmailactivatedModule { }