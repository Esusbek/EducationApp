import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Store } from '@ngrx/store';
import { resetPassword } from 'src/app/store/account/account.actions';
import { passwordMatchValidator } from 'src/app/validators/password-match.directive';

@Component({
  selector: 'app-resetpassword',
  templateUrl: './resetpassword.component.html',
  styleUrls: ['./resetpassword.component.css']
})
export class ResetPasswordComponent implements OnInit {
  code: string;
  id: string;
  resetPasswordForm = new FormGroup({
      password: new FormControl('', [
        Validators.required
      ]),
      confirmPassword: new FormControl('', [
        Validators.required
      ])
  }, {validators: passwordMatchValidator})
  constructor(private route: ActivatedRoute,private store: Store) { }

  ngOnInit(): void {
    this.route.queryParamMap
      .subscribe((params) => {
        this.code= params.get('code');
        this.id = params.get('userId');
      })
  }
  onSubmit() {
    this.store.dispatch(resetPassword({payload: {code: this.code, userId: this.id, password: this.resetPasswordForm.value.password}}));
  }
}
