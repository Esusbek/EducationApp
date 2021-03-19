import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { PrintingEditionFilterModel, PrintingEditionInfoModel, PrintingEditionModel } from 'src/app/models/printing-edition.models';
import { Urls } from 'src/app/shared/consts';
import { environment } from 'src/environments/environment';


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
    public getBooks(page: number)
        : Observable<{ books: Array<PrintingEditionModel>, info: PrintingEditionInfoModel }> {
        //debugger;
        return this.http.get<{ books: Array<PrintingEditionModel>, info: PrintingEditionInfoModel }>(`${environment.apiURL}${Urls.getBookURL}`, { params: { page: String(page) || "1" } })
            .pipe(
                catchError(error => this.handleError(error))
            ) as Observable<{ books: Array<PrintingEditionModel>, info: PrintingEditionInfoModel }>;
    }

    public getFiltered(filter: PrintingEditionFilterModel, orderAsc: string, page: number)
        : Observable<{ books: Array<PrintingEditionModel>, info: PrintingEditionInfoModel }> {

        return this.http.get<{ books: Array<PrintingEditionModel>, info: PrintingEditionInfoModel }>(`${environment.apiURL}${Urls.getBookURL}`, {
            params: {
                page: String(page) || "1",
                title: filter.title,
                orderAsc: orderAsc,
                lowPrice: String(filter.lowPrice),
                highPrice: String(filter.highPrice),
                type: filter.type.map(String)
            }
        })
            .pipe(
                catchError(error => this.handleError(error))
            ) as Observable<{ books: Array<PrintingEditionModel>, info: PrintingEditionInfoModel }>;
    }

}