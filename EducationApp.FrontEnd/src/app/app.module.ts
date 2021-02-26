import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms';
import {RouterModule} from '@angular/router';
import { FlexLayoutModule } from '@angular/flex-layout';

import { StoreModule } from '@ngrx/store';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { EffectsModule } from '@ngrx/effects';

import {JwtModule} from '@auth0/angular-jwt';

import {AppRoutingModule} from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginModule } from './components/login/login.module';
import { AccountReducer } from './store/reducers';
import { AccountEffects } from './store/effects';
import { LayoutModule} from './layout/layout.module';
import {RegisterModule} from './components/register/register.module';
import {EmailconfirmModule} from './components/emailconfirm/emailconfirm.module';
import {EmailactivatedModule} from './components/emailactivated/emailactivated.module';
import {ForgotpasswordModule} from './components/forgotpassword/forgotpassword.module';
import {AuthGuard} from './guards/auth-guard.guard';
import { environment } from '../environments/environment';
import { ResetpasswordComponent } from './components/resetpassword/resetpassword.component';


@NgModule({
  declarations: [
    AppComponent,
    ResetpasswordComponent,
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    ReactiveFormsModule,
    StoreModule.forRoot({ tokens: AccountReducer }),
    StoreDevtoolsModule.instrument({
      maxAge: 25,
      logOnly: environment.production,
    }),
    EffectsModule.forRoot([AccountEffects]),
    JwtModule.forRoot({
      config: {
        authScheme: 'Bearer',
        headerName: 'Authorization',
        tokenGetter: () => {
          return localStorage.getItem('accessToken');
        },
      }
    }),
    RouterModule.forRoot([]),
    AppRoutingModule,
    FlexLayoutModule,
    LayoutModule,
    LoginModule, 
    RegisterModule,
    EmailconfirmModule,
    EmailactivatedModule,
    ForgotpasswordModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
