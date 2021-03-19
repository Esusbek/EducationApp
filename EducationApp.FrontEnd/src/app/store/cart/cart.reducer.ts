import { Action, createReducer, on } from '@ngrx/store';
import { OrderItemModel } from 'src/app/models/cart.models';
import * as CartActions from './cart.actions';
import { CartState, initialState } from './cart.state';


const cartReducer = createReducer(
    initialState,
    on(CartActions.addToCart, (state: CartState, props: OrderItemModel) => {
        if (state.currentItems.some(item => item.printingEditionId === props.printingEditionId)) {
            var currentItem = state.currentItems.find(item => item.printingEditionId === props.printingEditionId);
            var index = state.currentItems.findIndex(item => item.printingEditionId === props.printingEditionId);
            var newItem = new OrderItemModel;
            newItem = { ...props };
            newItem.amount = +currentItem.amount + +props.amount;
            newItem.subTotal = currentItem.subTotal + props.subTotal;
            var newArray = new Array<OrderItemModel>(...state.currentItems);
            newArray[index] = newItem;
            return { ...state, currentItems: newArray }
        }
        return { ...state, currentItems: [...state.currentItems, props] };
    }),
);

export function CartReducer(state: CartState | undefined, action: Action) {
    return cartReducer(state, action);
}