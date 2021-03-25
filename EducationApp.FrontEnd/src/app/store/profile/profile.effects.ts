import { Injectable } from "@angular/core";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { map, mergeMap } from "rxjs/operators";
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
                    map(data => ProfileActions.getInfoSuccess(data)))
            ),
        );
    });
    updateUser$ = createEffect(() => {
        return this.actions$.pipe(
            ofType(ProfileActions.editProfile),
            mergeMap(data => this.profileService.updateUser(data).pipe(
                map(data => ProfileActions.editProfileSuccess(data)))
            ),
        );
    });
    changePassword$ = createEffect(() => {
        return this.actions$.pipe(
            ofType(ProfileActions.changePassword),
            mergeMap(data => this.profileService.changePassword(data).pipe(
                map(() => ProfileActions.changePasswordSuccess()))
            ),
        );
    });
}