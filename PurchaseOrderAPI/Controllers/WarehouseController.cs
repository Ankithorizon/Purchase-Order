using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PartTracking.Context.Models.DTO;
using PartTracking.Context.Models.Models;
using PartTracking.Service.UOfW;
using PartTracking.Service.Utility;
using PurchaseOrderAPI.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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

        [HttpGet]
        [Route("orderEdit/{selectedOrderId:int}")]
        public IActionResult OrderEdit(int selectedOrderId)
        {
            var orderMasterDetail = _unitOfWork.OrderMasters.Find(x => x.OrderMasterId == selectedOrderId && x.OrderStatus == 0);
            if (orderMasterDetail != null && orderMasterDetail.Count() == 1)
            {
                int partMasterId = orderMasterDetail.FirstOrDefault().PartMasterId;
                var partMasterDetail = _unitOfWork.PartMasters.Find(x => x.PartMasterId == partMasterId);
                if (orderMasterDetail != null && partMasterDetail != null && partMasterDetail.Count() == 1)
                {
                    var model = new OrderMasterEditVM()
                    {
                        OrderMasterId = selectedOrderId,
                        OrderQuantity = (int)(orderMasterDetail.FirstOrDefault().OrderQuantity),
                        PartMasterId = partMasterDetail.FirstOrDefault().PartMasterId,
                        PartMasterSelectList = _unitOfWork.PartMasters.GetPartMasterSelectList()
                    };
                    return Ok(model);
                }
                else
                {
                    return BadRequest("General Exception!");
                }
            }
            else
            {
                return BadRequest("Order is already Received!");
            }
        }
        [HttpPost]
        [Route("orderEditPost")]
        public IActionResult OrderEditPost(OrderMasterEditVM orderMasterEdit)
        {
            try
            {
                // throw new Exception();

                // ModelState.AddModelError("OrderMasterId", "Order# is required!");
                // ModelState.AddModelError("PartMasterId", "Part is required!");
                if (ModelState.IsValid)
                {
                    var spResponse = _unitOfWork.OrderMasters.SP_EditOrderMaster(orderMasterEdit);
                    return Ok(new APIResponse()
                    {
                        ResponseCode = 0,
                        ResponseMessage = spResponse + " : Warehouse - Order Edited Successfully!"
                    });
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }         
            catch (Exception ex)
            {
                return StatusCode(500, "Server Error !");
            }
        }

        [HttpGet]
        [Route("getPartMasterList")]
        public IActionResult GetPartMasterList()
        {
            try
            {
                var partMasterList = _unitOfWork.PartMasters.GetPartMasterSelectList();
                return Ok(partMasterList);
            }
            catch (Exception ex)
            {
                return BadRequest("Server Error!");
            }
        }
        [HttpPost]
        [Route("orderCreate")]
        public IActionResult OrderCreate(OrderMaster orderMaster)
        {
            try
            {
                // throw new Exception();
                // throw new DbUpdateException();

                // ModelState.AddModelError("PartMasterId", "Part is required!");

                if (ModelState.IsValid)
                {
                    orderMaster.OrderDate = DateTime.Now;
                    orderMaster.RefCode = RefCodeGenerator.RandomString(6);
                    orderMaster.OrderStatus = (int?)OrderStatusType.Confirmed;

                    orderMaster = _unitOfWork.OrderMasters.AddAndReturn(orderMaster);
                    return Ok(new APIResponse()
                    {
                        ResponseCode = 0,
                        ResponseMessage = orderMaster.RefCode
                    });
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (DbUpdateException dbUpdateEx)
            {
                return StatusCode(500, "Database Exception !");
            }          
            catch (Exception ex)
            {
                return StatusCode(500, "Server Error !");
            }
        }


    }
}
