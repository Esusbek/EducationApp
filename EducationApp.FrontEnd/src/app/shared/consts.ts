import { HttpHeaders } from "@angular/common/http";

export const HttpOptions = {
  headers: new HttpHeaders({
    'Content-Type': 'application/json',
    'Accept': 'application/json',
    'Access-Control-Allow-Headers': 'Content-Type'
  })
};
export const HttpFormOptions = {
  headers: new HttpHeaders({
    'Content-Type': 'application/x-www-form-urlencoded',
    'Accept': 'application/json',
    'Access-Control-Allow-Headers': 'Content-Type'
  })
};
export const Urls = {
  loginURL: 'Account/Login',
  registerURL: 'Account/Register',
  activationURL: 'Account/ConfirmEmail',
  forgotPasswordURL: 'Account/ForgotPassword',
  resetPasswordURL: 'Account/ResetPassword',
  getUserURL: 'Account/GetUserInfo',
  changePasswordURL: 'Account/ChangePassword',
  updateUserURL: 'Account/UpdateUser',
  refreshTokenURL: 'Account/RefreshToken',
  logoutURL: 'Account/Logout',
  getBookURL: 'PrintingEdition/GetFiltered',
  getEditionInfoURL: 'PrintingEdition/GetInfo',
  getLastPageURL: 'PrintingEdition/GetPage',
  checkoutURL: 'Order/Checkout',
  getOrdersURL: 'Order/Get',
  payOrderURL: 'Order/Success',
  googleLoginURL: 'Account/GoogleLogin'
}

export const Defaults = {
  pageOffset: 1,
  defaultPage: 1,
  maxQuantity: 10,
  startQuantity: 1
}
