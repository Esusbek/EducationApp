import { NgxSliderModule } from '@angular-slider/ngx-slider';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { BookListRoutingModule } from './book-list-routing.module';
import { BookListComponent } from './book-list.component';

@NgModule({
    imports: [
        CommonModule,
        BookListRoutingModule,
        ReactiveFormsModule,
        NgxSliderModule,
    ],
    declarations: [BookListComponent]
})
export class BookListModule { }