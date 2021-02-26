import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Store } from '@ngrx/store';
import {register} from '../../store/account/account.actions';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
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
    passwordConfirm: new FormControl('',[
      Validators.required,
      Validators.minLength(6)
    ])
  })
  constructor(private store: Store) {}

  ngOnInit(): void {
  }
  onSubmit() {
    this.store.dispatch(register({user: this.registerForm.value}))
  }

}
