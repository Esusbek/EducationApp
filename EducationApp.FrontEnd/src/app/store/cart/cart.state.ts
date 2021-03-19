import { OrderItemModel } from "src/app/models/cart.models";

export interface CartState {
    currentItems: Array<OrderItemModel>
}

export const initialState: CartState = {
    currentItems: []
}