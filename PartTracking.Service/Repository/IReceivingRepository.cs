using PartTracking.Context.Models.DTO;
using PartTracking.Context.Models.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PartTracking.Service.Repository
{
    public interface IReceivingRepository : IGenericRepository<ReceivePart>
    {
        List<ReceivePartView> GetReceivePartHistory();
        string SP_ReceivePart(ReceivePartAddVM receivePart);
    }
}
