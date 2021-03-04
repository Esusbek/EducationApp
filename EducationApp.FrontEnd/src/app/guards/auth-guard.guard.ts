import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { AccountService } from '../services/account.service';

@Injectable()
export class AuthGuard implements CanActivate {
  constructor(public auth: AccountService, public router: Router) {}
  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) : Observable<boolean> | boolean {
    //debugger;
    if (!localStorage.getItem('accessToken')) {
      this.router.navigate(['login']);
      return false;
    }
    return true;
  }
}