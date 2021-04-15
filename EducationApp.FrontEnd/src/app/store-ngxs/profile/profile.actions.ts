import { ChangePasswordModel, UserModel } from "src/app/models/profile.models";

export class getInfo {
    static readonly type = '[Profile Page] Load profile'
}
export class getInfoSuccess {
    static readonly type = '[Profile Page] Load profile success';
    constructor(public payload: UserModel) { }
}
export class editProfile {
    static readonly type = '[Profile Page] edit profile';
    constructor(public payload: UserModel) { }
}
export class editProfileSuccess {
    static readonly type = '[Profile Page] edit profile success';
    constructor(public payload: UserModel) { }
}
export class changePassword {
    static readonly type = '[Profile Page] change password';
    constructor(public payload: ChangePasswordModel) { }
}
export class changePasswordSuccess {
    static readonly type = '[Profile Page] change password success'
}
