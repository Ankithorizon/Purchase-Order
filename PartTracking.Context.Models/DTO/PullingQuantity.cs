using System;
using System.Collections.Generic;
using System.Text;

namespace PartTracking.Context.Models.DTO
{
    public class PullingQuantity
    {
        public int WOId { get; set; } 
        public int WorkOrderId { get; set; }
        public int CustomerOrderId { get; set; }
        public int PartMasterId { get; set; }
        public int PartQuantityRequired { get; set; }
        public int PartQuantityPulled { get; set; }
        public int PartQuantityAtWarehouse { get; set; }
        public int BalanceAfterPull { get; set; } 
    }
}
