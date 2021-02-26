import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Store } from '@ngrx/store';
import { forgotPassword } from 'src/app/store/account/account.actions';

@Component({
  selector: 'app-forgotpassword',
  templateUrl: './forgotpassword.component.html',
  styleUrls: ['./forgotpassword.component.css']
})
export class ForgotpasswordComponent implements OnInit {
  forgotPasswordForm = new FormGroup({
      userName: new FormControl('')
  })
  reseted: boolean;
  constructor(private store: Store) { 
    this.reseted= false;
  }

  ngOnInit(): void {
  }
  onSubmit() {
    this.store.dispatch(forgotPassword({...this.forgotPasswordForm.value}))
    this.reseted=true;
  }

}
