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
    public class PartCreateDTO
    {
        public IFormFile PartFile { get; set; }

        [Required(ErrorMessage = "Part Code is Required!")]
        public string PartCode { get; set; }

        [Required(ErrorMessage = "Part Name is Required!")]
        public string PartName { get; set; }

        [Required(ErrorMessage = "Part Desc is Required!")]
        public string PartDesc { get; set; }

        [Required(ErrorMessage = "Part Drg. File is Required!")]
        public string PartDrgFile { get; set; }
    }
}
