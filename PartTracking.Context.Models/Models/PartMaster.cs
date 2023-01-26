using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace PartTracking.Context.Models.Models
{
    [ModelMetadataType(typeof(PartMasterMetaData))]
    public partial class PartMaster
    {
        public PartMaster()
        {
            OrderMaster = new HashSet<OrderMaster>();
            PartWorkOrder = new HashSet<PartWorkOrder>();
            ReceivePart = new HashSet<ReceivePart>();
            WorkOrder = new HashSet<WorkOrder>();
        }

        public int PartMasterId { get; set; }
        public string PartCode { get; set; }
        public string PartName { get; set; }
        public int? Quantity { get; set; }

        public virtual PartDetail PartDetail { get; set; }
        public virtual ICollection<OrderMaster> OrderMaster { get; set; }
        public virtual ICollection<PartWorkOrder> PartWorkOrder { get; set; }
        public virtual ICollection<ReceivePart> ReceivePart { get; set; }
        public virtual ICollection<WorkOrder> WorkOrder { get; set; }
    }
}
