using System;
using System.Collections.Generic;

namespace PartTracking.Context.Models.Models
{
    public partial class CustomerOrder
    {
        public CustomerOrder()
        {
            WorkOrder = new HashSet<WorkOrder>();
        }

        public int CustomerOrderId { get; set; }
        public int CustomerId { get; set; }
        public string ProductName { get; set; }
        public int OrderQuantity { get; set; }
        public string ProductDrawing { get; set; }

        public virtual ICollection<WorkOrder> WorkOrder { get; set; }
    }
}
