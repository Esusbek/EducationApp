
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { act, Actions, createEffect, ofType } from '@ngrx/effects';
import { Action } from '@ngrx/store';
import { Observable, of, throwError } from 'rxjs';
import { catchError, exhaustMap, map, mergeMap } from 'rxjs/operators';
import * as LoginActions from '../login.store/login.actions';
import {AccountService} from '../../account.service';

@Injectable()
export class AuthEffects {

    constructor(private accountService: AccountService, private action$: Actions) { }

    Login$: Observable<Action> = createEffect(() =>
        this.action$.pipe(
            ofType(LoginActions.login),
            mergeMap(action => {
                console.log('effect', action.user);
                return this.accountService.login(action.user, action.rememberMe)
                .pipe(
                    map(tokens => LoginActions.loginSuccess(tokens)), 
                    catchError(error => of(LoginActions.loginFailed({error})))
                )}
            )
        )
    );
}

