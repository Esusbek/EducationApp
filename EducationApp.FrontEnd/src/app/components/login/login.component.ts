import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Store } from '@ngxs/store';
import { forgotPassword, login } from 'src/app/store-ngxs/account/account.actions';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  public forgotPassword: boolean;
  public showPasswords: boolean;
  public loginForm = new FormGroup({
    user: new FormGroup({
      userName: new FormControl('', [
        Validators.required
      ]),
      password: new FormControl('', [
        Validators.required
      ])
    }),
    rememberMe: new FormControl(false)
  })
  public forgotPasswordForm = new FormGroup({
    userName: new FormControl('', [
      Validators.required
    ])
  })
  public reseted: boolean;
  public get userName() { return this.loginForm.get('user').get('userName') }
  public get password() { return this.loginForm.get('user').get('password') }
  public get userNameRecovery() { return this.forgotPasswordForm.get('userName') }
  public constructor(private store: Store) {
    this.forgotPassword = false;
    this.reseted = false;
    this.showPasswords = false;
  }

  public ngOnInit(): void {
  }

  public onSubmit() {
    this.store.dispatch(new login({ ...this.loginForm.value }))
  }
  public onForgotPassword() {
    this.forgotPassword = !this.forgotPassword;
  }
  public onRecoverySubmit() {
    this.store.dispatch(new forgotPassword({ ...this.forgotPasswordForm.value }))
    this.reseted = true;
  }
  public showPassword() {
    this.showPasswords = !this.showPasswords;
  }
  public onGoogleSignIn(googleUser) {
    var profile = googleUser.getBasicProfile();
    console.log('ID: ' + profile.getId()); // Do not send to your backend! Use an ID token instead.
    console.log('Name: ' + profile.getName());
    console.log('Image URL: ' + profile.getImageUrl());
    console.log('Email: ' + profile.getEmail()); // This is null if the 'email' scope is not present.
  }
}
