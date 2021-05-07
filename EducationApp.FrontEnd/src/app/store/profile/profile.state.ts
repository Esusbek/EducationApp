import { UserModel } from "src/app/models/profile.models";

export interface ProfileState {
    user: UserModel;
}

export const initialState: ProfileState = {
    user: {
        id: "",
        userName: "",
        firstName: "",
        lastName: "",
        email: "",
    }
}