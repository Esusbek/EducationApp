import { createAction, props } from '@ngrx/store';
import { OrderItemModel } from 'src/app/models/cart.models';

export const addToCart = createAction(
    '[Catalog Page] Add to cart',
    props<OrderItemModel>()
);