import { Component, OnInit } from '@angular/core';
import { Store } from '@ngxs/store';
import { Currency } from 'src/app/shared/enums';
import { checkout } from 'src/app/store-ngxs/cart/cart.actions';
import { CartState, CartStateModel } from 'src/app/store-ngxs/cart/cart.state';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.css']
})
export class CheckoutComponent implements OnInit {
  public cart: CartStateModel;
  public currency = Currency;
  public sum: number;
  public constructor(private store: Store) {
    this.store.select(CartState.cart).subscribe(value => this.cart = value);
    this.sum = this.cart.currentItems.map(item => item.subTotal).reduce((sum, item) => sum + item)
  }
  public ngOnInit(): void {
  }
  public checkout(): void {
    this.store.dispatch(new checkout(this.cart.currentItems));
  }
}
