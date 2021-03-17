import { Injectable } from "@angular/core";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { of } from "rxjs";
import { catchError, map, mergeMap } from "rxjs/operators";
import { PrintingEditionService } from "src/app/services/printing-edition.service";
import * as PrintingEditionActions from "./printing-edition.actions";

@Injectable()
export class PrintingEditionEffects {
    constructor(private bookService: PrintingEditionService, private actions$: Actions) {
    }
    getBooks$ = createEffect(() => {
        return this.actions$.pipe(
            ofType(PrintingEditionActions.getBooks),
            mergeMap(data =>
                this.bookService.getBooks(data.page).pipe(
                    map(data => PrintingEditionActions.getBooksSuccess({ books: data })),
                    catchError(error => of(PrintingEditionActions.getBooksFailure(error))))
            ),
        );
    });
    getInfo$ = createEffect(() => {
        return this.actions$.pipe(
            ofType(PrintingEditionActions.getEditionInfo),
            mergeMap(() =>
                this.bookService.getInfo().pipe(
                    map(data => PrintingEditionActions.getEditionInfoSuccess(data)),
                    catchError(error => of(PrintingEditionActions.getEditionInfoFailure(error))))
            ),
        );
    });
    getLastPage$ = createEffect(() => {
        return this.actions$.pipe(
            ofType(PrintingEditionActions.getLastPage),
            mergeMap(payload => this.bookService.getLastPage(payload.filter).pipe(
                map(data => PrintingEditionActions.getLastPageSuccess({ page: data })),
                catchError(error => of(PrintingEditionActions.getLastPageFailure(error))))
            ),
        );
    });
    getFilteredBooks$ = createEffect(() => {
        return this.actions$.pipe(
            ofType(PrintingEditionActions.getFiltered),
            mergeMap(data =>
                this.bookService.getFiltered(data.filter, data.page).pipe(
                    map(data => PrintingEditionActions.getBooksSuccess({ books: data })),
                    catchError(error => of(PrintingEditionActions.getFilteredFailure(error))))
            ),
        );
    });
}