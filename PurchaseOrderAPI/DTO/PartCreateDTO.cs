using Microsoft.AspNetCore.Http;
using PartTracking.Context.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseOrderAPI.DTO
{
    public class PartCreateDTO
    {
        public IFormFile PartFile { get; set; }
        public string PartCode { get; set; }
        public string PartName { get; set; }
        public string PartDesc { get; set; }
        public string PartDrgFile { get; set; }
    }
}
