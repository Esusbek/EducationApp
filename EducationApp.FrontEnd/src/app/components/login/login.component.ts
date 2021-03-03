import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { forgotPassword, login } from '../../store/account/account.actions';
import { Store } from '@ngrx/store';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  public forgotPassword: boolean;
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
  reseted: boolean;

  constructor(private store: Store) {
    this.forgotPassword = false;
    this.reseted= false;
  }

  ngOnInit(): void {
  }

  onSubmit() {
    this.store.dispatch(login({ ...this.loginForm.value }))
  }
  onForgotPassword() {
    this.forgotPassword = true;
  }
  onLoginReturn() {
    this.forgotPassword = false;
  }
  onRecoverySubmit() {
    console.log({...this.forgotPasswordForm.value});
    this.store.dispatch(forgotPassword({...this.forgotPasswordForm.value}))
    this.reseted=true;
  }
}
