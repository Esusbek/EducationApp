import { Action, createReducer, on } from '@ngrx/store';
import * as LoginPageActions from '../login.store/login.actions';
import{LoginResult} from '../../account.models';

export interface State {
    accessToken: string;
    refreshToken: string;
}

export const initialState: State = {
    accessToken: '',
    refreshToken: ''
}

const loginReducer = createReducer(
    initialState, 
    on(LoginPageActions.loginSuccess, (state: State, props: LoginResult) => {
        console.log('reducer', props);
        return{accessToken: props.accessToken, refreshToken: props.refreshToken}
    })
);

export function LoginReducer(state: State | undefined, action: Action){
    return loginReducer(state, action);
}