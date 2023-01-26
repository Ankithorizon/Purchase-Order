using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PartTracking.Context.Models.DTO
{
    public class PartMasterPartDetailsAddVM
    {
        [Required(ErrorMessage = "Part Code is Required!")]
        [Remote(action: "VerifyPartCode", controller: "Engineering")]
        public string PartCode { get; set; }
        [Required(ErrorMessage = "Part Name is Required!")]
        public string PartName { get; set; }
        [Required(ErrorMessage = "Part Description is Required!")]
        public string PartDesc { get; set; }        
        public string PartDrgFile { get; set; }
    }
}
