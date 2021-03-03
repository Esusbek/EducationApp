import { Injectable } from "@angular/core";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { Action } from "@ngrx/store";
import { EMPTY, Observable, of } from "rxjs";
import { catchError, map, mergeMap } from "rxjs/operators";
import { ProfileService } from "src/app/services/profile.service";
import * as ProfileActions from "./profile.actions";

@Injectable()
export class ProfileEffects {
    constructor(private profileService: ProfileService, private actions$: Actions) {
    }
    getInfo$ = createEffect(() => {
        console.log('effect worked');
        return this.actions$.pipe(
                ofType(ProfileActions.getInfo),
                mergeMap(() =>
                    this.profileService.getInfo().pipe(
                        map(data => ProfileActions.getInfoSuccess(data)),
                        catchError(error => of(ProfileActions.getInfoFailure(error))))
                    ),
        );
    });
}