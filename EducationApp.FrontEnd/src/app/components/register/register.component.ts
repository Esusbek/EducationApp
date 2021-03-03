import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { createSelector, select, Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { AccountState } from 'src/app/store/account/account.state';
import { AppState } from 'src/app/store/state';
import {register} from '../../store/account/account.actions';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  public isRegistered$: boolean;
  registerForm = new FormGroup({
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
    ]),
    password: new FormControl('',[
      Validators.required,
      Validators.minLength(6)
    ]),
    confirmPassword: new FormControl('',[
      Validators.required,
      Validators.minLength(6)
    ])
  })
  constructor(private store: Store<{Account: AccountState}>) {
      store.select('Account').subscribe(val=>this.isRegistered$=val.isRegistered);
    }

  ngOnInit(): void {
  }
  onSubmit() {
    this.store.dispatch(register({user: this.registerForm.value}));
  }
}
