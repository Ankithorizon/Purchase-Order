using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PartTracking.Context.Models.Models
{
    // [ModelMetadataType(typeof(PartMasterMetaData))] @ PartMaster
    public class PartMasterMetaData
    {
        [Required(ErrorMessage = "Part Code is Required!")]
        [StringLength(20, ErrorMessage = "Maximum 20 Characters Allowed!")]
        public string PartCode { get; set; }
        [Required(ErrorMessage = "Part Name is Required!")]
        public string PartName { get; set; }
    }
}
