import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { PrintingEditionFilterModel, PrintingEditionInfoModel, PrintingEditionModel } from 'src/app/models/printing-edition.models';
import { Urls } from 'src/app/shared/consts';
import { handleError } from 'src/app/shared/methods';
import { environment } from 'src/environments/environment';


@Injectable({
    providedIn: 'root',
})
export class PrintingEditionService {

    constructor(private http: HttpClient, private modalService: NgbModal) {
    }
    public getBooks(page: number)
        : Observable<{ books: Array<PrintingEditionModel>, info: PrintingEditionInfoModel }> {
        //debugger;
        return this.http.get<{ books: Array<PrintingEditionModel>, info: PrintingEditionInfoModel }>(`${environment.apiURL}${Urls.getBookURL}`, { params: { page: String(page) || "1" } })
            .pipe(
                catchError(error => handleError(error, this.modalService))
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
                catchError(error => handleError(error, this.modalService))
            ) as Observable<{ books: Array<PrintingEditionModel>, info: PrintingEditionInfoModel }>;
    }

}