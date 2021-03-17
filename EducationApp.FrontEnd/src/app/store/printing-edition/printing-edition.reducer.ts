import { Action, createReducer, on } from '@ngrx/store';
import { PrintingEditionInfoModel, PrintingEditionModel } from 'src/app/models/printing-edition.models';
import * as PrintingEditionActions from './printing-edition.actions';
import { initialState, PrintingEditionState } from './printing-edition.state';


const printingEditionReducer = createReducer(
    initialState,
    on(PrintingEditionActions.getBooksSuccess, (state: PrintingEditionState, props: { books: Array<PrintingEditionModel> }) => {
        //state.books = [];
        return { ...state, ...props };
    }),
    on(PrintingEditionActions.getEditionInfoSuccess, (state: PrintingEditionState, props: PrintingEditionInfoModel) => {
        //state.books = [];
        return { ...state, info: { ...props } };
    }),
    on(PrintingEditionActions.getLastPageSuccess, (state: PrintingEditionState, { page }) => {
        //state.books = [];
        return { ...state, info: { ...state.info, page: page } };
    })
);

export function PrintingEditionReducer(state: PrintingEditionState | undefined, action: Action) {
    return printingEditionReducer(state, action);
}