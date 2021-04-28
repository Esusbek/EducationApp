import { Injectable } from '@angular/core';
import { Action, Selector, State, StateContext } from '@ngxs/store';
import { UserModel } from 'src/app/models/profile.models';
import { ProfileService } from 'src/app/services/profile.service';
import { changePassword, changePasswordSuccess, editProfile, editProfileSuccess, getInfo, getInfoSuccess } from './profile.actions';

export interface ProfileStateModel {
    user: UserModel;
}

const initialState: ProfileStateModel = {
    user: {
        id: "",
        userName: "",
        firstName: "",
        lastName: "",
        email: "",
        profilePictureURL: "",
    }
}
getInfo
getInfoSuccess
editProfile
editProfileSuccess
changePassword
changePasswordSuccess
@State<ProfileStateModel>({
    name: 'profile',
    defaults: initialState
})
@Injectable()
export class ProfileState {
    constructor(private profileService: ProfileService) { }
    @Selector()
    static user(state: ProfileStateModel) {
        return state.user;
    }
    @Action(getInfo)
    getInfo(ctx: StateContext<ProfileStateModel>, action: getInfo) {
        this.profileService.getInfo()
            .subscribe(data => ctx.dispatch(new getInfoSuccess(data)));
    }
    @Action(getInfoSuccess)
    getInfoSuccess(ctx: StateContext<ProfileStateModel>, action: getInfoSuccess) {
        const state = ctx.getState();
        ctx.setState({ ...state, user: { ...action.payload } });
    }
    @Action(editProfile)
    editProfile(ctx: StateContext<ProfileStateModel>, action: editProfile) {
        this.profileService.updateUser(action.payload)
            .subscribe(data => ctx.dispatch(new editProfileSuccess(data)))
    }
    @Action(editProfileSuccess)
    editProfileSuccess(ctx: StateContext<ProfileStateModel>, action: editProfileSuccess) {
        const state = ctx.getState();
        ctx.setState({ ...state, user: { ...action.payload } });
    }
    @Action(changePassword)
    changePassword(ctx: StateContext<ProfileStateModel>, action: changePassword) {
        this.profileService.changePassword(action.payload)
            .subscribe(() => ctx.dispatch(new changePasswordSuccess()))
    }
    @Action(changePasswordSuccess)
    changePasswordSuccess(ctx: StateContext<ProfileStateModel>, action: changePasswordSuccess) {

    }
}