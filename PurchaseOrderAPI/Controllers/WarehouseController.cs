using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PartTracking.Context.Models.DTO;
using PartTracking.Service.UOfW;
using PurchaseOrderAPI.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace PurchaseOrderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehouseController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public WarehouseController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("getWarehouseOrders")]
        public IActionResult GetWarehouseOrders(string sortOrder, string searchString)
        {
            try
            {
                // throw new Exception();

                var warehouseOrders = _unitOfWork.OrderMasters.GetWarehouseOrdersWithPartsInfo().OrderBy(x => x.OrderMasterId);

                if (!String.IsNullOrEmpty(searchString))
                {
                    var warehouseOrdersSearched = warehouseOrders
                                    .Where(x => x.RefCode.ToLower().Contains(searchString.ToLower()) || x.PartCode.ToLower().Contains(searchString.ToLower()) || x.PartName.ToLower().Contains(searchString.ToLower())).ToList();
                    // order by
                    switch (sortOrder)
                    {
                        case "partname_desc":
                            warehouseOrders = warehouseOrdersSearched.OrderByDescending(s => s.PartName);
                            break;
                        case "Date":
                            warehouseOrders = warehouseOrdersSearched.OrderBy(s => s.OrderDate);
                            break;
                        case "date_desc":
                            warehouseOrders = warehouseOrdersSearched.OrderByDescending(s => s.OrderDate);
                            break;
                        case "OrderStatus":
                            warehouseOrders = warehouseOrdersSearched.OrderBy(s => s.OrderStatus);
                            break;
                        case "orderstatus_desc":
                            warehouseOrders = warehouseOrdersSearched.OrderByDescending(s => s.OrderStatus);
                            break;
                        default:
                            warehouseOrders = warehouseOrdersSearched.OrderBy(s => s.PartName);
                            break;
                    }
                    return Ok(warehouseOrders.ToList());
                }
                else
                {
                    // order by
                    switch (sortOrder)
                    {
                        case "partname_desc":
                            warehouseOrders = warehouseOrders.OrderByDescending(s => s.PartName);
                            break;
                        case "Date":
                            warehouseOrders = warehouseOrders.OrderBy(s => s.OrderDate);
                            break;
                        case "date_desc":
                            warehouseOrders = warehouseOrders.OrderByDescending(s => s.OrderDate);
                            break;
                        case "OrderStatus":
                            warehouseOrders = warehouseOrders.OrderBy(s => s.OrderStatus);
                            break;
                        case "orderstatus_desc":
                            warehouseOrders = warehouseOrders.OrderByDescending(s => s.OrderStatus);
                            break;
                        default:
                            warehouseOrders = warehouseOrders.OrderBy(s => s.PartName);
                            break;
                    }
                    return Ok(warehouseOrders.ToList());
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Server Error!");
            }
        }

        [HttpGet]
        [Route("getOrderDetails/{selectedOrderId:int}")]
        public IActionResult GetOrderDetails(int selectedOrderId)
        {
            try
            {
                var orderMasterDetail = _unitOfWork.OrderMasters.GetById(selectedOrderId);
                int partMasterId = orderMasterDetail.PartMasterId;
                var partMasterDetail = _unitOfWork.PartMasters.Find(x => x.PartMasterId == partMasterId);
                if (orderMasterDetail != null && partMasterDetail != null && partMasterDetail.Count() == 1)
                {
                    OrderMasterView orderMaster = new OrderMasterView()
                    {
                        PartMasterId = orderMasterDetail.PartMasterId,
                        PartCode = partMasterDetail.FirstOrDefault().PartCode,
                        PartName = partMasterDetail.FirstOrDefault().PartName,
                        OrderQuantity = orderMasterDetail.OrderQuantity,
                        OrderDate = orderMasterDetail.OrderDate,
                        OrderMasterId = orderMasterDetail.OrderMasterId,
                        OrderStatus = orderMasterDetail.OrderStatus,
                        RefCode = orderMasterDetail.RefCode
                    };
                    return Ok(orderMaster);
                }
                else
                {
                    return Ok(new OrderMasterView());
                }
            }
            catch (Exception ex)
            {
                return BadRequest("General Exception!");
            }
        }

    }
}
