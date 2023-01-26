using System;
using System.Collections.Generic;

namespace PartTracking.Context.Models.Models
{
    public partial class WorkOrder
    {
        public int Woid { get; set; }
        public int WorkOrderId { get; set; }
        public int CustomerOrderId { get; set; }
        public int PartMasterId { get; set; }
        public int PartQuantityRequired { get; set; }
        public int? PartQuantityPulled { get; set; }
        public int? BalanceAfterPull { get; set; }

        public virtual CustomerOrder CustomerOrder { get; set; }
        public virtual PartMaster PartMaster { get; set; }
    }
}
