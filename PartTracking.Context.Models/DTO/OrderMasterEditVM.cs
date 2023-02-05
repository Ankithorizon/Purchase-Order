using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PartTracking.Context.Models.DTO
{
    public class OrderMasterEditVM
    {
        [Required(ErrorMessage = "Order# is Required!")]
        public int OrderMasterId { get; set; }
        [Required(ErrorMessage = "Part is Required!")]
        public int PartMasterId { get; set; }
        [Required(ErrorMessage = "Part Quantity is Required!")]
        public int OrderQuantity { get; set; }
        public List<SelectListItem> PartMasterSelectList { get; set; }
    }
}
