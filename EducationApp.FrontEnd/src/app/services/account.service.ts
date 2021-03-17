import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Store } from '@ngrx/store';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from '../../environments/environment';
import { EmailActivationModel, LoginCredentials, LoginResult, RegisterModel, ResetPasswordModel } from '../models/account.models';
import { HttpOptions, Urls } from '../shared/consts';
import { getUserId } from '../shared/methods';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  refreshTokenInProgress$;
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

  constructor(private http: HttpClient, public jwtHelper: JwtHelperService, private store: Store) {
    this.refreshTokenInProgress$ = false;
  }
  public isAuthenticated(): boolean {
    const token = localStorage.getItem('accessToken');
    return !this.jwtHelper.isTokenExpired(token);
  }
  public login(user: LoginCredentials, rememberMe: boolean): Observable<LoginResult> {
    return this.http.post<LoginResult>(`${environment.apiURL}${Urls.LoginURL}`, { user, rememberMe }, HttpOptions)
      .pipe(
        catchError(error => this.handleError(error))
      )
  }
  public register(user: RegisterModel): Observable<string> {
    //debugger;
    return this.http.post<string>(`${environment.apiURL}${Urls.RegisterURL}`, user, HttpOptions)
      .pipe(
        catchError(error => this.handleError(error))
      )
  }

  public activateEmail(activationModel: EmailActivationModel): Observable<string> {
    //debugger;
    return this.http.post<string>(`${environment.apiURL}${Urls.ActivationURL}`, activationModel, HttpOptions)
      .pipe(
        catchError(error => this.handleError(error))
      )
  }
  public forgotPassword(username: string): Observable<string> {
    return this.http.post<string>(`${environment.apiURL}${Urls.ForgotPasswordURL}`, { username }, HttpOptions)
      .pipe(
        catchError(error => this.handleError(error))
      )
  }
  public resetPassword(payload: ResetPasswordModel): Observable<string> {

    return this.http.post<string>(`${environment.apiURL}${Urls.ResetPasswordURL}`, payload, HttpOptions)
      .pipe(
        catchError(error => this.handleError(error))
      )
  }
  public refreshToken(): Observable<LoginResult> {
    const accessToken = localStorage.getItem('accessToken');
    const refreshToken = localStorage.getItem('refreshToken');
    return this.http.post<LoginResult>(`${environment.apiURL}${Urls.refreshTokenURL}`, { accessToken, refreshToken }, HttpOptions)
      .pipe(
        catchError(error => this.handleError(error))
      )
  }

  public logout(): Observable<string> {
    const id = getUserId();
    localStorage.removeItem('accessToken');
    localStorage.removeItem('refreshToken');
    return this.http.post<string>(`${environment.apiURL}${Urls.logoutURL}`, { userId: id }, HttpOptions)
      .pipe(
        catchError(error => this.handleError(error))
      )
  }
}