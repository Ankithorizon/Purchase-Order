using System;
using System.Collections.Generic;

namespace PartTracking.Context.Models.Models
{
    public partial class PartWorkOrder
    {
        public int PartWorkOrderId { get; set; }
        public int PartId { get; set; }
        public int WorkOrderId { get; set; }
        public int PartQuantityPulled { get; set; }
        public DateTime? PulledDate { get; set; }

        public virtual PartMaster Part { get; set; }
    }
}
