using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PartTracking.Context.Models.Models
{

    // [ModelMetadataType(typeof(OrderMasterMetaData))] @ OrderMaster
    public class OrderMasterMetaData
    {
        [Required(ErrorMessage = "Part is Required!")]
        public int PartMasterId { get; set; }

        [Required(ErrorMessage = "Order Quantity is Required!")]
        public int OrderQuantity { get; set; }      
        public DateTime? OrderDate { get; set; }
    }
}
