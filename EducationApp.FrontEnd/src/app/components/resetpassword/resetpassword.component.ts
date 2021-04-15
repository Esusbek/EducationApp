import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Store } from '@ngxs/store';
import { resetPassword } from 'src/app/store-ngxs/account/account.actions';
import { passwordMatchValidator } from 'src/app/validators/password-match.directive';

@Component({
  selector: 'app-resetpassword',
  templateUrl: './resetpassword.component.html',
  styleUrls: ['./resetpassword.component.css']
})
export class ResetPasswordComponent implements OnInit {
  private code: string;
  private id: string;
  public resetPasswordForm = new FormGroup({
    password: new FormControl('', [
      Validators.required
    ]),
    passwordConfirm: new FormControl('', [
      Validators.required
    ])
  }, { validators: passwordMatchValidator })
  public constructor(private route: ActivatedRoute, private store: Store) { }
  public get password() { return this.resetPasswordForm.get('password') }
  public get passwordConfirm() { return this.resetPasswordForm.get('passwordConfirm') }
  public ngOnInit(): void {
    this.route.queryParamMap
      .subscribe((params) => {
        this.code = params.get('code');
        this.id = params.get('userId');
      })
  }
  public onSubmit() {
    this.store.dispatch(new resetPassword({ code: this.code, userId: this.id, password: this.resetPasswordForm.value.password }));
  }
}
