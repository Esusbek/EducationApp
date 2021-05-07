import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { OrderFailRoutingModule } from './order-fail-routing.module';
import { OrderFailComponent } from './order-fail.component';

@NgModule({
    imports: [
        CommonModule,
        OrderFailRoutingModule
    ],
    declarations: [OrderFailComponent]
})
export class OrderFailModule { }