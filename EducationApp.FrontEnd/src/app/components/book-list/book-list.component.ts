import { Options } from "@angular-slider/ngx-slider";
import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { Store } from '@ngrx/store';
import { Subscription } from "rxjs";
import { PrintingEditionInfoModel, PrintingEditionModel } from 'src/app/models/printing-edition.models';
import { Currency, PrintingEditionType } from 'src/app/shared/enums';
import { addToCart } from "src/app/store/cart/cart.actions";
import { getBooks, getFiltered } from 'src/app/store/printing-edition/printing-edition.actions';
import { PrintingEditionState } from "src/app/store/printing-edition/printing-edition.state";

@Component({
  selector: 'app-book-list',
  templateUrl: './book-list.component.html',
  styleUrls: ['./book-list.component.css']
})
export class BookListComponent implements OnInit {
  private page$: number;
  public books$: Array<PrintingEditionModel>;
  public currentBook$: PrintingEditionModel;
  public showDetails$: boolean;
  public Currency = Currency;
  private info$: PrintingEditionInfoModel;
  get info(): PrintingEditionInfoModel {
    return this.info$;
  }
  set info(value: PrintingEditionInfoModel) {
    if (value !== this.info$) {
      this.info$ = value;
      this.updateInfo();
    }
  }
  private subscription: Subscription;
  public floor$: number;
  private checkboxes = [];
  public filterForm: FormGroup;
  public minValue: number;
  public maxValue: number;
  public options: Options;
  public orderForm = new FormGroup({
    order: new FormControl("false")
  })
  public quantity: Array<number>;
  public maxQuantity: number;
  public selectedQuantity: number;


  constructor(private store: Store<{ Books: PrintingEditionState }>, private formBuilder: FormBuilder) {
    store.select('Books').subscribe(value => {
      this.books$ = value.books;
      this.info = value.info;
    });
    this.showDetails$ = false;
    this.options = {
      floor: 0,
      ceil: 100,
      translate: (value: number): string => {
        return "$" + value;
      }
    };;
    this.checkboxes = [{
      name: PrintingEditionType[1],
      value: 1,
    }, {
      name: PrintingEditionType[2],
      value: 2
    }, {
      name: PrintingEditionType[3],
      value: 3
    }];
    this.filterForm = this.formBuilder.group({
      checkboxes: this.formBuilder.array(this.checkboxes.map(() => false)),
      sliderControl: new FormControl([this.info.minPrice, this.info.maxPrice]),
      searchControl: new FormControl(''),
      lowPrice: new FormControl(this.info.minPrice),
      highPrice: new FormControl(this.info.maxPrice)
    })

    this.maxQuantity = 10;
    this.selectedQuantity = 1;
    this.quantity = Array(this.maxQuantity).fill(1).map((x, i) => i + 1);
  }

  ngOnInit(): void {
    this.page$ = 1;
    const checkboxControl = (this.filterForm.controls.checkboxes as FormArray);
    this.subscription = checkboxControl.valueChanges.subscribe(() => {
      checkboxControl.setValue(
        checkboxControl.value.map((value, i) => value ? this.checkboxes[i].value : false),
        { emitEvent: false }
      );
    });
    this.store.dispatch(getBooks({ page: this.page$ }))
    this.updateInfo();
  }
  getImage(type: number): string {
    return `assets/img/${PrintingEditionType[type]}.png`
  }
  updateInfo() {
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
  show(id: number): void {
    this.currentBook$ = this.books$.find(book => book.id === id);
    this.showDetails$ = true;

  }
  filter(): void {
    const checkboxControl = (this.filterForm.controls.checkboxes as FormArray);
    const formValue = {
      ...this.filterForm.value,
      title: this.filterForm.value.searchControl || "",
      type: checkboxControl.value.filter(value => !!value)
    }
    this.store.dispatch(getFiltered({ filter: formValue, orderAsc: this.orderForm.value.order, page: this.page$ }));
  }
  updateSliderLow(event) {
    this.minValue = event.target.value;
  }
  updateSliderHigh(event) {
    this.maxValue = event.target.value;
  }
  hasPreviousPage() {
    return +this.page$ > 1;
  }
  hasNextPage() {
    return +this.page$ < this.info.lastPage;
  }
  nextPage() {
    this.page$ = this.page$ + 1;
    this.filter();
  }
  previousPage() {
    this.page$ = this.page$ - 1;
    this.filter();
  }
  resetFilter() {
    this.ngOnInit();
  }
  addToCart() {
    this.store.dispatch(addToCart({
      amount: this.selectedQuantity, subTotal: this.currentBook$.price * this.selectedQuantity,
      printingEditionId: this.currentBook$.id, price: this.currentBook$.price, currency: this.currentBook$.currency
    }))
  }
  ngOnDestroy() {
    this.subscription.unsubscribe();
  }
}
