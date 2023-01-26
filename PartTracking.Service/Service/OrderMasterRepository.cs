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
    public class OrderMasterRepository : GenericRepository<OrderMaster>, IOrderMasterRepository
    {
        public OrderMasterRepository(PartMgtContext context) : base(context)
        {            
        }           
        public List<WarehouseOrderView> GetWarehouseOrdersWithPartsInfo()
        {
            List<WarehouseOrderView> orders = new List<WarehouseOrderView>();

            var _orders = _context.OrderMaster.Include(x=>x.PartMaster);
            if(_orders!=null && _orders.Count() > 0)
            {
                foreach(var _order in _orders){
                    WarehouseOrderView order = new WarehouseOrderView();
                    orders.Add(new WarehouseOrderView()
                    {
                         OrderDate = _order.OrderDate,
                          OrderMasterId = _order.OrderMasterId,
                           OrderQuantity = _order.OrderQuantity,
                            OrderStatus = _order.OrderStatus,
                             RefCode = _order.RefCode,
                              PartCode = _order.PartMaster.PartCode,
                               PartName = _order.PartMaster.PartName  ,
                               PartMasterId = _order.PartMasterId
                    });
                }             
            }
            return orders;
        }

        public string SP_EditOrderMaster(OrderMasterEditVM orderMaster)
        {
            var partMasterIdParam = new SqlParameter("@PartMasterId", orderMaster.PartMasterId);
            var orderQuantityParam = new SqlParameter("@OrderQuantity", orderMaster.OrderQuantity);
            var orderMasterIdParam = new SqlParameter("@OrderMasterId", orderMaster.OrderMasterId);
            var retCode = new SqlParameter("@retCode", SqlDbType.Int);
            retCode.Direction = ParameterDirection.Output;

            try
            {
                _context.Database.ExecuteSqlRaw("exec EditOrderMaster @PartMasterId,@OrderQuantity, @OrderMasterId, @retCode out",
                        partMasterIdParam, orderQuantityParam, orderMasterIdParam, retCode);

                if (Convert.ToInt32(retCode.Value) == 0)
                {
                    // success
                    return "SUCCESS!";
                }
                else
                {
                    // fail
                    return "FAIL!... STORED PROCEDURE ERROR!";
                }
            }
            catch (Exception ex)
            {
                return "FAIL!... STORED PROCEDURE ERROR!";
            }
        }
    }
}
