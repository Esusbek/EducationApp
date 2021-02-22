import { Component, OnInit } from '@angular/core';
import {FormControl, FormGroup} from '@angular/forms';
import {login} from './login.store/login.actions';
import {select, Store} from '@ngrx/store';
import {LoginResult} from '../account.models';
import { Observable } from 'rxjs';
import {State} from './login.store/login.reducer';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginForm = new FormGroup({
    user: new FormGroup({
      userName: new FormControl(''),
      password: new FormControl('')
    }),
    rememberMe: new FormControl('')
  })
  tokens$: Observable<LoginResult>;

  constructor(private store: Store<{tokens: State}>) { 
    this.tokens$ = store.pipe(select('tokens'));
  }

  ngOnInit(): void {
  }

  onSubmit() {
    console.log(this.loginForm.value);
    this.store.dispatch(login({...this.loginForm.value}))
  }

}
