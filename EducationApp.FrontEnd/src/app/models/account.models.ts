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
    firstname: string;
    lastname: string;
    email: string;
    password: string;
    passwordConfirm: string;
}