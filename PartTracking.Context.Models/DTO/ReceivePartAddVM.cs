using System;
using System.Collections.Generic;
using System.Text;

namespace PartTracking.Context.Models.DTO
{
    public class ReceivePartAddVM
    {
        public int ReceivePartId { get; set; }
        public int OrderMasterId { get; set; }
        public int PartMasterId { get; set; }
        public int ReceiveQuantity { get; set; }
        public DateTime ReceiveDate { get; set; }
        public string RefCode { get; set; }
    }
}
