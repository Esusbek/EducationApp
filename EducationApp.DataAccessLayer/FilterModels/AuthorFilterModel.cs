using System.Collections.Generic;

namespace EducationApp.DataAccessLayer.FilterModels
{
    public class AuthorFilterModel
    {
        public string Name { get; set; }
        public List<string> EditionAuthors { get; set; }
        public AuthorFilterModel()
        {
            Name = "";
            EditionAuthors = new List<string>();
        }
    }
}
