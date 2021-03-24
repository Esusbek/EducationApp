import { HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { Store } from "@ngrx/store";
import { EMPTY, Observable, Subject, throwError } from "rxjs";
import { catchError, switchMap, tap } from "rxjs/operators";
import { AccountService } from "src/app/services/account.service";
import { refreshTokens } from "src/app/store/account/account.actions";

@Injectable()
export class TokenInterceptor implements HttpInterceptor {
    refreshTokenInProgress = false;

    tokenRefreshedSource = new Subject();
    tokenRefreshed = this.tokenRefreshedSource.asObservable();
    constructor(private auth: AccountService, private router: Router, private store: Store) { }

    addAuthHeader(request) {
        const authToken = localStorage.getItem('accessToken');
        if (authToken) {
            const authHeader = `Bearer ${authToken}`;
            return request.clone({
                setHeaders: {
                    "Authorization": authHeader
                }
            });
        }
        return request;
    }

    refreshToken(): Observable<any> {
        if (this.refreshTokenInProgress) {
            return new Observable(observer => {
                this.tokenRefreshed.subscribe(() => {
                    observer.next();
                    observer.complete();
                });
            });
        } else {
            this.refreshTokenInProgress = true;
            return this.auth.refreshToken().pipe(
                tap((tokens) => {
                    this.store.dispatch(refreshTokens(tokens));
                    this.refreshTokenInProgress = false;
                    this.tokenRefreshedSource.next();
                }),
                catchError(() => {
                    this.refreshTokenInProgress = false;
                    this.logout();
                    return EMPTY;
                }));
        }
    }
    logout() {
        this.auth.logout().subscribe();
        this.router.navigate(["/login"]);
    }

    handleResponseError(error, request?, next?) {
        if (error.status === 401) {
            return this.refreshToken().pipe(
                switchMap(() => {
                    request = this.addAuthHeader(request);
                    return next.handle(request);
                }),
                catchError(error => {
                    if (error.status !== 401) {
                        return this.handleResponseError(error);
                    } else {
                        debugger;
                        this.logout();
                    }
                }));
        }
        if (error.status === 403) {
            this.router.navigate(["/"]);
        }
        return throwError(error);
    }
    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<any> {
        request = this.addAuthHeader(request);
        return next.handle(request).pipe(catchError(error => {
            return this.handleResponseError(error, request, next);
        }));
    }
}