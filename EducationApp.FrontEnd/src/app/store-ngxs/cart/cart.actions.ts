import { OrderItemModel, OrderModel, SessionModel } from 'src/app/models/cart.models';

export class addToCart {
    static readonly type = '[Catalog Page] Add to cart';
    constructor(public payload: OrderItemModel) { }
}
export class checkout {
    static readonly type = '[Cart] Checkout';
    constructor(public items: Array<OrderItemModel>) { }
}

export class checkoutSuccess {
    static readonly type = '[Cart] Checkout success';
    constructor(public payload: SessionModel) { }
}

export class checkoutExisting {
    static readonly type = '[Cart] Checkout exisisting order';
    constructor(public order: OrderModel) { }
}
export class getOrders {
    static readonly type = '[Order list page] Get Orders';
    constructor(public page: number) { }
}
export class getOrdersSuccess {
    static readonly type = '[Order list page] Get Orders success';
    constructor(public payload: { orders: Array<OrderModel>, lastPage: number }) { }
}
export class payOrder {
    static readonly type = '[Order success page] Pay order'
}
export class payOrderSuccess {
    static readonly type = '[Order success page] Pay Order success'
}