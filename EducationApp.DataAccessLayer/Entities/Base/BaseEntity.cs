using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.DataAccessLayer.Entities.Base
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; protected set; }
        public bool IsRemoved { get; set; }

        public BaseEntity()
        {
            CreatedAt = DateTime.Now;
        }
    }
}
