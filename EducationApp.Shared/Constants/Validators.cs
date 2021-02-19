namespace EducationApp.Shared.Constants
{
    public partial class Constants
    {
        public const string EMAILVALIDATOR = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
        public const string USERNAMEVALIDATOR = @"^[a-zA-Z0-9]+$";
        public const string NAMEVALIDATOR = @"^[а-яА-Яa-zA-Z]+$";
        public const string BANNEDWORDSVALIDATOR = "Admin";
    }
}
