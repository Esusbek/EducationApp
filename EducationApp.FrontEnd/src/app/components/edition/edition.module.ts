import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { EditionRoutingModule } from './edition-routing.module';
import { EditionComponent } from './edition.component';

@NgModule({
    imports: [
        CommonModule,
        EditionRoutingModule,
        ReactiveFormsModule,
        FormsModule
    ],
    declarations: [EditionComponent]
})
export class EditionModule { }