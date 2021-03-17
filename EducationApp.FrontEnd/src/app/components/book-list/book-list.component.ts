import { Options } from "@angular-slider/ngx-slider";
import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Store } from '@ngrx/store';
import { Subscription } from "rxjs";
import { PrintingEditionInfoModel, PrintingEditionModel } from 'src/app/models/printing-edition.models';
import { Currency, PrintingEditionType } from '../../shared/enums';
import { getBooks, getEditionInfo, getFiltered, getLastPage } from '../../store/printing-edition/printing-edition.actions';
import { PrintingEditionState } from '../../store/printing-edition/printing-edition.state';

@Component({
  selector: 'app-book-list',
  templateUrl: './book-list.component.html',
  styleUrls: ['./book-list.component.css']
})
export class BookListComponent implements OnInit {
  private page$: string;
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
  options: Options;
  constructor(private route: ActivatedRoute, private store: Store<{ Books: PrintingEditionState }>, private formBuilder: FormBuilder) {
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

  }

  ngOnInit(): void {
    this.route.queryParamMap
      .subscribe((params) => {
        this.page$ = params.get('page') || "1";
      })

    const checkboxControl = (this.filterForm.controls.checkboxes as FormArray);
    this.subscription = checkboxControl.valueChanges.subscribe(checkbox => {
      checkboxControl.setValue(
        checkboxControl.value.map((value, i) => value ? this.checkboxes[i].value : false),
        { emitEvent: false }
      );
    });
    this.store.dispatch(getBooks({ page: this.page$ }))
    this.store.dispatch(getEditionInfo())
    this.updateInfo();
  }
  getImage(type: number): string {
    return `../../../assets/img/${PrintingEditionType[type]}.png`
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
    this.filterForm?.reset({ sliderControl: [this.info.minPrice, this.info.maxPrice], lowPrice: this.info.minPrice, highPrice: this.info.maxPrice })
  }
  show(id: number): void {
    this.currentBook$ = this.books$.find(book => book.id === id);
    this.showDetails$ = true;
    console.log(this.info);
    console.log(this.filterForm.value);

  }
  filter(): void {
    const checkboxControl = (this.filterForm.controls.checkboxes as FormArray);
    const formValue = {
      ...this.filterForm.value,
      title: this.filterForm.value.searchControl || "",
      type: checkboxControl.value.filter(value => !!value)
    }
    console.log(formValue);
    this.store.dispatch(getFiltered({ filter: formValue, page: this.page$ }));
    this.store.dispatch(getLastPage({ filter: formValue }));
  }
  updateSliderLow(event) {
    this.minValue = event.target.value;
  }
  updateSliderHigh(event) {
    this.maxValue = event.target.value;
  }
  hasPreviousPage() {
    console.log(+this.page$ > 1)
    return +this.page$ > 1;
  }
  hasNextPage() {
    return +this.page$ < this.info.lastPage;
  }
  ngOnDestroy() {
    this.subscription.unsubscribe();
  }
}
