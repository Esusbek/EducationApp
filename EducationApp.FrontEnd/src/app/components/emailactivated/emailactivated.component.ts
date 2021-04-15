import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Store } from '@ngxs/store';
import { EmailActivationModel } from 'src/app/models/account.models';
import { activateEmail } from 'src/app/store-ngxs/account/account.actions';

@Component({
  selector: 'app-emailactivated',
  templateUrl: './emailactivated.component.html',
  styleUrls: ['./emailactivated.component.css']
})
export class EmailactivatedComponent implements OnInit {
  private activationModel: EmailActivationModel;
  public constructor(private route: ActivatedRoute, private store: Store) { }

  public ngOnInit(): void {
    this.route.queryParamMap
      .subscribe((params) => {
        this.activationModel = { code: params.get('code'), userId: params.get('userId') };
      })
    this.store.dispatch(new activateEmail(this.activationModel));
  }

}
