import { HttpErrorResponse } from '@angular/common/http';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import decode from 'jwt-decode';
import { throwError } from 'rxjs';
import { TokenPayload } from 'src/app/models/profile.models';
import { ErrorComponent } from '../components/error/error.component';
export function getUserId() {
    const token = localStorage.getItem('accessToken');
    if (token === "undefined") {
        return undefined;
    }
    const tokenPayload = decode<TokenPayload>(token);
    const id = tokenPayload.id;
    return id;
}
export function handleError(error: HttpErrorResponse, modalService: NgbModal) {
    let modalRef = modalService.open(ErrorComponent);
    if (error.error instanceof ErrorEvent) {
        modalRef.componentInstance.message = error.error.message;
    } else {
        modalRef.componentInstance.message = error.error;
    }
    return throwError(
        'Some error happened; please try again later.');
}
