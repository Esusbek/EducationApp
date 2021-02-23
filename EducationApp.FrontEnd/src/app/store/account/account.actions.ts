import { HttpErrorResponse } from '@angular/common/http';
import {createAction, props} from '@ngrx/store';
import {LoginCredentials, LoginResult} from '../../models/account.models';

export const login = createAction(
    '[Login Page] login',
    props<{user: LoginCredentials, rememberMe: boolean}>()
);
export const loginSuccess = createAction(
    '[Login Page] loginSuccess',
    props<LoginResult>()
);
export const loginFailed = createAction(
    '[Login Page] loginFailure',
    props<{error: HttpErrorResponse}>()
);