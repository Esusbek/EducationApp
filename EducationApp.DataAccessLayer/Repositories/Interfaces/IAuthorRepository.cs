using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.DataAccessLayer.Repositories.Interfaces
{
    public interface IAuthorRepository<T>:IBaseRepository<T> where T: AuthorEntity
    {
        AuthorEntity GetById(int id);
        void Insert(AuthorEntity entity);
    }
}
