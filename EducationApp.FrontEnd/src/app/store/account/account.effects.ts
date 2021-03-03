
import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { Action } from '@ngrx/store';
import { Observable, of } from 'rxjs';
import { catchError, map, mergeMap, tap } from 'rxjs/operators';
import * as AccountActions from './account.actions';
import { AccountService } from '../../services/account.service';
import { Router } from '@angular/router';

@Injectable()
export class AccountEffects {

    constructor(private accountService: AccountService, private action$: Actions, private router: Router) { }

    Login$: Observable<Action> = createEffect(() =>
        this.action$.pipe(
            ofType(AccountActions.login),
            mergeMap(action => {
                return this.accountService.login(action.user, action.rememberMe)
                    .pipe(
                        map(tokens => AccountActions.loginSuccess(tokens)),
                        catchError(error => of(AccountActions.loginFailed({ error })))
                    )
            }
            )
        )
    );
    Register$: Observable<Action> = createEffect(() =>
        this.action$.pipe(
            ofType(AccountActions.register),
            mergeMap(action => {
                return this.accountService.register(action.user)
                    .pipe(
                        map(() => AccountActions.registerSuccess()),
                        catchError(error => of(AccountActions.registerFailed({ error })))
                    )
            }
            )
        )
    );
    EmailActivate$: Observable<Action> = createEffect(() =>
        this.action$.pipe(
            ofType(AccountActions.activateEmail),
            mergeMap(action => {
                return this.accountService.activateEmail(action.payload)
                    .pipe(
                        map(() => AccountActions.registerSuccess()),
                        catchError(error => of(AccountActions.registerFailed({ error })))
                    )
            }
            )
        ));
    Forgotpassword$: Observable<Action> = createEffect(() =>
        this.action$.pipe(
            ofType(AccountActions.forgotPassword),
            mergeMap(action => {
                return this.accountService.forgotPassword(action.userName)
                    .pipe(
                        map(() => AccountActions.registerSuccess()),
                        catchError(error => of(AccountActions.registerFailed({ error })))
                    )
            })
        ));
    Resetpassword$: Observable<Action> = createEffect(() =>
        this.action$.pipe(
            ofType(AccountActions.resetPassword),
            mergeMap(action => {
                return this.accountService.resetPassword(action.payload)
                    .pipe(
                        map(() => AccountActions.registerSuccess()),
                        catchError(error => of(AccountActions.registerFailed({ error })))
                    )
            })
        ));
}

