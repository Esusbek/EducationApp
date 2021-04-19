import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Store } from '@ngxs/store';
import { CheckoutComponent } from 'src/app/components/checkout/checkout.component';
import { AccountService } from 'src/app/services/account.service';
import { CartState, CartStateModel } from 'src/app/store-ngxs/cart/cart.state';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {
  public cart: CartStateModel;
  constructor(private accountService: AccountService, private store: Store, private modalService: NgbModal) {
    this.store.select(CartState.cart).subscribe(value => this.cart = value);
  }

  ngOnInit(): void {
  }
  public itemsInCart(): boolean {
    return this.cart.currentItems.length > 0;
  }
  public isLoggedIn(): boolean {
    return this.accountService.isAuthenticated();
  }
  public open(): void {
    this.modalService.open(CheckoutComponent);
  }
  public Logout(): void {
    var auth2 = gapi.auth2.getAuthInstance();
    auth2.signOut().then(function () {
      console.log('User signed out.');
    });
    this.accountService.logout();
  }
}
