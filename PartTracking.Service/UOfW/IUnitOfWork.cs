using PartTracking.Service.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PartTracking.Service.UOfW
{
    public interface IUnitOfWork : IDisposable
    {
        IOrderMasterRepository OrderMasters { get; }
        IPartMasterRepository PartMasters { get; }
        IPartDetailRepository PartDetails { get; }
        IReceivingRepository ReceiveParts { get; }
        ICustomerWorkOrderRepository CustomerWorkOrders { get; }
        ITrackingRepository PartTrackingService { get; }
        int Complete();
    }
}
