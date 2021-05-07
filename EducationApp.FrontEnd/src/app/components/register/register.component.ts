import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Store } from '@ngrx/store';
import { register } from 'src/app/store/account/account.actions';
import { AccountState } from 'src/app/store/account/account.state';
import { passwordMatchValidator } from 'src/app/validators/password-match.directive';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  public isRegistered: boolean;
  public registerForm = new FormGroup({
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
    password: new FormControl('', [
      Validators.required,
      Validators.minLength(6)
    ]),
    passwordConfirm: new FormControl('', [
      Validators.required,
      Validators.minLength(6)
    ])
  }, { validators: passwordMatchValidator })

  public get userName() { return this.registerForm.get('userName') }
  public get firstName() { return this.registerForm.get('firstName') }
  public get lastName() { return this.registerForm.get('lastName') }
  public get email() { return this.registerForm.get('email') }
  public get password() { return this.registerForm.get('password') }
  public get passwordConfirm() { return this.registerForm.get('passwordConfirm') }

  public constructor(private store: Store<{ Account: AccountState }>) {
    store.select('Account').subscribe(value => this.isRegistered = value.isRegistered);
  }

  public ngOnInit(): void {
  }
  public onSubmit() {
    this.store.dispatch(register({ user: this.registerForm.value }));
  }
}
