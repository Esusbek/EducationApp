namespace EducationApp.Shared.Constants
{
    public partial class Constants
    {
        public const string DEFAULTCURRENCY = "USD";
        public const int DEFAULTPAGE = 1;
        public const string DEFAULTPAYMENTMETHOD = "card";
        public const string DEFAULTPAYMENTMODE = "payment";
        public const string DEFAULTJOINTTABLENAME = "AuthorInPrintingEditions";
        public const string DEFAULTEMAILCONFIRMATION = "Email confirmation";
        public const string DEFAULTEMAILCONFIRMATIONRESPONSE = "Для завершения регистрации проверьте электронную почту и перейдите по ссылке, указанной в письме";
        public const string DEFAULTEMAILCONFIRMATIONBODY = "Подтвердите регистрацию, перейдя по ссылке: <a href='{0}'>link</a>";
        public const string DEFAULTPASSWORDRESET = "Password reset request";
        public const string DEFAULTPASSWORDRESETRESPONSE = "Для сброса пароля проверьте электронную почту и перейдите по ссылке, указанной в письме";
        public const string DEFAULTPASSWORDRESETBODY = "Произведите сброс пароля, перейдя по ссылке: <a href='{0}'>link</a>";
        public const int DEFAULTDAYSPERRATEREFRESH = 1;
        public const int DEFAULTPREVIOUSPAGEOFFSET = 1;
        public const int DEFAULTMINPASSWORDLENGTH = 6;
        public const int DEFAULTREFRESHTOKENLENGTH = 32;
        public const string DEFAULTSORT = "Id";
        public const string ALLOWSPECIFICORIGINS = "AllowSpecificOrigins";
        public const string DEFAULTORDERSORT = "Date";
        public const string DEFAULTAUTHORSORT = "Id";
        public const string DEFAULTEDITIONSORT = "Price";
        public const string DEFAULTFRONTENDORIGIN = "http://localhost:4200";
        public const string USERNAMECLAIMNAME = "username";
        public const string IDCLAIMNAME = "id";
        public const string ROLECLAIMNAME = "role";
        public const string MAPPROFILEASSEMBLYNAME = "EducationApp.BusinessLogicLayer";
        public const int ORDERBYPARAMCOUNT = 2;
        public const string DEFAULTSORTORDER = "asc";
        public const int CENTMULTIPLIER = 100;
        public const string USERIDKEY = "userId";
        public const string CODEKEY = "code";
        public const string FIRSTNAMECLAINNAME = "given_name";
        public const string LASTNAMECLAINNAME = "family_name";
        public const string EMAILCLAINNAME = "email";
        public const string EMAILSEPARATOR = "@";
        public const string ALPHANUMERICCHARACTERS = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        public const int DEFAULTPASSWORDLENGTH = 8;
        public const string DEFAULTPASSWORDGENERATED = "Thank you for signing up";
        public const string DEFAULTPASSWORDGENERATEDBODY = "<p>Thank you for signing up on Book Publishing Company site. Below are your credentials you can use to login without google account if you wish</p><p>Username: {0}</p><p>Password: {1}</p>";
    }
}
