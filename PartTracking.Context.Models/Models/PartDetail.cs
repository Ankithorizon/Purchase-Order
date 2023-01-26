using System;
using System.Collections.Generic;

namespace PartTracking.Context.Models.Models
{
    public partial class PartDetail
    {
        public int PartDetailId { get; set; }
        public int PartMasterId { get; set; }
        public string PartDesc { get; set; }
        public string PartDrgFile { get; set; }

        public virtual PartMaster PartMaster { get; set; }
    }
}
