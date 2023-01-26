using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PartTracking.Context.Models.DTO
{
    public class ReceivePartView
    {
        public int ReceivePartId { get; set; }
        public int OrderMasterId { get; set; }
        public int PartMasterId { get; set; }
        [Required(ErrorMessage = "Receive Quantity is Required!")]
        public int ReceiveQuantity { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime ReceiveDate { get; set; }
        public int OrderQuantity { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime OrderDate { get; set; }
        [Required(ErrorMessage = "Reference Code is Required!")]
        public string RefCode  { get; set; }
        public int OrderStatus { get; set; }

        public string Part { get; set; }
    }
}
