using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;
using PartTracking.Context.Models.Models;
using PartTracking.Service.Repository;
using PartTracking.Context.Models.DTO;

namespace PartTracking.Service.Service
{
    public class CustomerWorkOrderRepository : GenericRepository<WorkOrder>, ICustomerWorkOrderRepository
    {
        public CustomerWorkOrderRepository(PartMgtContext context) : base(context)
        {
        }

        public List<CustomerWorkOrderView> GetCustomerWorkOrders()
        {
            List<CustomerWorkOrderView> customerWorkOrders = new List<CustomerWorkOrderView>();

            // workorders
            var _workorders = _context.WorkOrder
                        .Include(x => x.PartMaster)
                        .Include(x => x.CustomerOrder);
            if (_workorders != null && _workorders.Count()>0)
            {
                foreach(var _workorder in _workorders)
                {
                    customerWorkOrders.Add(new CustomerWorkOrderView()
                    {
                        WOId = _workorder.Woid,
                         WorkOrderId = _workorder.WorkOrderId,
                          PartQuantityRequired = _workorder.PartQuantityRequired,
                           PartMasterId = _workorder.PartMasterId,
                            PartName = _workorder.PartMaster.PartName,
                             PartCode = _workorder.PartMaster.PartCode,
                              PartQuantityAtWarehouse = (int?)_workorder.PartMaster.Quantity,
                               CustomerOrderId = _workorder.CustomerOrderId,
                                CustomerId = _workorder.CustomerOrder.CustomerId,
                                 ProductName = _workorder.CustomerOrder.ProductName,
                                  OrderQuantity = _workorder.CustomerOrder.OrderQuantity,
                                   BalanceAfterPull =(int)_workorder.BalanceAfterPull

                    });
                }
            }
            return customerWorkOrders;
        }

        public int RunPullingQuantityProcess(PullingQuantity pullingQuantity)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var workOrder = _context.WorkOrder
                    .Include(x => x.PartMaster)
                    .Where(x => x.Woid == pullingQuantity.WOId).FirstOrDefault();

                    if (workOrder != null)
                    {
                        // update workorder
                        workOrder.PartQuantityPulled = pullingQuantity.PartQuantityPulled;
                        workOrder.BalanceAfterPull -= pullingQuantity.PartQuantityPulled;

                        // update partmaster
                        workOrder.PartMaster.Quantity -= pullingQuantity.PartQuantityPulled;

                        _context.SaveChanges();

                        // insert partworkorder
                        PartWorkOrder partWorkOrder = new PartWorkOrder()
                        {
                            PartId = pullingQuantity.PartMasterId,
                            PartQuantityPulled = pullingQuantity.PartQuantityPulled,
                            WorkOrderId = pullingQuantity.WorkOrderId,
                            PulledDate = DateTime.Now
                        };
                        _context.PartWorkOrder.Add(partWorkOrder);

                        _context.SaveChanges();

                        transaction.Commit();
                        return 0;
                    }
                    return -1;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return -1;
                }
            }
        }
    }
}
