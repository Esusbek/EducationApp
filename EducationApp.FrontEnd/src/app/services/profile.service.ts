import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ChangePasswordModel, UserModel } from 'src/app/models/profile.models';
import { HttpOptions, Urls } from 'src/app/shared/consts';
import { getUserId, handleError } from 'src/app/shared/methods';
import { environment } from 'src/environments/environment';


@Injectable({
  providedIn: 'root',
})
export class ProfileService {


  constructor(private http: HttpClient, private modalService: NgbModal) {
  }


  public getInfo(): Observable<UserModel> {
    const id = getUserId();
    return this.http.get<UserModel>(`${environment.apiURL}${Urls.getUserURL}`, { params: { userId: id } })
      .pipe(
        catchError(error => handleError(error, this.modalService))
      ) as Observable<UserModel>;
  }
  public changePassword(model: ChangePasswordModel): Observable<string> {
    return this.http.post<string>(`${environment.apiURL}${Urls.changePasswordURL}`, model, HttpOptions)
      .pipe(
        catchError(error => handleError(error, this.modalService))
      )
  }
  public updateUser(model: UserModel): Observable<UserModel> {
    return this.http.post<UserModel>(`${environment.apiURL}${Urls.updateUserURL}`, model, HttpOptions)
      .pipe(
        catchError(error => handleError(error, this.modalService))
      )
  }
}