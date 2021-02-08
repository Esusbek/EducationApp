using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.DataAccessLayer.Entities.Enums
{
    public partial class Enums
    {
        public enum Currency
        {
            USD = 0,
            EUR = 1,
            GBP = 2, 
            CHF = 3, 
            JPY = 4, 
            UAH = 5
        }
        public partial class PrintingEdition
        {
            public enum Status
            {
                InStock = 0,
                SoldOut = 1
            }
            public enum Type
            {
                Book = 0,
                Journal = 1,
                Newspaper = 2
            }
        }

        public partial class Order
        {
            public enum Status
            {
                Unpaid = 0,
                Paid = 1
            }
        }

        public partial class User
        {
            public enum Roles
            {
                Admin = 0,
                Client = 1
            }
        }
    }
}
