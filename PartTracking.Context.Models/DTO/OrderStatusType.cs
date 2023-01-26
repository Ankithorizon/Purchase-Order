using System;
using System.Collections.Generic;
using System.Text;

namespace PartTracking.Context.Models.DTO
{
    public enum OrderStatusType
    {
        Confirmed = 0, // 0
        Received = 1, //1
        Cancelled = 2, // 2
        Received_WIP = 3 // 3
    }
}
