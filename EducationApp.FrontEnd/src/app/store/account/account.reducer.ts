import { Action, createReducer, on } from '@ngrx/store';
import * as AccountActions from './account.actions';
import{LoginResult} from '../../models/account.models';
import{initialState, AuthState} from './account.state';


const accountReducer = createReducer(
    initialState, 
    on(AccountActions.loginSuccess, (state: AuthState, props: LoginResult) => {
        localStorage.setItem('accessToken', props.accessToken);
        localStorage.setItem('refreshToken', props.refreshToken);
        return {...state}
    })
);

export function AccountReducer(state: AuthState | undefined, action: Action){
    return accountReducer(state, action);
}