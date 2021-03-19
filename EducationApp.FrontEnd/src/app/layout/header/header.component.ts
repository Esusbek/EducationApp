import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { AccountService } from 'src/app/services/account.service';
import { CartState } from 'src/app/store/cart/cart.state';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {
  public cart$: CartState;
  constructor(private accountService: AccountService, private store: Store<{ Cart: CartState }>) {
    store.select('Cart').subscribe(value => {
      this.cart$ = value
    });
  }

  ngOnInit(): void {
  }
  public itemsInCart(): boolean {
    return this.cart$.currentItems.length > 0;
  }
  public isLoggedIn(): boolean {
    return this.accountService.isAuthenticated();
  }
  public Logout(): void {
    this.accountService.logout();
  }
}
