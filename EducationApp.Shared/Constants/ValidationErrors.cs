namespace EducationApp.Shared.Constants
{
    public partial class Constants
    {
        public partial class ValidationErrors
        {
            public const string InvalidName = "Имя и фамилия должны состоять только из букв";
            public const string InvalidUsername = "Логин должен состоять только из букв или цифр";
            public const string InvalidEmail = "Невалидная электронная почта";
            public const string HasBannedWords = "содержит запрещенные слова. попробуйте еще раз";
            public const string PasswordDoNotMatch = "2 пароля не совпадают";
        }
    }
}
