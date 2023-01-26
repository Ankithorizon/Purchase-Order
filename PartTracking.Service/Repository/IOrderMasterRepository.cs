using PartTracking.Context.Models.DTO;
using PartTracking.Context.Models.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PartTracking.Service.Repository
{
    public interface IOrderMasterRepository : IGenericRepository<OrderMaster>
    {
        List<WarehouseOrderView> GetWarehouseOrdersWithPartsInfo();
        string SP_EditOrderMaster(OrderMasterEditVM orderMaster);
    }
}
