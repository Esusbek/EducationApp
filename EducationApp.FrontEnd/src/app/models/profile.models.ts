export class UserModel {
    id: string;
    userName: string;
    firstName: string;
    lastName: string;
    email: string;
}

export class ChangePasswordModel {
    user: {
        id: string;
    }
    currentPassword: string;
    newPassword: string;
}

export class TokenPayload {
    id: string;
    username: string;
    role: string;
}