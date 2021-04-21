import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Store } from '@ngxs/store';
import { GoogleLoginProvider, SocialAuthService } from 'angularx-social-login';
import { forgotPassword, googleLogin, login } from 'src/app/store-ngxs/account/account.actions';

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
  public constructor(private store: Store, private socialAuthService: SocialAuthService) {
    this.forgotPassword = false;
    this.reseted = false;
    this.showPasswords = false;
  }

  public ngOnInit(): void {

  }
  public loginWithGoogle(): void {
    this.socialAuthService.authState.subscribe((user) => {
      if (user != null) {
        this.store.dispatch(new googleLogin(user));
      }
    });
    this.socialAuthService.signIn(GoogleLoginProvider.PROVIDER_ID);
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

}
