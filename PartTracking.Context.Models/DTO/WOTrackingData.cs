using System;
using System.Collections.Generic;
using System.Text;

namespace PartTracking.Context.Models.DTO
{
    public class WOTrackingData
    {
        public int WOId { get; set; }
        public int WorkOrderId { get; set; }
        public int  WorkOrderQuantity { get; set; }
        public int PulledQuantity { get; set; }
        public List<PullingTrackingData> PullingData { get; set; }
    }
}
