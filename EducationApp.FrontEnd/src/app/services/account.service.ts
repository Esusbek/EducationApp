import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { SocialAuthService, SocialUser } from 'angularx-social-login';
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
  public isLoggedInByGoogle: boolean;
  constructor(private http: HttpClient, public jwtHelper: JwtHelperService, private modalService: NgbModal, private socialAuthService: SocialAuthService) {
    this.socialAuthService.authState.subscribe((user) => {
      this.isLoggedInByGoogle = (user != null);
    });
  }
  public isAuthenticated(): boolean {
    const token = localStorage.getItem('accessToken');
    return !!token;
  }
  public login(user: LoginCredentials, rememberMe: boolean): Observable<LoginResult> {
    return this.http.post<LoginResult>(`${environment.apiURL}${Urls.loginURL}`, { user, rememberMe }, HttpOptions)
      .pipe(
        catchError(error => handleError(error, this.modalService))
      )
  }
  public register(user: RegisterModel): Observable<string> {
    return this.http.post<string>(`${environment.apiURL}${Urls.registerURL}`, user, HttpOptions)
      .pipe(
        catchError(error => handleError(error, this.modalService))
      )
  }

  public activateEmail(activationModel: EmailActivationModel): Observable<string> {
    return this.http.post<string>(`${environment.apiURL}${Urls.activationURL}`, activationModel, HttpOptions)
      .pipe(
        catchError(error => handleError(error, this.modalService))
      )
  }
  public forgotPassword(username: string): Observable<string> {
    return this.http.post<string>(`${environment.apiURL}${Urls.forgotPasswordURL}`, { username }, HttpOptions)
      .pipe(
        catchError(error => handleError(error, this.modalService))
      )
  }
  public resetPassword(payload: ResetPasswordModel): Observable<string> {

    return this.http.post<string>(`${environment.apiURL}${Urls.resetPasswordURL}`, payload, HttpOptions)
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
    if (this.isLoggedInByGoogle) {
      this.socialAuthService.signOut();
    }
    const id = getUserId();
    localStorage.removeItem('accessToken');
    localStorage.removeItem('refreshToken');
    return this.http.post<string>(`${environment.apiURL}${Urls.logoutURL}`, { userId: id }, HttpOptions)
      .pipe(
        catchError(error => handleError(error, this.modalService))
      )
  }

  public googleLogin(user: SocialUser): Observable<LoginResult> {
    return this.http.post<LoginResult>(`${environment.apiURL}${Urls.googleLoginURL}`, { idToken: user.idToken }, HttpOptions)
      .pipe(
        catchError(error => handleError(error, this.modalService))
      )
  }
}