import { createAction, props } from "@ngrx/store";
import { PrintingEditionFilterModel, PrintingEditionInfoModel, PrintingEditionModel } from "src/app/models/printing-edition.models";

export const getBooks = createAction(
    '[Catalog Page] Load Books',
    props<{ page: number }>());
export const getBooksSuccess = createAction(
    '[Catalog Page] Load books success',
    props<{ books: Array<PrintingEditionModel>, info: PrintingEditionInfoModel }>());
export const getFiltered = createAction(
    '[Catalog Page] Load filtered books',
    props<{ filter: PrintingEditionFilterModel, orderAsc: string, page: number }>());