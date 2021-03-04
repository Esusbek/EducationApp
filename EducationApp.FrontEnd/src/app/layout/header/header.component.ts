import { Component, OnInit } from '@angular/core';
import { AccountService } from 'src/app/services/account.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {
  constructor(private accountService: AccountService) { }

  ngOnInit(): void {
  }
  public isLoggedIn(): boolean {
    return this.accountService.isAuthenticated();
  }
  public Logout(): void {
    this.accountService.logout();
  }
}
