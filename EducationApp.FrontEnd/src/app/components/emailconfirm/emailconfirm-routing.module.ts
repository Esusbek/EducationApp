import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { EmailconfirmComponent } from './emailconfirm.component';

const routes: Routes = [
    { path: 'emailconfirm', component: EmailconfirmComponent }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
  })
  export class EmailconfirmRoutingModule { }