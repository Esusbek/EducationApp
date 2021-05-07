import { Action, createReducer, on } from '@ngrx/store';
import { LoginResult } from 'src/app/models/account.models';
import * as AccountActions from './account.actions';
import { AccountState, initialState } from './account.state';


const accountReducer = createReducer(
    initialState,
    on(AccountActions.refreshTokens, (state: AccountState, props: LoginResult) => {
        localStorage.setItem('accessToken', props.accessToken);
        localStorage.setItem('refreshToken', props.refreshToken);
        return { ...state };
    }),
    on(AccountActions.registerSuccess, (state: AccountState) => {
        return {
            ...state,
            isRegistered: true
        };
    })
);

export function AccountReducer(state: AccountState | undefined, action: Action) {
    return accountReducer(state, action);
}