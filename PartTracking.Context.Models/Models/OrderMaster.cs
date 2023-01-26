using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace PartTracking.Context.Models.Models
{
    [ModelMetadataType(typeof(OrderMasterMetaData))]
    public partial class OrderMaster
    {
        public OrderMaster()
        {
            ReceivePart = new HashSet<ReceivePart>();
        }

        public int OrderMasterId { get; set; }
        public int PartMasterId { get; set; }
        public int? OrderQuantity { get; set; }
        // public DateTime? OrderDate { get; set; }
        public DateTime OrderDate { get; set; }
        public int? OrderStatus { get; set; }
        public string RefCode { get; set; }

        public virtual PartMaster PartMaster { get; set; }
        public virtual ICollection<ReceivePart> ReceivePart { get; set; }
    }
}
