import { Injectable } from '@angular/core';
import { Action, Selector, State, StateContext } from '@ngxs/store';
import { PrintingEditionInfoModel, PrintingEditionModel } from 'src/app/models/printing-edition.models';
import { PrintingEditionService } from 'src/app/services/printing-edition.service';
import { getBooks, getBooksSuccess, getFiltered } from './printing-edition.actions';

export interface PrintingEditionStateModel {
    books: Array<PrintingEditionModel>;
    info: PrintingEditionInfoModel;
}

const initialState: PrintingEditionStateModel = {
    books: [],
    info: {
        maxPrice: 0,
        minPrice: 0,
        lastPage: 1,
    },
}
getBooks
getBooksSuccess
getFiltered
@State<PrintingEditionStateModel>({
    name: 'books',
    defaults: initialState
})
@Injectable()
export class PrintingEditionState {
    constructor(private bookService: PrintingEditionService) { }
    @Selector()
    static books(state: PrintingEditionStateModel) {
        return state.books;
    }
    @Selector()
    static info(state: PrintingEditionStateModel) {
        return state.info;
    }
    @Action(getBooks)
    getBooks(ctx: StateContext<PrintingEditionStateModel>, action: getBooks) {
        this.bookService.getBooks(action.page)
            .subscribe(data =>
                ctx.dispatch(new getBooksSuccess({ books: data.books, info: data.info }))
            )
    }
    @Action(getBooksSuccess)
    getBooksSuccess(ctx: StateContext<PrintingEditionStateModel>, action: getBooksSuccess) {
        const state = ctx.getState();
        ctx.setState({ ...state, ...action.payload });
    }
    @Action(getFiltered)
    getFiltered(ctx: StateContext<PrintingEditionStateModel>, action: getFiltered) {
        this.bookService.getFiltered(action.payload.filter, action.payload.orderAsc, action.payload.page)
            .subscribe(data => ctx.dispatch(new getBooksSuccess({ books: data.books, info: data.info })));
    }
}