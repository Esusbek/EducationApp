namespace EducationApp.Shared.Enums
{
    public partial class Enums
    {
        public partial class PrintingEdition
        {
            public enum Status
            {
                NotAssigned = 0,
                InStock = 1,
                SoldOut = 2
            }
            public enum Type
            {
                NotAssigned = 0,
                Book = 1,
                Journal = 2,
                Newspaper = 3
            }
        }
    }
}
