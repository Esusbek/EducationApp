import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { OrderListRoutingModule } from './order-list-routing.module';
import { OrderListComponent } from './order-list.component';

@NgModule({
    imports: [
        CommonModule,
        OrderListRoutingModule
    ],
    declarations: [OrderListComponent]
})
export class OrderListModule { }