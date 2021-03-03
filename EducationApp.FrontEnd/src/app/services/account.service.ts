import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import {LoginCredentials, LoginResult, RegisterModel, EmailActivationModel, ResetPasswordModel} from '../models/account.models';
import { HttpClient, HttpErrorResponse, HttpHeaders, HttpParams } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import {JwtHelperService} from '@auth0/angular-jwt';
import {environment} from '../../environments/environment';
import {HttpOptions} from '../shared/consts';
import decode from 'jwt-decode';
import { TokenPayload } from '../models/profile.models';

@Injectable({
  providedIn: 'root',
})
export class AccountService {

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

  constructor(private http: HttpClient, public jwtHelper: JwtHelperService) { 
  }
  public login(user: LoginCredentials, rememberMe: boolean): Observable<LoginResult> {
    return this.http.post<LoginResult>(`${environment.apiURL}${environment.LoginURL}`, {user, rememberMe}, HttpOptions)
                    .pipe(
                        catchError(error => this.handleError(error))
                    )
  }
  public register(user: RegisterModel): Observable<string> {
    //debugger;
    return this.http.post<string>(`${environment.apiURL}${environment.RegisterURL}`, user, HttpOptions)
                    .pipe(
                        catchError(error => this.handleError(error))
                    )
  }
  public isAuthenticated(): boolean {
    //debugger;
    return true;
    const token = localStorage.getItem('accessToken');
    return !this.jwtHelper.isTokenExpired(token);
  }
  public activateEmail(activationModel: EmailActivationModel): Observable<string>{
    //debugger;
    return this.http.post<string>(`${environment.apiURL}${environment.ActivationURL}`, activationModel, HttpOptions)
                    .pipe(
                        catchError(error => this.handleError(error))
                    )
  }
  public forgotPassword(username: string): Observable<string>{
    return this.http.post<string>(`${environment.apiURL}${environment.ForgotPasswordURL}`, {username}, HttpOptions)
                    .pipe(
                        catchError(error => this.handleError(error))
                    )
  }
  public resetPassword(payload: ResetPasswordModel): Observable<string>{
  
    return this.http.post<string>(`${environment.apiURL}${environment.ResetPasswordURL}`, payload, HttpOptions)
                    .pipe(
                        catchError(error => this.handleError(error))
                    )
  }
  public refreshToken(): Observable<LoginResult>{
    const accessToken = localStorage.getItem('accessToken');
    const refreshToken = localStorage.getItem('refreshToken');
    return this.http.post<LoginResult>(`${environment.apiURL}${environment.refreshTokenURL}`, {accessToken, refreshToken}, HttpOptions)
    .pipe(
        catchError(error => this.handleError(error))
    )
  } 

  public logout(): Observable<string>{
    const token = localStorage.getItem('accessToken');
      const tokenPayload = decode<TokenPayload>(token);
      const id = tokenPayload.id;
      console.log(id);
    return this.http.post<string>(`${environment.apiURL}${environment.logoutURL}`,{userId: id}, HttpOptions)
    .pipe(
      catchError(error => this.handleError(error))
    )
  }
}