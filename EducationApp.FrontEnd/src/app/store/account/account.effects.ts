
import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { Action } from '@ngrx/store';
import { Observable, of} from 'rxjs';
import { catchError, map, mergeMap } from 'rxjs/operators';
import * as AccountActions from './account.actions';
import {AccountService} from '../../services/account.service';

@Injectable()
export class AccountEffects {

    constructor(private accountService: AccountService, private action$: Actions) { }

    Login$: Observable<Action> = createEffect(() =>
        this.action$.pipe(
            ofType(AccountActions.login),
            mergeMap(action => {
                return this.accountService.login(action.user, action.rememberMe)
                .pipe(
                    map(tokens => AccountActions.loginSuccess(tokens)), 
                    catchError(error => of(AccountActions.loginFailed({error})))
                )}
            )
        )
    );
}

