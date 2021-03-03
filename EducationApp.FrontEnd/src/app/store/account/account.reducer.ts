import { Action, createReducer, on } from '@ngrx/store';
import * as AccountActions from './account.actions';
import{LoginResult} from '../../models/account.models';
import{initialState, AccountState} from './account.state';


const accountReducer = createReducer(
    initialState, 
    on(AccountActions.loginSuccess, (state: AccountState, props: LoginResult) => {
        localStorage.setItem('accessToken', props.accessToken);
        localStorage.setItem('refreshToken', props.refreshToken);
        return {...state};
    }),
    on(AccountActions.registerSuccess, (state: AccountState)=> {
        return {...state,
                isRegistered: true};
    })
);

export function AccountReducer(state: AccountState | undefined, action: Action){
    return accountReducer(state, action);
}