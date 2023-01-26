using System;
using System.Collections.Generic;
using System.Text;

namespace PartTracking.Context.Models.DTO
{
    public class ReceivedPartDetailsView
    {
        public ReceivePartView ReceivePartView { get; set; }
        public OrderMasterView OrderMasterView { get; set; }
        public PartMasterPartDetailsView PartMasterPartDetailsView { get; set; }    

    }
}
