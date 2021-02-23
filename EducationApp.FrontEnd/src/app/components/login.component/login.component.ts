import { Component, OnInit } from '@angular/core';
import {FormControl, FormGroup, Validators} from '@angular/forms';
import {login} from '../../store/account/account.actions';
import {select, Store} from '@ngrx/store';
import {LoginResult} from '../../models/account.models';
import { Observable } from 'rxjs';
import {AuthState} from '../../store/account/account.state';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginForm = new FormGroup({
    user: new FormGroup({
      userName: new FormControl('', [
        Validators.required
      ]),
      password: new FormControl('',[
        Validators.required
      ])
    }),
    rememberMe: new FormControl(false)
  })
  tokens$: Observable<LoginResult>;

  constructor(private store: Store<{tokens: AuthState}>) { 
    this.tokens$ = store.pipe(select('tokens'));
  }

  ngOnInit(): void {
  }

  onSubmit() {
    this.store.dispatch(login({...this.loginForm.value}))
  }

}
