import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { loadStripe } from '@stripe/stripe-js';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { OrderItemModel, OrderModel, SessionModel } from 'src/app/models/cart.models';
import { HttpOptions, Urls } from 'src/app/shared/consts';
import { getUserId, handleError } from 'src/app/shared/methods';
import { environment } from 'src/environments/environment';
const stripePromise = loadStripe(environment.stripeKey);

@Injectable({
    providedIn: 'root',
})
export class CartService {

    constructor(private http: HttpClient, private modalService: NgbModal) {
    }

    public checkout(items: Array<OrderItemModel>): Observable<SessionModel> {
        const id = getUserId();
        return this.http.post<string>(`${environment.apiURL}${Urls.checkoutURL}`, { userId: id, currentItems: items }, HttpOptions)
            .pipe(
                catchError(error => handleError(error, this.modalService))
            ) as Observable<SessionModel>;
    }

    public async createSession(session: SessionModel): Promise<void> {
        const stripe = await stripePromise;
        localStorage.setItem('intent', session.paymentIntentId);
        await stripe.redirectToCheckout({
            sessionId: session.id
        })
    }
    public getOrders(page: number): Observable<{ orders: Array<OrderModel>, lastPage: number }> {
        const id = getUserId();
        return this.http.get<Array<OrderModel>>(`${environment.apiURL}${Urls.getOrdersURL}`, {
            params: {
                page: String(page) || "1",
                id
            }
        }).pipe(
            catchError(error => handleError(error, this.modalService))
        ) as Observable<{ orders: Array<OrderModel>, lastPage: number }>;
    }
    public payOrder(): Observable<string> {
        const paymentIntentId = localStorage.getItem('intent');
        return this.http.post<string>(`${environment.apiURL}${Urls.payOrderURL}`, { PaymentIntentId: paymentIntentId }).pipe(
            catchError(error => handleError(error, this.modalService))
        ) as Observable<string>;
    }
}