import { createAction, props } from "@ngrx/store";
import { ChangePasswordModel, UserModel } from "src/app/models/profile.models";

export const getInfo = createAction(
    '[Profile Page] Load profile'
);
export const getInfoSuccess = createAction(
    '[Profile Page] Load profile success',
    props<UserModel>()
);
export const editProfile = createAction(
    '[Profile Page] edit profile',
    props<UserModel>()
);
export const editProfileSuccess = createAction(
    '[Profile Page] edit profile success',
    props<UserModel>()
);
export const changePassword = createAction(
    '[Profile Page] change password',
    props<ChangePasswordModel>()
);
export const changePasswordSuccess = createAction(
    '[Profile Page] change password success'
);

