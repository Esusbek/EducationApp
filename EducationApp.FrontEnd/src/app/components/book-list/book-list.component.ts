import { Options } from "@angular-slider/ngx-slider";
import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { Router } from "@angular/router";
import { Store } from '@ngxs/store';
import { Subscription } from "rxjs";
import { PrintingEditionInfoModel, PrintingEditionModel } from 'src/app/models/printing-edition.models';
import { Defaults } from "src/app/shared/consts";
import { Currency, PrintingEditionType } from 'src/app/shared/enums';
import { getBooks, getFiltered } from 'src/app/store-ngxs/printing-edition/printing-edition.actions';
import { PrintingEditionState } from "src/app/store-ngxs/printing-edition/printing-edition.state";

@Component({
  selector: 'app-book-list',
  templateUrl: './book-list.component.html',
  styleUrls: ['./book-list.component.css']
})
export class BookListComponent implements OnInit {
  public page: number;
  public books: Array<PrintingEditionModel>;
  public currency = Currency;
  private info$: PrintingEditionInfoModel;
  public get info(): PrintingEditionInfoModel {
    return this.info$;
  }
  public set info(value: PrintingEditionInfoModel) {
    if (value !== this.info$) {
      this.info$ = value;
      this.updateInfo();
    }
  }
  private subscription: Subscription;
  public floor: number;
  public checkboxes = [];
  public filterForm: FormGroup;
  public minValue: number;
  public maxValue: number;
  public options: Options;
  public orderForm = new FormGroup({
    order: new FormControl("false")
  })

  public constructor(private store: Store, private formBuilder: FormBuilder, private router: Router) {
    this.store.select(PrintingEditionState.books).subscribe(value => this.books = value);
    this.store.select(PrintingEditionState.info).subscribe(value => this.info = value);
    this.options = {
      floor: 0,
      ceil: 100,
      translate: (value: number): string => {
        return "";
      }
    };;
    this.checkboxes = [{
      name: PrintingEditionType[PrintingEditionType.Book],
      value: PrintingEditionType.Book,
    }, {
      name: PrintingEditionType[PrintingEditionType.Journal],
      value: PrintingEditionType.Journal
    }, {
      name: PrintingEditionType[PrintingEditionType.Newspaper],
      value: PrintingEditionType.Newspaper
    }];
    this.filterForm = this.formBuilder.group({
      checkboxes: this.formBuilder.array(this.checkboxes.map(() => false)),
      sliderControl: new FormControl([this.info.minPrice, this.info.maxPrice]),
      searchControl: new FormControl(''),
      lowPrice: new FormControl(this.info.minPrice),
      highPrice: new FormControl(this.info.maxPrice)
    })
  }

  public ngOnInit(): void {
    this.page = 1;
    const checkboxControl = (this.filterForm.controls.checkboxes as FormArray);
    this.subscription = checkboxControl.valueChanges.subscribe(() => {
      checkboxControl.setValue(
        checkboxControl.value.map((value, i) => value ? this.checkboxes[i].value : false),
        { emitEvent: false }
      );
    });
    this.store.dispatch(new getBooks(this.page))
    this.updateInfo();
  }
  public getImage(type: number): string {
    return `assets/img/${PrintingEditionType[type]}.png`
  }
  public updateInfo() {
    this.options = {
      floor: this.info.minPrice,
      ceil: this.info.maxPrice,
      step: 0.01,
      translate: (value: number): string => {
        return "";
      }
    };
    this.minValue = this.info.minPrice;
    this.maxValue = this.info.maxPrice;
  }
  public show(id: number): void {
    this.router.navigate(['/edition'], { queryParams: { id: String(id) } });
  }
  public filter(): void {
    const checkboxControl = (this.filterForm.controls.checkboxes as FormArray);
    const formValue = {
      ...this.filterForm.value,
      title: this.filterForm.value.searchControl || "",
      type: checkboxControl.value.filter(value => !!value)
    }
    this.store.dispatch(new getFiltered({ filter: formValue, orderAsc: this.orderForm.value.order, page: this.page }));
  }
  public updateSliderLow(event) {
    this.minValue = event.target.value;
  }
  public updateSliderHigh(event) {
    this.maxValue = event.target.value;
  }
  public hasPreviousPage() {
    return this.page > Defaults.defaultPage;
  }
  public hasNextPage() {
    return this.page < this.info.lastPage;
  }
  public nextPage() {
    this.page = this.page + Defaults.pageOffset;
    this.filter();
  }
  public previousPage() {
    this.page = this.page - Defaults.pageOffset;
    this.filter();
  }
  public resetFilter() {
    this.ngOnInit();
  }
  public ngOnDestroy() {
    this.subscription.unsubscribe();
  }
}
