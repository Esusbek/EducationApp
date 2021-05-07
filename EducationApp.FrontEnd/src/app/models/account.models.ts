export class LoginResult {
    accessToken: string;
    refreshToken: string;
}

export class LoginCredentials {
    userName: string;
    password: string;
}

export class RegisterModel {
    userName: string;
    firstName: string;
    lastName: string;
    email: string;
    password: string;
    passwordConfirm: string;
}

export class EmailActivationModel {
    code: string;
    userId: string;
}
export class ResetPasswordModel {
    code: string;
    userId: string;
    password: string;
}