using Microsoft.AspNetCore.Http;
using PartTracking.Context.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseOrderAPI.DTO
{
    public class PartEditDTO
    {
        public IFormFile PartFile { get; set; }
        public int PartMasterId { get; set; }
        public int PartDetailId { get; set; }
        public string PartCode { get; set; }
        public string PartName { get; set; }
        public string PartDesc { get; set; }
        public string PreviousPartDrgFile {get; set; }
        public string PreviousPartCode { get; set; }

    }
}
