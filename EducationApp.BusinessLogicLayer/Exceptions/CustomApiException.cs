﻿using System;
using System.Net;

namespace EducationApp.BusinessLogicLayer.Exceptions
{
    public class CustomApiException : Exception
    {
        public HttpStatusCode Status { get; set; }
        public CustomApiException(HttpStatusCode code, string msg)
            : base(msg)
        {
            Status = code;
        }
    }
}
