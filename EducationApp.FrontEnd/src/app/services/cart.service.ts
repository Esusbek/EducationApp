import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { throwError } from 'rxjs';


@Injectable({
    providedIn: 'root',
})
export class CartService {

    private handleError(error: HttpErrorResponse) {
        if (error.error instanceof ErrorEvent) {
            console.error('An error occurred:', error.error.message);
        } else {

        }
        return throwError(
            'Some error happened; please try again later.');
    }

    constructor(private http: HttpClient) {
    }


}