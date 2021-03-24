import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { Currency } from 'src/app/shared/enums';
import { checkout } from 'src/app/store/cart/cart.actions';
import { CartState } from 'src/app/store/cart/cart.state';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.css']
})
export class CheckoutComponent implements OnInit {
  public cart: CartState;
  public Currency = Currency;
  public sum: number;
  constructor(private store: Store<{ Cart: CartState }>) {
    store.select('Cart').subscribe(value => {
      this.cart = value
    });
    this.sum = this.cart.currentItems.map(item => item.subTotal).reduce((sum, item) => sum + item)
  }
  ngOnInit(): void {
  }
  checkout(): void {
    this.store.dispatch(checkout({ items: this.cart.currentItems }));
  }
}
