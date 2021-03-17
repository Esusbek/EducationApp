import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { PrintingEditionFilterModel, PrintingEditionInfoModel, PrintingEditionModel } from '../models/printing-edition.models';
import { Urls } from '../shared/consts';


@Injectable({
    providedIn: 'root',
})
export class PrintingEditionService {

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
    public getBooks(page: string): Observable<Array<PrintingEditionModel>> {
        //debugger;
        return this.http.get<Array<PrintingEditionModel>>(`${environment.apiURL}${Urls.getBookURL}`, { params: { page: page || "1" } })
            .pipe(
                catchError(error => this.handleError(error))
            ) as Observable<Array<PrintingEditionModel>>;
    }
    public getInfo(): Observable<PrintingEditionInfoModel> {
        return this.http.get<PrintingEditionInfoModel>(`${environment.apiURL}${Urls.getEditionInfoURL}`)
            .pipe(
                catchError(error => this.handleError(error))
            ) as Observable<PrintingEditionInfoModel>;
    }
    public getLastPage(filter: PrintingEditionFilterModel): Observable<number> {
        return this.http.get<PrintingEditionInfoModel>(`${environment.apiURL}${Urls.getLastPageURL}`, {
            params: {
                title: filter.title,
                lowPrice: String(filter.lowPrice),
                highPrice: String(filter.highPrice),
                type: filter.type.map(String)
            }
        })
            .pipe(
                catchError(error => this.handleError(error))
            ) as Observable<number>;
    }

    public getFiltered(filter: PrintingEditionFilterModel, page: string): Observable<Array<PrintingEditionModel>> {
        return this.http.get<Array<PrintingEditionModel>>(`${environment.apiURL}${Urls.getBookURL}`, {
            params: {
                page: page || "1",
                title: filter.title,
                lowPrice: String(filter.lowPrice),
                highPrice: String(filter.highPrice),
                type: filter.type.map(String)
            }
        })
            .pipe(
                catchError(error => this.handleError(error))
            ) as Observable<Array<PrintingEditionModel>>;
    }

}