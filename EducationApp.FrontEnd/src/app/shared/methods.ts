import decode from 'jwt-decode';
import { TokenPayload } from 'src/app/models/profile.models';
export function getUserId() {
    const token = localStorage.getItem('accessToken');
    if (token === "undefined") {
        return undefined;
    }
    const tokenPayload = decode<TokenPayload>(token);
    const id = tokenPayload.id;
    return id;
}