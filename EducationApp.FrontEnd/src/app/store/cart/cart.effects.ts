
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { Action } from '@ngrx/store';
import { Observable, of } from 'rxjs';
import { catchError, map, mergeMap, tap } from 'rxjs/operators';
import { CartService } from 'src/app/services/cart.service';
import * as CartActions from './cart.actions';

@Injectable()
export class CartEffects {

    constructor(private cartService: CartService, private action$: Actions, private router: Router) { }
    Checkout$: Observable<Action> = createEffect(() =>
        this.action$.pipe(
            ofType(CartActions.checkout),
            mergeMap(action => {
                return this.cartService.checkout(action.items)
                    .pipe(
                        map(response => CartActions.checkoutSuccess(response)),
                        catchError(error => of(CartActions.checkoutFailure({ error })))
                    )
            }
            )
        )
    );
    CheckoutSuccess$: Observable<Action> = createEffect(() =>
        this.action$.pipe(
            ofType(CartActions.checkoutSuccess),
            tap((action) => {
                this.cartService.createSession(action);
            })
        )
        , { dispatch: false });
    GetOrders$: Observable<Action> = createEffect(() =>
        this.action$.pipe(
            ofType(CartActions.getOrders),
            mergeMap(action => {
                return this.cartService.getOrders(action.page)
                    .pipe(
                        map(response => CartActions.getOrdersSuccess({ orders: response.orders, lastPage: response.lastPage })),
                        catchError(error => of(CartActions.getOrdersFailure({ error })))
                    )
            }
            )
        )
    );
    PayOrder$: Observable<Action> = createEffect(() =>
        this.action$.pipe(
            ofType(CartActions.payOrder),
            mergeMap(() => {
                return this.cartService.payOrder()
                    .pipe(
                        map(() => CartActions.payOrderSuccess()),
                        catchError(error => of(CartActions.payOrderFailure({ error })))
                    )
            }
            )
        ));
    PayOrderSuccess$: Observable<Action> = createEffect(() =>
        this.action$.pipe(
            ofType(CartActions.payOrderSuccess),
            tap(() => {
                localStorage.removeItem('intent');
            })
        )
        , { dispatch: false });
}

