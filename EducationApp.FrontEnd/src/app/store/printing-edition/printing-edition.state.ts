import { PrintingEditionInfoModel, PrintingEditionModel } from "src/app/models/printing-edition.models";

export interface PrintingEditionState {
    books: Array<PrintingEditionModel>;
    info: PrintingEditionInfoModel;
}

export const initialState: PrintingEditionState = {
    books: [],
    info: {
        maxPrice: 0,
        minPrice: 0,
        lastPage: 1,
    },
}