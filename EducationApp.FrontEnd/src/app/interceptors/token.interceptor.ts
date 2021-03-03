import { HttpErrorResponse, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { Store } from "@ngrx/store";
import { EMPTY, Observable, of, Subject, throwError } from "rxjs";
import { catchError, map, switchMap, tap } from "rxjs/operators";
import { LoginResult } from "../models/account.models";
import { AccountService } from "../services/account.service";

@Injectable()
export class TokenInterceptor implements HttpInterceptor {
    refreshTokenInProgress = false;

    tokenRefreshedSource = new Subject();
    tokenRefreshed$ = this.tokenRefreshedSource.asObservable();
    constructor(private auth: AccountService, private router: Router) { }

    refreshToken(): Observable<any> {
        if (this.refreshTokenInProgress) {
            return new Observable(observer => {
                this.tokenRefreshed$.subscribe(() => {
                    observer.next();
                    observer.complete();
                });
            });
        } else {
            this.refreshTokenInProgress = true;
            debugger;
            return this.auth.refreshToken().pipe(
                tap((tokens) => {
                    localStorage.setItem('accessToken', tokens.accessToken);
                    localStorage.setItem('refreshToken', tokens.refreshToken);
                    this.refreshTokenInProgress = false;
                    this.tokenRefreshedSource.next();
                }),
                catchError( () => {
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
                    return next.handle(request);
                }),
                catchError(e => {
                    if (e.status !== 401) {
                        return this.handleResponseError(e);
                    } else {
                        this.logout();
                    }
                }));
        }
        return throwError(error);
    }
    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<any> {
        return next.handle(request).pipe(catchError(error => {
            return this.handleResponseError(error, request, next);
        }));
    }
}