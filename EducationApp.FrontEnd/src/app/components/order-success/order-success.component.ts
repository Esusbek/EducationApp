import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { payOrder } from 'src/app/store/cart/cart.actions';

@Component({
  selector: 'app-order-success',
  templateUrl: './order-success.component.html',
  styleUrls: ['./order-success.component.css']
})
export class OrderSuccessComponent implements OnInit {

  constructor(private store: Store) { }

  ngOnInit(): void {
    this.store.dispatch(payOrder());
  }

}
