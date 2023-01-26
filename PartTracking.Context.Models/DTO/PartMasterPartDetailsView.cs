using System;
using System.Collections.Generic;
using System.Text;

namespace PartTracking.Context.Models.DTO
{
    public class PartMasterPartDetailsView
    {
        public int PartMasterId { get; set; }
        public string PartCode { get; set; }
        public string PartName { get; set; }
        public int? Quantity { get; set; }
        public int PartDetailId { get; set; }
        public string PartDesc { get; set; }
        public string PartDrgFile { get; set; }

    }
}
