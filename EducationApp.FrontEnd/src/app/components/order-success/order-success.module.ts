import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { OrderSuccessRoutingModule } from './order-success-routing.module';
import { OrderSuccessComponent } from './order-success.component';

@NgModule({
    imports: [
        CommonModule,
        OrderSuccessRoutingModule
    ],
    declarations: [OrderSuccessComponent]
})
export class OrderSuccessModule { }