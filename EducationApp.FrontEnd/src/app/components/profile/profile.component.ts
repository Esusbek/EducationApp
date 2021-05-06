import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Store } from '@ngxs/store';
import { UserModel } from 'src/app/models/profile.models';
import { AccountService } from 'src/app/services/account.service';
import { Defaults } from 'src/app/shared/consts';
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
  @ViewChild('profilePictureInput')
  public profilePictureInput: ElementRef;
  public imgBase64Path: string;
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
    this.store.select(ProfileState.user).subscribe(value => this.profilePictureURL = value.profilePictureURL);
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
    console.log(this.profileForm.value);

    this.store.dispatch(new editProfile({ ...this.profileForm.value, id: getUserId(), profilePicture: this.imgBase64Path }));
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
  public onProfilePictureChange(event) {

    if (event.target.files.length <= 0) {
      return;
    }
    const file = event.target.files[0];
    if (!file.type.match(Defaults.filePattern)) {
      alert("File was not an image");
      this.profilePictureInput.nativeElement.value = "";
    }
    const reader = new FileReader();
    reader.onload = (e: any) => {
      const image = new Image();
      image.src = e.target.result;
      image.onload = rs => {
        const img_height = rs.currentTarget['height'];
        const img_width = rs.currentTarget['width'];
        if (img_height > Defaults.max_height && img_width > Defaults.max_width) {
          alert(`Maximum dimentions allowed ${Defaults.max_height}'*'${Defaults.max_width}px`);
          return false;
        } else {
          this.imgBase64Path = e.target.result;
        }
      };
    };
    reader.readAsDataURL(event.target.files[0]);
  }
}
