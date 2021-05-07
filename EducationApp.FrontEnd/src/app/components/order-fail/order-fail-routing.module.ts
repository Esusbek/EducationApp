import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { OrderFailComponent } from './order-fail.component';

const routes: Routes = [
    {
        path: 'order-fail', component: OrderFailComponent
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class OrderFailRoutingModule { }