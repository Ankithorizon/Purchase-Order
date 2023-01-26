using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PartTracking.Context.Models.DTO
{
    public class TrackingInputData
    {
        public int PartMasterId { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
    }
}
