import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Store } from '@ngrx/store';
import { UserModel } from 'src/app/models/profile.models';
import { AccountService } from 'src/app/services/account.service';
import { getUserId } from 'src/app/shared/methods';
import { changePassword, editProfile, getInfo } from 'src/app/store/profile/profile.actions';
import { ProfileState } from 'src/app/store/profile/profile.state';
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
  profileForm = new FormGroup({
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
  changePasswordForm = new FormGroup({
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
  constructor(private store: Store<{ Profile: ProfileState }>, private auth: AccountService) {
    store.select('Profile').subscribe(val => this.profileForm.controls['userName'].setValue(val.user.userName));
    store.select('Profile').subscribe(val => this.profileForm.controls['firstName'].setValue(val.user.firstName));
    store.select('Profile').subscribe(val => this.profileForm.controls['lastName'].setValue(val.user.lastName));
    store.select('Profile').subscribe(val => this.profileForm.controls['email'].setValue(val.user.email));
    this.profileForm.controls['userName'].disable();
    this.profileForm.controls['firstName'].disable();
    this.profileForm.controls['lastName'].disable();
    this.profileForm.controls['email'].disable();
    this.isEditing = false;
    this.isChangingPassword = false;
    this.showPasswords = false;
  }

  ngOnInit(): void {
    this.store.dispatch(getInfo());

  }
  editStart() {
    this.isEditing = true;
    this.profileForm.controls['firstName'].enable();
    this.profileForm.controls['lastName'].enable();
  }
  editEnd() {
    this.isEditing = false;
    this.profileForm.controls['firstName'].disable();
    this.profileForm.controls['lastName'].disable();
  }
  onSubmit() {
    this.isEditing = false;
    this.profileForm.controls['firstName'].disable();
    this.profileForm.controls['lastName'].disable();
    this.store.dispatch(editProfile({ ...this.profileForm.value, id: getUserId() }));
  }
  changingPassword() {
    this.isChangingPassword = !this.isChangingPassword;
  }
  onChangePassword() {
    this.isChangingPassword = false;
    this.store.dispatch(changePassword({
      user: { id: getUserId() },
      newPassword: this.changePasswordForm.value.password,
      currentPassword: this.changePasswordForm.value.currentPassword
    }));
  }
  showPassword() {
    this.showPasswords = !this.showPasswords;
  }
}
