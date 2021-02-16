namespace EducationApp.Shared.Constants
{
    public partial class Constants
    {
        public partial class Validators
        {
            public const string EmailValidator = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
            public const string UsernameValidator = @"^[a-zA-Z0-9]+$";
            public const string NameValidator = @"^[а-яА-Яa-zA-Z]+$";
            public const string BannedWords = "Admin";
            public const string PrintingEditionFieldNames = "Title,Description,Price,Status,Currency,Type";
        }
    }
}
