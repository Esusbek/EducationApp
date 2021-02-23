export interface AuthState {
    accessToken: string;
    refreshToken: string;
}

export const initialState: AuthState = {
    accessToken: '',
    refreshToken: ''
}
