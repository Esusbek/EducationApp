import { HttpHeaders } from "@angular/common/http";

export const HttpOptions = {
    headers: new HttpHeaders({
      'Content-Type':  'application/json',
      'Accept': 'application/json',
      'Access-Control-Allow-Headers': 'Content-Type'
    })
  };
