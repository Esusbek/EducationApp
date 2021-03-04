import { HttpErrorResponse } from "@angular/common/http";
import { createAction, props } from "@ngrx/store";
import { ChangePasswordModel, UserModel } from "src/app/models/profile.models";

export const getInfo = createAction(
    '[Profile Page] Load profile'
);

export const getInfoSuccess = createAction(
    '[Profile Page] Load profile success',
    props<UserModel>()
);

export const getInfoFailure = createAction(
    '[Profile Page] Load profile failure',
    props<{error: HttpErrorResponse}>()
);

export const editProfile = createAction(
    '[Profile Page] edit profile', 
    props<UserModel>()
);
export const editProfileSuccess = createAction(
    '[Profile Page] edit profile success',
    props<UserModel>()
);
export const editProfileFailure = createAction(
    '[Profile Page] edit profile failure',
    props<{error: HttpErrorResponse}>()
);
export const changePassword = createAction(
    '[Profile Page] change password', 
    props<ChangePasswordModel>()
);

export const changePasswordSuccess = createAction(
    '[Profile Page] change password success'
);
export const changePasswordFailure = createAction(
    '[Profile Page] change password failure', 
    props<{error: HttpErrorResponse}>()
);

