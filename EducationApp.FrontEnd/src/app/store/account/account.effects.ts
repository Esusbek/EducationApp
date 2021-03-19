
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { Action } from '@ngrx/store';
import { Observable, of } from 'rxjs';
import { catchError, map, mergeMap, tap } from 'rxjs/operators';
import { AccountService } from 'src/app/services/account.service';
import * as AccountActions from './account.actions';

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
    LoginSuccess$: Observable<Action> = createEffect(() =>
        this.action$.pipe(
            ofType(AccountActions.loginSuccess),
            tap((action) => {
                localStorage.setItem('accessToken', action.accessToken);
                localStorage.setItem('refreshToken', action.refreshToken);
                this.router.navigate(['/book-list'])
            })
        )
        , { dispatch: false });
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

