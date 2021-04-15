import { Injectable } from '@angular/core';
import { Action, Selector, State, StateContext } from '@ngxs/store';
import { OrderItemModel, OrderModel } from 'src/app/models/cart.models';
import { CartService } from 'src/app/services/cart.service';
import { addToCart, checkout, checkoutExisting, checkoutSuccess, getOrders, getOrdersSuccess, payOrder, payOrderSuccess } from './cart.actions';

export interface CartStateModel {
    currentItems: Array<OrderItemModel>;
    orders: Array<OrderModel>;
    lastPage: number;
}

const initialState: CartStateModel = {
    currentItems: [],
    orders: [],
    lastPage: 1
}
@State<CartStateModel>({
    name: 'cart',
    defaults: initialState
})
@Injectable()
export class CartState {
    constructor(private cartService: CartService) { }
    @Selector()
    static cart(state: CartStateModel) {
        return state;
    }
    @Selector()
    static currentItems(state: CartStateModel) {
        return state.currentItems;
    }
    @Selector()
    static orders(state: CartStateModel) {
        return state.orders;
    }
    @Selector()
    static lastPage(state: CartStateModel) {
        return state.lastPage;
    }
    @Action(addToCart)
    addToCart(ctx: StateContext<CartStateModel>, action: addToCart) {
        const state = ctx.getState();
        if (state.currentItems.some(item => item.printingEditionId === action.payload.printingEditionId)) {
            var currentItem = state.currentItems.find(item => item.printingEditionId === action.payload.printingEditionId);
            var index = state.currentItems.findIndex(item => item.printingEditionId === action.payload.printingEditionId);
            var newItem = new OrderItemModel;
            newItem = { ...action.payload };
            newItem.amount = +currentItem.amount + +action.payload.amount;
            newItem.subTotal = currentItem.subTotal + action.payload.subTotal;
            var newArray = new Array<OrderItemModel>(...state.currentItems);
            newArray[index] = newItem;
            ctx.setState({ ...state, currentItems: newArray });
        }
        ctx.setState({ ...state, currentItems: [...state.currentItems, action.payload] });
    }
    @Action(checkout)
    checkout(ctx: StateContext<CartStateModel>, action: checkout) {
        this.cartService.checkout(action.items)
            .subscribe(response => ctx.dispatch(new checkoutSuccess(response)))
    }
    @Action(checkoutSuccess)
    checkoutSuccess(ctx: StateContext<CartStateModel>, action: checkoutSuccess) {
        this.cartService.createSession(action.payload);
    }
    @Action(checkoutExisting)
    checkoutExisting(ctx: StateContext<CartStateModel>, action: checkoutExisting) {
        this.cartService.checkoutExisiting(action.order)
            .subscribe(response => ctx.dispatch(new checkoutSuccess(response)))
    }
    @Action(getOrders)
    getOrders(ctx: StateContext<CartStateModel>, action: getOrders) {
        this.cartService.getOrders(action.page)
            .subscribe(response => ctx.dispatch(new getOrdersSuccess({ orders: response.orders, lastPage: response.lastPage })))
    }
    @Action(getOrdersSuccess)
    getOrdersSuccess(ctx: StateContext<CartStateModel>, action: getOrdersSuccess) {
        const state = ctx.getState();
        ctx.setState({ ...state, orders: action.payload.orders, lastPage: action.payload.lastPage });
    }
    @Action(payOrder)
    payOrder(ctx: StateContext<CartStateModel>, action: payOrder) {
        this.cartService.payOrder()
            .subscribe(() => ctx.dispatch(new payOrderSuccess()))
    }
    @Action(payOrderSuccess)
    payOrderSuccess(ctx: StateContext<CartStateModel>, action: payOrderSuccess) {
        localStorage.removeItem('intent');
    }

}