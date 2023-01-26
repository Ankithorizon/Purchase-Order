using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PartTracking.Context.Models.DTO
{
    public class WarehouseOrderView
    {
        public int OrderMasterId { get; set; }
        public int PartMasterId { get; set; }
        public int? OrderQuantity { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? OrderDate { get; set; }
        public int? OrderStatus { get; set; }
        public string RefCode { get; set; }
        public string PartCode { get; set; }
        public string PartName { get; set; }
    }
}
