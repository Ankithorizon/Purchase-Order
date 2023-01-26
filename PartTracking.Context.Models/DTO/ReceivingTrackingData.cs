using System;
using System.Collections.Generic;
using System.Text;

namespace PartTracking.Context.Models.DTO
{
    public class ReceivingTrackingData
    {
        public int ReceivePartId { get; set; }
        public int ReceiveQuantity { get; set; }
        public DateTime ReceiveDate { get; set; }
    }
}
