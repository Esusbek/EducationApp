
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Actions } from '@ngrx/effects';
import { CartService } from 'src/app/services/cart.service';

@Injectable()
export class CartEffects {

    constructor(private cartService: CartService, private action$: Actions, private router: Router) { }
}

