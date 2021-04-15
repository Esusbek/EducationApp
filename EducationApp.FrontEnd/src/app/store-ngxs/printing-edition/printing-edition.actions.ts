import { PrintingEditionFilterModel, PrintingEditionInfoModel, PrintingEditionModel } from "src/app/models/printing-edition.models";

export class getBooks {
    static readonly type = '[Catalog Page] Load Books';
    constructor(public page: number) { }
}
export class getBooksSuccess {
    static readonly type = '[Catalog Page] Load books success';
    constructor(public payload: { books: Array<PrintingEditionModel>, info: PrintingEditionInfoModel }) { }
}
export class getFiltered {
    static readonly type = '[Catalog Page] Load filtered books';
    constructor(public payload: { filter: PrintingEditionFilterModel, orderAsc: string, page: number }) { }
}