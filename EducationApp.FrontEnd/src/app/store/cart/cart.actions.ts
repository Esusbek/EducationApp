import { HttpErrorResponse } from '@angular/common/http';
import { createAction, props } from '@ngrx/store';
import { OrderItemModel, OrderModel, SessionModel } from 'src/app/models/cart.models';

export const addToCart = createAction(
    '[Catalog Page] Add to cart',
    props<OrderItemModel>()
);
export const checkout = createAction(
    '[Cart] Checkout',
    props<{ items: Array<OrderItemModel> }>()
);

export const checkoutSuccess = createAction(
    '[Cart] Checkout success',
    props<SessionModel>()
);
export const checkoutFailure = createAction(
    '[Cart] Checkout',
    props<{ error: HttpErrorResponse }>()
);
export const getOrders = createAction(
    '[Order list page] Get Orders',
    props<{ page: number }>()
);
export const getOrdersSuccess = createAction(
    '[Order list page] Get Orders success',
    props<{ orders: Array<OrderModel>, lastPage: number }>()
);
export const getOrdersFailure = createAction(
    '[Order list page] Get Orders failure',
    props<{ error: HttpErrorResponse }>()
);
export const payOrder = createAction(
    '[Order success page] Pay order'
);
export const payOrderSuccess = createAction(
    '[Order list page] Get Orders success'
);
export const payOrderFailure = createAction(
    '[Order list page] Get Orders failure',
    props<{ error: HttpErrorResponse }>()
);