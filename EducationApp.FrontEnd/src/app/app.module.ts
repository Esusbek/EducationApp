
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';
import { JwtModule } from '@auth0/angular-jwt';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { environment } from '../environments/environment';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BookListModule } from './components/book-list/book-list.module';
import { EmailactivatedModule } from './components/emailactivated/emailactivated.module';
import { LoginModule } from './components/login/login.module';
import { ProfileModule } from './components/profile/profile.module';
import { RegisterModule } from './components/register/register.module';
import { ResetPasswordModule } from './components/resetpassword/resetpassword.module';
import { httpInterceptorProviders } from './interceptors/interceptors';
import { LayoutModule } from './layout/layout.module';
import { AccountEffects, PrintingEditionEffects, ProfileEffects } from './store/effects';
import { AccountReducer, PrintingEditionReducer, ProfileReducer } from './store/reducers';



@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    ReactiveFormsModule,
    StoreModule.forRoot({ Account: AccountReducer, Profile: ProfileReducer, Books: PrintingEditionReducer }),
    StoreDevtoolsModule.instrument({
      maxAge: 25,
      logOnly: environment.production,
    }),
    EffectsModule.forRoot([AccountEffects, ProfileEffects, PrintingEditionEffects]),
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
    EmailactivatedModule,
    ResetPasswordModule,
    ProfileModule,
    BookListModule
  ],
  providers: [
    httpInterceptorProviders
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
