using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace PartTracking.Context.Models.Models
{
    [ModelMetadataType(typeof(ReceivePartMetaData))]
    public partial class ReceivePart
    {
        public int ReceivePartId { get; set; }
        public int OrderMasterId { get; set; }
        public int PartMasterId { get; set; }
        public int ReceiveQuantity { get; set; }
        public DateTime ReceiveDate { get; set; }

        public virtual OrderMaster OrderMaster { get; set; }
        public virtual PartMaster PartMaster { get; set; }
    }
}
