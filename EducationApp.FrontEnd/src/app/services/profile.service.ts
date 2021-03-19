import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ChangePasswordModel, UserModel } from 'src/app/models/profile.models';
import { HttpOptions, Urls } from 'src/app/shared/consts';
import { getUserId } from 'src/app/shared/methods';
import { environment } from 'src/environments/environment';


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
    return this.http.get<UserModel>(`${environment.apiURL}${Urls.getUserURL}`, { params: { userId: id } })
      .pipe(
        catchError(error => this.handleError(error))
      ) as Observable<UserModel>;
  }
  public changePassword(model: ChangePasswordModel): Observable<string> {
    debugger;
    return this.http.post<string>(`${environment.apiURL}${Urls.changePasswordURL}`, model, HttpOptions)
      .pipe(
        catchError(error => this.handleError(error))
      )
  }
  public updateUser(model: UserModel): Observable<UserModel> {
    debugger;

    return this.http.post<UserModel>(`${environment.apiURL}${Urls.updateUserURL}`, model, HttpOptions)
      .pipe(
        catchError(error => this.handleError(error))
      )
  }
}