import { createAction, props } from '@ngrx/store';
import { EmailActivationModel, LoginCredentials, LoginResult, RegisterModel, ResetPasswordModel } from 'src/app/models/account.models';

export const login = createAction(
    '[Login Page] login',
    props<{ user: LoginCredentials, rememberMe: boolean }>()
);
export const loginSuccess = createAction(
    '[Login Page] loginSuccess',
    props<LoginResult>()
);
export const refreshTokens = createAction(
    '[Any page] Refresh tokens',
    props<LoginResult>()
);
export const register = createAction(
    '[Register Page] register',
    props<{ user: RegisterModel }>()
);
export const registerSuccess = createAction(
    '[Register Page] registerSuccess'
);
export const activateEmail = createAction(
    '[Activation Page] Email activation',
    props<{ payload: EmailActivationModel }>()
);
export const forgotPassword = createAction(
    '[Password Reset Page] Forgot Password',
    props<{ userName: string }>()
);
export const resetPassword = createAction(
    '[Password Reset Page] Reset Password',
    props<{ payload: ResetPasswordModel }>()
);