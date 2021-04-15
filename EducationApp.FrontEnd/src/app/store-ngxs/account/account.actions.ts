import { EmailActivationModel, LoginCredentials, LoginResult, RegisterModel, ResetPasswordModel } from 'src/app/models/account.models';

export class login {
    static readonly type = '[Login Page] login';
    constructor(public payload: { user: LoginCredentials, rememberMe: boolean }) { }
}
export class loginSuccess {
    static readonly type = '[Login Page] loginSuccess';
    constructor(public payload: { tokens: LoginResult, rememberMe: boolean }) { }
}
export class refreshTokens {
    static readonly type = '[Any page] Refresh tokens';
    constructor(public payload: LoginResult) { }
}
export class register {
    static readonly type = '[Register Page] register';
    constructor(public payload: RegisterModel) { }
}
export class registerSuccess {
    static readonly type = '[Register Page] registerSuccess';
}
export class activateEmail {
    static readonly type = '[Activation Page] Email activation';
    constructor(public payload: EmailActivationModel) { }
}
export class forgotPassword {
    static readonly type = '[Password Reset Page] Forgot Password';
    constructor(public userName: string) { }
}
export class resetPassword {
    static readonly type = '[Password Reset Page] Reset Password';
    constructor(public payload: ResetPasswordModel) { }
}