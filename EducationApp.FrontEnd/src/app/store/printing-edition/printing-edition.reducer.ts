import { Action, createReducer, on } from '@ngrx/store';
import { PrintingEditionInfoModel, PrintingEditionModel } from 'src/app/models/printing-edition.models';
import * as PrintingEditionActions from './printing-edition.actions';
import { initialState, PrintingEditionState } from './printing-edition.state';


const printingEditionReducer = createReducer(
    initialState,
    on(PrintingEditionActions.getBooksSuccess, (state: PrintingEditionState, props: { books: Array<PrintingEditionModel>, info: PrintingEditionInfoModel }) => {
        return { ...state, ...props };
    }),
);

export function PrintingEditionReducer(state: PrintingEditionState | undefined, action: Action) {
    return printingEditionReducer(state, action);
}