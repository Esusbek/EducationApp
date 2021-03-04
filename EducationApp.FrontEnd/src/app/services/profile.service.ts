import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { HttpClient, HttpErrorResponse, HttpHeaders, HttpParams } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import decode from 'jwt-decode';
import { environment } from '../../environments/environment';
import { HttpOptions } from '../shared/consts'
import { ChangePasswordModel, TokenPayload, UserModel } from '../models/profile.models';
import { AccountService } from './account.service';
import { getUserId } from '../shared/methods';


@Injectable({
  providedIn: 'root',
})
export class ProfileService {

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
  

  public getInfo(): Observable<UserModel> {
    const id = getUserId();
    return this.http.get<UserModel>(`${environment.apiURL}${environment.getUserURL}`, { params: { userId: id } })
      .pipe(
        catchError(error => this.handleError(error))
      ) as Observable<UserModel>;
  }
  public changePassword(model: ChangePasswordModel): Observable<string> {
    debugger;
    return this.http.post<string>(`${environment.apiURL}${environment.changePasswordURL}`, model, HttpOptions)
      .pipe(
        catchError(error => this.handleError(error))
      )
  }
  public updateUser(model: UserModel): Observable<UserModel> {
    debugger;
    
    return this.http.post<UserModel>(`${environment.apiURL}${environment.updateUserURL}`, model, HttpOptions)
      .pipe(
        catchError(error => this.handleError(error))
      )
  }
}