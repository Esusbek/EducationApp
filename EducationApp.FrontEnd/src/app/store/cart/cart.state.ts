import { OrderItemModel, OrderModel } from "src/app/models/cart.models";

export interface CartState {
    currentItems: Array<OrderItemModel>;
    orders: Array<OrderModel>;
    lastPage: number;
}

export const initialState: CartState = {
    currentItems: [],
    orders: [],
    lastPage: 1
}