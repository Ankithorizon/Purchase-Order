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
    public class ReceivingRepository : GenericRepository<ReceivePart>, IReceivingRepository
    {
        public ReceivingRepository(PartMgtContext context) : base(context)
        {
        }

        public List<ReceivePartView> GetReceivePartHistory()
        {
            List<ReceivePartView> receivedParts = new List<ReceivePartView>();

            var _receivedParts = _context.ReceivePart.Include(x => x.OrderMaster).Include(x=>x.PartMaster);
            if (_receivedParts != null && _receivedParts.Count() > 0)
            {
                foreach (var _receivedPart in _receivedParts)
                {
                    receivedParts.Add(new ReceivePartView()
                    {
                         OrderDate = (DateTime)_receivedPart.OrderMaster.OrderDate,
                          OrderMasterId = _receivedPart.OrderMasterId,
                           OrderQuantity =(int)_receivedPart.OrderMaster.OrderQuantity,
                            OrderStatus = (int)_receivedPart.OrderMaster.OrderStatus,
                             PartMasterId = _receivedPart.PartMasterId,
                              ReceiveDate = _receivedPart.ReceiveDate,
                               ReceivePartId = _receivedPart.ReceivePartId,
                                ReceiveQuantity = _receivedPart.ReceiveQuantity,
                                 RefCode = _receivedPart.OrderMaster.RefCode,
                                 Part = _receivedPart.PartMaster.PartName + " ["+ _receivedPart.PartMaster.PartCode+" ]"
                       
                    });
                }
            }
            return receivedParts;
        }

        public string SP_ReceivePart(ReceivePartAddVM receivePart)
        {
            var orderMasterIdParam = new SqlParameter("@OrderMasterId", receivePart.OrderMasterId);
            var partMasterIdParam = new SqlParameter("@PartMasterId", receivePart.PartMasterId);
            var receiveQuantityParam = new SqlParameter("@ReceiveQuantity", receivePart.ReceiveQuantity);
            var receiveDateParam = new SqlParameter("@ReceiveDate", receivePart.ReceiveDate);
            var refCodeParam = new SqlParameter("@RefCode", receivePart.RefCode);           
           
            var retCode = new SqlParameter("@retCode", SqlDbType.Int);
            retCode.Direction = ParameterDirection.Output;

            try
            {
                _context.Database.ExecuteSqlRaw("exec ReceivePartMaster @OrderMasterId,@PartMasterId, @ReceiveQuantity, @ReceiveDate, @RefCode, @retCode out",
                        orderMasterIdParam, partMasterIdParam, receiveQuantityParam, receiveDateParam, refCodeParam,  retCode);

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
