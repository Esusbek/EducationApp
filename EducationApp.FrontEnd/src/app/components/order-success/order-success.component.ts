import { Component, OnInit } from '@angular/core';
import { Store } from '@ngxs/store';
import { payOrder } from 'src/app/store-ngxs/cart/cart.actions';

@Component({
  selector: 'app-order-success',
  templateUrl: './order-success.component.html',
  styleUrls: ['./order-success.component.css']
})
export class OrderSuccessComponent implements OnInit {

  public constructor(private store: Store) { }

  public ngOnInit(): void {
    this.store.dispatch(new payOrder());
  }

}
