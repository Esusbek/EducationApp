import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { EmailActivationModel, LoginCredentials, LoginResult, RegisterModel, ResetPasswordModel } from 'src/app/models/account.models';
import { HttpOptions, Urls } from 'src/app/shared/consts';
import { getUserId, handleError } from 'src/app/shared/methods';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class AccountService {

  constructor(private http: HttpClient, public jwtHelper: JwtHelperService, private store: Store, private modalService: NgbModal) { }
  public isAuthenticated(): boolean {
    const token = localStorage.getItem('accessToken');
    return !!token;
  }
  public login(user: LoginCredentials, rememberMe: boolean): Observable<LoginResult> {
    return this.http.post<LoginResult>(`${environment.apiURL}${Urls.LoginURL}`, { user, rememberMe }, HttpOptions)
      .pipe(
        catchError(error => handleError(error, this.modalService))
      )
  }
  public register(user: RegisterModel): Observable<string> {
    return this.http.post<string>(`${environment.apiURL}${Urls.RegisterURL}`, user, HttpOptions)
      .pipe(
        catchError(error => handleError(error, this.modalService))
      )
  }

  public activateEmail(activationModel: EmailActivationModel): Observable<string> {
    return this.http.post<string>(`${environment.apiURL}${Urls.ActivationURL}`, activationModel, HttpOptions)
      .pipe(
        catchError(error => handleError(error, this.modalService))
      )
  }
  public forgotPassword(username: string): Observable<string> {
    return this.http.post<string>(`${environment.apiURL}${Urls.ForgotPasswordURL}`, { username }, HttpOptions)
      .pipe(
        catchError(error => handleError(error, this.modalService))
      )
  }
  public resetPassword(payload: ResetPasswordModel): Observable<string> {

    return this.http.post<string>(`${environment.apiURL}${Urls.ResetPasswordURL}`, payload, HttpOptions)
      .pipe(
        catchError(error => handleError(error, this.modalService))
      )
  }
  public refreshToken(): Observable<LoginResult> {
    const accessToken = localStorage.getItem('accessToken');
    const refreshToken = localStorage.getItem('refreshToken');
    return this.http.post<LoginResult>(`${environment.apiURL}${Urls.refreshTokenURL}`, { accessToken, refreshToken }, HttpOptions)
      .pipe(
        catchError(error => handleError(error, this.modalService))
      )
  }

  public logout(): Observable<string> {
    const id = getUserId();
    localStorage.removeItem('accessToken');
    localStorage.removeItem('refreshToken');
    return this.http.post<string>(`${environment.apiURL}${Urls.logoutURL}`, { userId: id }, HttpOptions)
      .pipe(
        catchError(error => handleError(error, this.modalService))
      )
  }
}