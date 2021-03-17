import { HttpErrorResponse } from "@angular/common/http";
import { createAction, props } from "@ngrx/store";
import { PrintingEditionFilterModel, PrintingEditionInfoModel, PrintingEditionModel } from "src/app/models/printing-edition.models";

export const getBooks = createAction(
    '[Catalog Page] Load Books',
    props<{ page: string }>());
export const getBooksSuccess = createAction(
    '[Catalog Page] Load books success',
    props<{ books: Array<PrintingEditionModel> }>());
export const getBooksFailure = createAction(
    '[Catalog Page] Load books failure',
    props<{ error: HttpErrorResponse }>());
export const getEditionInfo = createAction(
    '[Catalog Page] Load Info');
export const getEditionInfoSuccess = createAction(
    '[Catalog Page] Load info success',
    props<PrintingEditionInfoModel>());
export const getEditionInfoFailure = createAction(
    '[Catalog Page] Load info failure',
    props<{ error: HttpErrorResponse }>());
export const getFiltered = createAction(
    '[Catalog Page] Load filtered books',
    props<{ filter: PrintingEditionFilterModel, page: string }>());
export const getFilteredFailure = createAction(
    '[Catalog Page] Load filtered books failure',
    props<{ error: HttpErrorResponse }>());
export const getLastPage = createAction(
    '[Catalog Page] Load page count',
    props<{ filter: PrintingEditionFilterModel }>());
export const getLastPageSuccess = createAction(
    '[Catalog Page] Load page count success',
    props<{ page: number }>());
export const getLastPageFailure = createAction(
    '[Catalog Page] Load page count failure',
    props<{ error: HttpErrorResponse }>());