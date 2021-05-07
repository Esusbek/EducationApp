import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { EmailactivatedComponent } from './emailactivated.component';

const routes: Routes = [
    { path: 'emailactivated', component: EmailactivatedComponent }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
  })
  export class EmailactivatedRoutingModule { }