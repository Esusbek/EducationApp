import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Store } from '@ngrx/store';
import { UserModel } from 'src/app/models/profile.models';
import { AccountService } from 'src/app/services/account.service';
import { getInfo } from 'src/app/store/profile/profile.actions';
import { ProfileState } from 'src/app/store/profile/profile.state';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  public isEditing$: boolean = false;
  public profile$: UserModel;
  profileForm = new FormGroup({
    userName: new FormControl({value: '', disabled: true}, [
      Validators.required
    ]),
    firstName: new FormControl({value: '', disabled: !this.isEditing$}, [
      Validators.required
    ]),
    lastName: new FormControl({value: '', disabled: !this.isEditing$}, [
      Validators.required
    ]),
    email: new FormControl({value: '', disabled: !this.isEditing$}, [
      Validators.required,
      Validators.email
    ])
  })
  constructor(private store: Store<{Profile: ProfileState}>, private auth: AccountService) { 
    store.select('Profile').subscribe(val=>this.profile$=val.user);
    store.select('Profile').subscribe(val=>this.profileForm.controls['userName'].setValue(val.user.userName));
    store.select('Profile').subscribe(val=>this.profileForm.controls['firstName'].setValue(val.user.firstName));
    store.select('Profile').subscribe(val=>this.profileForm.controls['lastName'].setValue(val.user.lastName));
    store.select('Profile').subscribe(val=>this.profileForm.controls['email'].setValue(val.user.email));
    this.isEditing$ = false;
  }

  ngOnInit(): void {
    this.store.dispatch(getInfo());
    
  }
  onSubmit() {
  }
}
