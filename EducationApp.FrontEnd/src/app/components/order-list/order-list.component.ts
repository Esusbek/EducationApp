import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { OrderModel } from 'src/app/models/cart.models';
import { PrintingEditionModel } from 'src/app/models/printing-edition.models';
import { Defaults } from 'src/app/shared/consts';
import { OrderStatusType, PrintingEditionType } from 'src/app/shared/enums';
import { checkoutExisting, getOrders } from 'src/app/store/cart/cart.actions';
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
  public constructor(private store: Store<{ Cart: CartState, Books: PrintingEditionState }>) {
    store.select('Cart').subscribe(value => {
      this.orders = value.orders,
        this.lastPage = value.lastPage
    });
    this.OrderStatusType = OrderStatusType;
    this.PrintingEditionType = PrintingEditionType;
  }

  public ngOnInit(): void {
    this.page = 1;
    this.getOrders();
  }
  public getOrders(): void {
    this.store.dispatch(getOrders({ page: this.page }))
  }
  public payOrder(order: OrderModel) {
    this.store.dispatch(checkoutExisting({ order }));
  }
  public hasPreviousPage() {
    return this.page > Defaults.defaultPage;
  }
  public hasNextPage() {
    return this.page < this.lastPage;
  }
  public nextPage() {
    this.page = this.page + Defaults.pageOffset;
    this.getOrders();
  }
  public previousPage() {
    this.page = this.page - Defaults.pageOffset;
    this.getOrders();
  }
}
