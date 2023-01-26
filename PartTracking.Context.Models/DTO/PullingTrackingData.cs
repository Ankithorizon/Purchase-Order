using System;
using System.Collections.Generic;
using System.Text;

namespace PartTracking.Context.Models.DTO
{
    public class PullingTrackingData
    {
        public int PartWorkOrderId { get; set; }
        public int PullingQuantity { get; set; }
        public DateTime PullingDate { get; set; }
    }
}
