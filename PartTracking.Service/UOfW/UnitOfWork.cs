using PartTracking.Context.Models.Models;
using PartTracking.Service.Repository;
using PartTracking.Service.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace PartTracking.Service.UOfW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PartMgtContext _context;
        public UnitOfWork(PartMgtContext context)
        {
            _context = context;
            OrderMasters = new OrderMasterRepository(_context);
            PartMasters = new PartMasterRepository(_context);
            PartDetails = new PartDetailRepository(_context);
            ReceiveParts = new ReceivingRepository(_context);
            CustomerWorkOrders = new CustomerWorkOrderRepository(_context);
            PartTrackingService = new TrackingRepository(_context);
        }
        public IOrderMasterRepository OrderMasters { get; private set; }
        public IPartMasterRepository PartMasters { get; private set; }
        public IPartDetailRepository PartDetails { get; private set; }
        public IReceivingRepository ReceiveParts { get; private set; }
        public ICustomerWorkOrderRepository CustomerWorkOrders { get; private set; }
        public ITrackingRepository PartTrackingService { get; private set; }
        public int Complete()
        {
            return _context.SaveChanges();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
