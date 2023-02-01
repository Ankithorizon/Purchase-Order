using Microsoft.AspNetCore.Http;
using PartTracking.Context.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PurchaseOrderAPI.DTO
{
    public class PartEditDTO
    {
        public IFormFile PartFile { get; set; }
        public int PartMasterId { get; set; }
        public int PartDetailId { get; set; }

        [Required(ErrorMessage = "Part Code is Required!")]
        public string PartCode { get; set; }

        [Required(ErrorMessage = "Part Name is Required!")]
        public string PartName { get; set; }
        
        [Required(ErrorMessage = "Part Desc is Required!")]
        public string PartDesc { get; set; }

        public string PreviousPartDrgFile {get; set; }
        public string PreviousPartCode { get; set; }

    }
}
