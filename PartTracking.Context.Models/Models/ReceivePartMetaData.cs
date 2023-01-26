using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PartTracking.Context.Models.Models
{

    // [ModelMetadataType(typeof(ReceivePartMetaData))] @ ReceivePart
    public class ReceivePartMetaData
    {
        [Required(ErrorMessage = "Order Reference is Required!")]
        public int OrderMasterId { get; set; }
        [Required(ErrorMessage = "Part is Required!")]
        public int PartMasterId { get; set; }
        [Required(ErrorMessage = "Receive Quantity is Required!")]
        public int ReceiveQuantity { get; set; }
        [Required(ErrorMessage = "Receive Date is Required!")]
        public DateTime ReceiveDate { get; set; }

    }
}
