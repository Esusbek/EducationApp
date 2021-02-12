using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace EducationApp.Shared.Exceptions
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
