using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PartTracking.Context.Models.DTO
{
    public class PartMasterPartDetailsEditVM
    {        
        public int PartMasterId { get; set; }
        public int PartDetailId { get; set; }
        public int? Quantity { get; set; }

        [Required(ErrorMessage = "Part Code is Required!")]
        [Remote(action: "VerifyPartCodeEditOperation", controller: "Engineering",
            AdditionalFields = "PreviousPartCode")]
        [StringLength(20, ErrorMessage = "Maximum 20 Characters Allowed!")]
        public string PartCode { get; set; }
        [Required(ErrorMessage = "Part Name is Required!")]
        public string PartName { get; set; }
        [Required(ErrorMessage = "Part Description is Required!")]
        public string PartDesc { get; set; }
        public string PartDrgFile { get; set; }
        public string PreviousPartDrgFile { get; set; }
        // previous partcode is ok, in case user doesn't want to change part code
        public string PreviousPartCode { get; set; }
    }
}
