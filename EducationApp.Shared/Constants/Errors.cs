using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.Shared.Constants
{
    public partial class Constants
    {
        public partial class Errors
        {
            public const string FailedLogin = "Неверный логин или пароль";
            public const string IncorrectInput = "Incorrect input data";
            public const string FailedToCreate = "Error during user creation";
            public const string FailedToReset = "Error password reset";
            public const string UserNotFound = "User doesnt exist";
            public const string AuthorNotFound = "Author doesnt exist";
            public const string InvalidToken = "Incorrect token";
            public const string Unathorized = "Unauthorized";
        }
    }
}
