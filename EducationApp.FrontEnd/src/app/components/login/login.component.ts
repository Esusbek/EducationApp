import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Store } from '@ngrx/store';
import { forgotPassword, login } from 'src/app/store/account/account.actions';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  public forgotPassword: boolean;
  public showPasswords: boolean;
  loginForm = new FormGroup({
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
  forgotPasswordForm = new FormGroup({
    userName: new FormControl('', [
      Validators.required
    ])
  })
  public reseted: boolean;
  get userName() { return this.loginForm.get('user').get('userName') }
  get password() { return this.loginForm.get('user').get('password') }
  get userNameRecovery() { return this.forgotPasswordForm.get('userName') }
  constructor(private store: Store) {
    this.forgotPassword = false;
    this.reseted = false;
    this.showPasswords = false;
  }

  ngOnInit(): void {
  }

  onSubmit() {
    this.store.dispatch(login({ ...this.loginForm.value }))
  }
  onForgotPassword() {
    this.forgotPassword = !this.forgotPassword;
  }
  onRecoverySubmit() {
    console.log({ ...this.forgotPasswordForm.value });
    this.store.dispatch(forgotPassword({ ...this.forgotPasswordForm.value }))
    this.reseted = true;
  }
  showPassword() {
    this.showPasswords = !this.showPasswords;
  }
}
