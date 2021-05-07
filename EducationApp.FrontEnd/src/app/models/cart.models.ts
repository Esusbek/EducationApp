import { PrintingEditionModel } from "./printing-edition.models";

export class OrderItemModel {
    amount: number;
    subTotal: number;
    price: number;
    currency: number;
    printingEditionId: number;
    printingEdition: PrintingEditionModel;
}

export class OrderModel {
    id: number;
    date: string;
    status: number;
    currentItems: Array<OrderItemModel>;
}

export class SessionModel {
    id: string;
    paymentIntentId: string;
}