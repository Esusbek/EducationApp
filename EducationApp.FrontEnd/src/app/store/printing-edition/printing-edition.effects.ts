import { Injectable } from "@angular/core";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { map, mergeMap } from "rxjs/operators";
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
                    map(data => PrintingEditionActions.getBooksSuccess({ books: data.books, info: data.info })))
            ),
        );
    });

    getFilteredBooks$ = createEffect(() => {
        return this.actions$.pipe(
            ofType(PrintingEditionActions.getFiltered),
            mergeMap(data =>
                this.bookService.getFiltered(data.filter, data.orderAsc, data.page).pipe(
                    map(data => PrintingEditionActions.getBooksSuccess({ books: data.books, info: data.info })))
            ),
        );
    });
}