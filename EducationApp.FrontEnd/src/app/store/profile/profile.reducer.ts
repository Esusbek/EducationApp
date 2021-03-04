import { Action, createReducer, on } from '@ngrx/store';
import * as ProfileActions from './profile.actions';
import{UserModel} from '../../models/profile.models';
import{initialState, ProfileState} from './profile.state';


const profileReducer = createReducer(
    initialState, 
    on(ProfileActions.getInfoSuccess, (state: ProfileState, props: UserModel) => {
        return {...state, user: {...props}};
    }),
    on(ProfileActions.editProfileSuccess, (state: ProfileState, props: UserModel) => {
        return {...state, user: {...props}};
    })
);

export function ProfileReducer(state: ProfileState | undefined, action: Action){
    return profileReducer(state, action);
}