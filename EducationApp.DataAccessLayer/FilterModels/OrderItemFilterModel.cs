using System.Collections.Generic;

namespace EducationApp.DataAccessLayer.FilterModels
{
    public class OrderItemFilterModel
    {
        public List<int> OrderIds { get; set; }
        public OrderItemFilterModel()
        {
            OrderIds = new List<int>();
        }
    }
}
