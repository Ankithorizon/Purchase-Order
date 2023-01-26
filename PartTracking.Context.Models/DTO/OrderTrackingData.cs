using System;
using System.Collections.Generic;
using System.Text;

namespace PartTracking.Context.Models.DTO
{
    public class OrderTrackingData
    {
        public int OrderMasterId { get; set; }
        public int OrderQuantity { get; set; }
        public DateTime OrderDate { get; set; }
        public int OrderStatus { get; set; }
        public string RefCode { get; set; }
        public int ReceivedQuantity { get; set; }
        public List<ReceivingTrackingData> ReceivingData { get; set; }
    }
}
