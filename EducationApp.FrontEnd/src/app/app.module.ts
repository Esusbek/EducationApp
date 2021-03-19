
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
import { AppRoutingModule } from 'src/app/app-routing.module';
import { AppComponent } from 'src/app/app.component';
import { BookListModule } from 'src/app/components/book-list/book-list.module';
import { EmailactivatedModule } from 'src/app/components/emailactivated/emailactivated.module';
import { LoginModule } from 'src/app/components/login/login.module';
import { ProfileModule } from 'src/app/components/profile/profile.module';
import { RegisterModule } from 'src/app/components/register/register.module';
import { ResetPasswordModule } from 'src/app/components/resetpassword/resetpassword.module';
import { httpInterceptorProviders } from 'src/app/interceptors/interceptors';
import { LayoutModule } from 'src/app/layout/layout.module';
import { AccountEffects, CartEffects, PrintingEditionEffects, ProfileEffects } from 'src/app/store/effects';
import { AccountReducer, CartReducer, PrintingEditionReducer, ProfileReducer } from 'src/app/store/reducers';
import { environment } from 'src/environments/environment';



@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    ReactiveFormsModule,
    StoreModule.forRoot({ Account: AccountReducer, Profile: ProfileReducer, Books: PrintingEditionReducer, Cart: CartReducer }),
    StoreDevtoolsModule.instrument({
      maxAge: 25,
      logOnly: environment.production,
    }),
    EffectsModule.forRoot([AccountEffects, ProfileEffects, PrintingEditionEffects, CartEffects]),
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
