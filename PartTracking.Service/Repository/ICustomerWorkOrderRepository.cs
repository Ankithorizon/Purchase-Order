using PartTracking.Context.Models.DTO;
using PartTracking.Context.Models.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PartTracking.Service.Repository
{
    public interface ICustomerWorkOrderRepository : IGenericRepository<WorkOrder>
    {
        List<CustomerWorkOrderView> GetCustomerWorkOrders();
        int RunPullingQuantityProcess(PullingQuantity pullingQuantity);
    }
}
