import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { OrderModel } from 'src/app/models/cart.models';
import { PrintingEditionModel } from 'src/app/models/printing-edition.models';
import { OrderStatusType, PrintingEditionType } from 'src/app/shared/enums';
import { getOrders } from 'src/app/store/cart/cart.actions';
import { CartState } from 'src/app/store/cart/cart.state';
import { PrintingEditionState } from 'src/app/store/printing-edition/printing-edition.state';

@Component({
  selector: 'app-order-list',
  templateUrl: './order-list.component.html',
  styleUrls: ['./order-list.component.css']
})
export class OrderListComponent implements OnInit {
  public orders: Array<OrderModel>;
  public books: Array<PrintingEditionModel>
  public page: number;
  public lastPage: number;
  public PrintingEditionType;
  public OrderStatusType;
  constructor(private store: Store<{ Cart: CartState, Books: PrintingEditionState }>) {
    store.select('Cart').subscribe(value => {
      this.orders = value.orders,
        this.lastPage = value.lastPage
    });
    store.select('Books').subscribe(value => {
      this.books = value.books;
    });
    this.OrderStatusType = OrderStatusType;
    this.PrintingEditionType = PrintingEditionType;
  }

  ngOnInit(): void {
    this.page = 1;
    this.getOrders();
  }
  public getOrders(): void {
    this.store.dispatch(getOrders({ page: this.page }))
  }
  hasPreviousPage() {
    return +this.page > 1;
  }
  hasNextPage() {
    return +this.page < this.lastPage;
  }
  nextPage() {
    this.page = this.page + 1;
    this.getOrders();
  }
  previousPage() {
    this.page = this.page - 1;
    this.getOrders();
  }
}
