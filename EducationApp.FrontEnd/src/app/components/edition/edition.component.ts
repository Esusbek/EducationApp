import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Store } from '@ngxs/store';
import { CartConfirmComponent } from 'src/app/components/cart-confirm/cart-confirm.component';
import { PrintingEditionModel } from 'src/app/models/printing-edition.models';
import { Defaults } from 'src/app/shared/consts';
import { Currency, PrintingEditionType } from 'src/app/shared/enums';
import { addToCart } from 'src/app/store-ngxs/cart/cart.actions';
import { PrintingEditionState } from 'src/app/store-ngxs/printing-edition/printing-edition.state';

@Component({
  selector: 'app-edition',
  templateUrl: './edition.component.html',
  styleUrls: ['./edition.component.css']
})
export class EditionComponent implements OnInit {
  public books: Array<PrintingEditionModel>;
  public orderForm = new FormGroup({
    order: new FormControl("false")
  })
  public quantity: Array<number>;
  public maxQuantity: number;
  public selectedQuantity: number;
  public currentBook: PrintingEditionModel;
  public id: number;
  public currency = Currency;

  public constructor(private store: Store, private route: ActivatedRoute, private modalService: NgbModal, private router: Router) {
    this.store.select(PrintingEditionState.books).subscribe(value => this.books = value);
    this.maxQuantity = Defaults.maxQuantity;
    this.selectedQuantity = Defaults.startQuantity;
    this.quantity = Array(this.maxQuantity).fill(Defaults.startQuantity).map((x, i) => i + 1);
    this.route.queryParamMap
      .subscribe((params) => {
        this.id = +params.get('id');
      })
    this.currentBook = this.books.find(book => book.id === this.id);
    if (!this.currentBook) {
      this.router.navigate(['/book-list']);
    }
  }
  public ngOnInit(): void {

  }
  public addToCart() {
    this.store.dispatch(new addToCart({
      amount: this.selectedQuantity, subTotal: this.currentBook.price * this.selectedQuantity,
      printingEditionId: this.currentBook.id, price: this.currentBook.price, currency: this.currentBook.currency,
      printingEdition: this.currentBook
    }))
    this.modalService.open(CartConfirmComponent);
  }
  public getImage(type: number): string {
    return `assets/img/${PrintingEditionType[type]}.png`
  }
}
