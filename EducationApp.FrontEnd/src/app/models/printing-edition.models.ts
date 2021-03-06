import { AuthorModel } from "./author.models";

export class PrintingEditionModel {
    id: number;
    title: string;
    description: string;
    price: number;
    status: boolean;
    currency: number;
    type: number;
    authors: AuthorModel[];
}

export class PrintingEditionInfoModel {
    minPrice: number;
    maxPrice: number;
    lastPage: number;
}

export class PrintingEditionFilterModel {
    title: string;
    lowPrice: number;
    highPrice: number;
    type: Array<number>;
    orderAsc: boolean;
}