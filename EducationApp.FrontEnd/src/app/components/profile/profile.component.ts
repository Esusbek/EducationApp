import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Store } from '@ngxs/store';
import { UserModel } from 'src/app/models/profile.models';
import { AccountService } from 'src/app/services/account.service';
import { getUserId } from 'src/app/shared/methods';
import { changePassword, editProfile, getInfo } from 'src/app/store-ngxs/profile/profile.actions';
import { ProfileState } from 'src/app/store-ngxs/profile/profile.state';
import { passwordMatchValidator } from 'src/app/validators/password-match.directive';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  public isEditing: boolean;
  public profile: UserModel;
  public isChangingPassword: boolean;
  public showPasswords: boolean;
  public profilePictureURL: string;
  public profileForm = new FormGroup({
    userName: new FormControl('', [
      Validators.required
    ]),
    firstName: new FormControl('', [
      Validators.required
    ]),
    lastName: new FormControl('', [
      Validators.required
    ]),
    email: new FormControl('', [
      Validators.required,
      Validators.email
    ])
  })
  public changePasswordForm = new FormGroup({
    currentPassword: new FormControl('', [
      Validators.required
    ]),
    password: new FormControl('', [
      Validators.required
    ]),
    passwordConfirm: new FormControl('', [
      Validators.required
    ])
  }, { validators: passwordMatchValidator })
  public constructor(private store: Store, private auth: AccountService) {
    this.store.select(ProfileState.user).subscribe(value => this.profilePictureURL = value.profilePictureURL);
    this.store.select(ProfileState.user).subscribe(value => this.profileForm.controls['userName'].setValue(value.userName));
    this.store.select(ProfileState.user).subscribe(value => this.profileForm.controls['firstName'].setValue(value.firstName));
    this.store.select(ProfileState.user).subscribe(value => this.profileForm.controls['lastName'].setValue(value.lastName));
    this.store.select(ProfileState.user).subscribe(value => this.profileForm.controls['email'].setValue(value.email));
    this.profileForm.controls['userName'].disable();
    this.profileForm.controls['firstName'].disable();
    this.profileForm.controls['lastName'].disable();
    this.profileForm.controls['email'].disable();
    this.isEditing = false;
    this.isChangingPassword = false;
    this.showPasswords = false;
  }

  public ngOnInit(): void {
    this.store.dispatch(new getInfo());

  }
  public editStart() {
    console.log(this.profilePictureURL);
    this.isEditing = true;
    this.profileForm.controls['firstName'].enable();
    this.profileForm.controls['lastName'].enable();
  }
  public editEnd() {
    this.isEditing = false;
    this.profileForm.controls['firstName'].disable();
    this.profileForm.controls['lastName'].disable();
  }
  public onSubmit() {
    this.isEditing = false;
    this.profileForm.controls['firstName'].disable();
    this.profileForm.controls['lastName'].disable();
    this.store.dispatch(new editProfile({ ...this.profileForm.value, id: getUserId() }));
  }
  public changingPassword() {
    this.isChangingPassword = !this.isChangingPassword;
  }
  public onChangePassword() {
    this.isChangingPassword = false;
    this.store.dispatch(new changePassword({
      user: { id: getUserId() },
      newPassword: this.changePasswordForm.value.password,
      currentPassword: this.changePasswordForm.value.currentPassword
    }));
  }
  public showPassword() {
    this.showPasswords = !this.showPasswords;
  }
}
