import { Injectable } from "@angular/core";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { of } from "rxjs";
import { catchError, map, mergeMap } from "rxjs/operators";
import { ProfileService } from "src/app/services/profile.service";
import * as ProfileActions from "./profile.actions";

@Injectable()
export class ProfileEffects {
    constructor(private profileService: ProfileService, private actions$: Actions) {
    }
    getInfo$ = createEffect(() => {
        return this.actions$.pipe(
            ofType(ProfileActions.getInfo),
            mergeMap(() =>
                this.profileService.getInfo().pipe(
                    map(data => ProfileActions.getInfoSuccess(data)),
                    catchError(error => of(ProfileActions.getInfoFailure(error))))
            ),
        );
    });
    updateUser$ = createEffect(() => {
        return this.actions$.pipe(
            ofType(ProfileActions.editProfile),
            mergeMap(data => this.profileService.updateUser(data).pipe(
                map(data => ProfileActions.editProfileSuccess(data)),
                catchError(error => of(ProfileActions.editProfileFailure(error))))
            ),
        );
    });
    changePassword$ = createEffect(() => {
        return this.actions$.pipe(
            ofType(ProfileActions.changePassword),
            mergeMap(data => this.profileService.changePassword(data).pipe(
                map(() => ProfileActions.changePasswordSuccess()),
                catchError(error => of(ProfileActions.changePasswordFailure(error))))
            ),
        );
    });
}