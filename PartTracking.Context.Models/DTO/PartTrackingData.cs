using System;
using System.Collections.Generic;
using System.Text;

namespace PartTracking.Context.Models.DTO
{
    public class PartTrackingData
    {
        public int PartMasterId { get; set; }
        public string Part { get; set; }
        public int Quantity { get; set; }
        public List<OrderTrackingData> Orders { get; set; }
        public List<WOTrackingData> WorkOrders { get; set; }
    }
}
