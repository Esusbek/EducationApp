namespace EducationApp.BusinessLogicLayer.Models.Requests
{
    public class AuthorAndEditionRequestModel
    {
        public Authors.AuthorModel Author { get; set; }
        public PrintingEditions.PrintingEditionModel PrintingEdition { get; set; }
    }
}
