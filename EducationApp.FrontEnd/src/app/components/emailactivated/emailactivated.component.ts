import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Store } from '@ngrx/store';
import {activateEmail} from '../../store/account/account.actions';
import {EmailActivationModel} from '../../models/account.models';

@Component({
  selector: 'app-emailactivated',
  templateUrl: './emailactivated.component.html',
  styleUrls: ['./emailactivated.component.css']
})
export class EmailactivatedComponent implements OnInit {
  private activationModel: EmailActivationModel;
  constructor(private route: ActivatedRoute,private store: Store) { }

  ngOnInit(): void {
    this.route.queryParamMap
      .subscribe((params) => {
        this.activationModel={code: params.get('code'), userId: params.get('userId')};
      })
    debugger;
    this.store.dispatch(activateEmail({payload: this.activationModel}));
  }

}
