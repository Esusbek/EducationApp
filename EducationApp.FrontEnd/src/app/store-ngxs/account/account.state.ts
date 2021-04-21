import { Injectable, NgZone } from '@angular/core';
import { Router } from '@angular/router';
import { Action, Selector, State, StateContext } from '@ngxs/store';
import { AccountService } from 'src/app/services/account.service';
import { activateEmail, forgotPassword, googleLogin, login, loginSuccess, refreshTokens, register, registerSuccess, resetPassword } from './account.actions';


export interface AccountStateModel {
    isRegistered: boolean | undefined;
}

const initialState: AccountStateModel = {
    isRegistered: false
}

@State<AccountStateModel>({
    name: 'account',
    defaults: initialState
})
@Injectable()
export class AccountState {
    constructor(private accountService: AccountService, private router: Router, private ngZone: NgZone) { }
    @Selector()
    static isRegistered(state: AccountStateModel) {
        return state.isRegistered;
    }
    @Action(login)
    login(ctx: StateContext<AccountStateModel>, action: login) {
        this.accountService.login(action.payload.user, action.payload.rememberMe)
            .subscribe(tokens => ctx.dispatch(new loginSuccess({ tokens, rememberMe: action.payload.rememberMe })))

    }
    @Action(loginSuccess)
    loginSuccess(ctx: StateContext<AccountStateModel>, action: loginSuccess) {
        localStorage.setItem('accessToken', action.payload.tokens.accessToken);
        if (action.payload.rememberMe) {
            localStorage.setItem('refreshToken', action.payload.tokens.refreshToken);
        }
        this.ngZone.run(() => this.router.navigate(['/book-list']))
    }
    @Action(register)
    register(ctx: StateContext<AccountStateModel>, action: register) {
        this.accountService.register(action.payload)
            .subscribe(() => ctx.dispatch(new registerSuccess()));
    }
    @Action(registerSuccess)
    registerSuccess(ctx: StateContext<AccountStateModel>, action: registerSuccess) {
        ctx.patchState({ isRegistered: true });
    }
    @Action(activateEmail)
    activateEmail(ctx: StateContext<AccountStateModel>, action: activateEmail) {
        this.accountService.activateEmail(action.payload)
            .subscribe(() => ctx.dispatch(new registerSuccess()))
    }
    @Action(forgotPassword)
    forgotPassword(ctx: StateContext<AccountStateModel>, action: forgotPassword) {
        this.accountService.forgotPassword(action.userName)
            .subscribe(() => ctx.dispatch(new registerSuccess()))
    }
    @Action(resetPassword)
    resetPassword(ctx: StateContext<AccountStateModel>, action: resetPassword) {
        this.accountService.resetPassword(action.payload)
            .subscribe(() => ctx.dispatch(new registerSuccess()))
    }
    @Action(refreshTokens)
    refreshTokens(ctx: StateContext<AccountStateModel>, action: refreshTokens) {
        localStorage.setItem('accessToken', action.payload.accessToken);
        localStorage.setItem('refreshToken', action.payload.refreshToken);
    }
    @Action(googleLogin)
    googleLogin(ctx: StateContext<AccountStateModel>, action: googleLogin) {
        this.accountService.googleLogin(action.user).subscribe(tokens => ctx.dispatch(new loginSuccess({ tokens, rememberMe: true })))
    }
}