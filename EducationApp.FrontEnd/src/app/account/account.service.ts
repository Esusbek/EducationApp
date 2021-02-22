import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import {LoginCredentials, LoginResult} from './account.models';
import { HttpClient, HttpErrorResponse, HttpHeaders, HttpParams } from '@angular/common/http';
import { catchError } from 'rxjs/operators';

const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type':  'application/json',
    'Accept': 'application/json',
    'Access-Control-Allow-Headers': 'Content-Type'
  })
};

@Injectable({
  providedIn: 'root',
})
export class AccountService {

  private LoginURL: string = 'https://localhost:44319/Account/Login';

  private handleError(error: HttpErrorResponse) {
    if (error.error instanceof ErrorEvent) {
        console.error('An error occurred:', error.error.message);
    } else {
        console.error(
            `Backend returned code ${error.status}, ` +
            `body was: ${error.error}`);
    }
    return throwError(
        'Some error happened; please try again later.');
}

  constructor(private http: HttpClient) { }
  login(credentials: LoginCredentials, rememberMe: boolean): Observable<LoginResult> {
    console.log('service', {user: credentials, rememberMe});
    return this.http.post<LoginResult>(this.LoginURL, {user: credentials, rememberMe}, httpOptions)
                    .pipe(
                        catchError(error => this.handleError(error))
                    )
  }
}