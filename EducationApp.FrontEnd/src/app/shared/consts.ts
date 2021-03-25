import { HttpHeaders } from "@angular/common/http";

export const HttpOptions = {
  headers: new HttpHeaders({
    'Content-Type': 'application/json',
    'Accept': 'application/json',
    'Access-Control-Allow-Headers': 'Content-Type'
  })
};

export const Urls = {
  LoginURL: 'Account/Login',
  RegisterURL: 'Account/Register',
  ActivationURL: 'Account/ConfirmEmail',
  ForgotPasswordURL: 'Account/ForgotPassword',
  ResetPasswordURL: 'Account/ResetPassword',
  getUserURL: 'Account/GetUserInfo',
  changePasswordURL: 'Account/ChangePassword',
  updateUserURL: 'Account/UpdateUser',
  refreshTokenURL: 'Account/RefreshToken',
  logoutURL: 'Account/Logout',
  dummyURL: 'Account/DummyRequest',
  getBookURL: 'PrintingEdition/GetFiltered',
  getEditionInfoURL: 'PrintingEdition/GetInfo',
  getLastPageURL: 'PrintingEdition/GetPage',
  checkoutURL: 'Order/Checkout',
  getOrdersURL: 'Order/Get',
  payOrderURL: 'Order/Success'
}
