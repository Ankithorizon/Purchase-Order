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
    public class ReceivingController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReceivingController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("getReceivedOrders")]
        public IActionResult GetReceivedOrders(string sortOrder, string searchString)
        {
            try
            {
                // throw new Exception();

                var receivedOrders = _unitOfWork.ReceiveParts.GetReceivePartHistory().OrderBy(x => x.ReceivePartId);

                if (!String.IsNullOrEmpty(searchString))
                {
                    var receivedOrdersSearched = receivedOrders
                                    .Where(x => x.RefCode.ToLower().Contains(searchString.ToLower()) || x.Part.ToLower().Contains(searchString.ToLower())).ToList();
                    // order by
                    switch (sortOrder)
                    {
                        case "refcode_desc":
                            receivedOrders = receivedOrdersSearched.OrderByDescending(s => s.RefCode);
                            break;
                        case "Date":
                            receivedOrders = receivedOrdersSearched.OrderBy(s => s.ReceiveDate);
                            break;
                        case "receivedate_desc":
                            receivedOrders = receivedOrdersSearched.OrderByDescending(s => s.ReceiveDate);
                            break;
                        default:
                            receivedOrders = receivedOrdersSearched.OrderBy(s => s.RefCode);
                            break;
                    }
                    return Ok(receivedOrders.ToList());
                }
                else
                {
                    // order by
                    switch (sortOrder)
                    {
                        case "refcode_desc":
                            receivedOrders = receivedOrders.OrderByDescending(s => s.RefCode);
                            break;
                        case "Date":
                            receivedOrders = receivedOrders.OrderBy(s => s.ReceiveDate);
                            break;
                        case "receivedate_desc":
                            receivedOrders = receivedOrders.OrderByDescending(s => s.ReceiveDate);
                            break;
                        default:
                            receivedOrders = receivedOrders.OrderBy(s => s.RefCode);
                            break;
                    }
                    return Ok(receivedOrders.ToList());
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Server Error!");
            }
        }

        [AcceptVerbs("GET", "POST")]
        [Route("getOrderQuantity")]
        public IActionResult GetOrderQuantity(string refCode)
        {
            var order_ = _unitOfWork.OrderMasters.Find(x => x.RefCode == refCode);
            if (order_ != null && order_.Count() == 1)
            {
                return Ok(new 
                {
                    qty = order_.FirstOrDefault().OrderQuantity,
                    status = "Success!"
                });
            }
            else
            {
                return Ok(new
                {
                    qty = 0,
                    status = "Invalid Input!"
                });
            }
        }

        [HttpPost]
        [Route("orderReceive")]
        public IActionResult OrderReceive(ReceivePartView receivePart)
        {
            try
            {
                // throw new Exception();

                if (ModelState.IsValid)
                {
                    // prepare object from _unitOfWork
                    // send it to repository to call SP with transaction


                    // prepare object from _unitOfWork
                    var _orderMaster = _unitOfWork.OrderMasters.Find(x => x.RefCode == receivePart.RefCode && (x.OrderStatus == 0 || x.OrderStatus == 3));
                    if (_orderMaster != null && _orderMaster.Count() == 1)
                    {
                        ReceivePartAddVM _receivePart = new ReceivePartAddVM()
                        {
                            OrderMasterId = _orderMaster.FirstOrDefault().OrderMasterId,
                            PartMasterId = _orderMaster.FirstOrDefault().PartMasterId,
                            ReceiveDate = DateTime.Now,
                            ReceiveQuantity = receivePart.ReceiveQuantity,
                            RefCode = receivePart.RefCode
                        };
                        // send it to repository to call SP with transaction
                        // TR1 add @ReceivePart
                        // TR2 change StatusCode to 1 -- Received @OrderMaster
                        // TR3 change Quantity to Quantity+ReceiveQuantity @PartMaster
                        var spResponse = _unitOfWork.ReceiveParts.SP_ReceivePart(_receivePart);
                        return Ok(new APIResponse()
                        {
                            ResponseCode = 0,
                            ResponseMessage = "Order Received Successfully!"
                        });
                    }
                    else
                    {
                        return Ok(new APIResponse()
                        {
                            ResponseCode = -1,
                            ResponseMessage = "FAIL : Order Already Received!"
                        });
                    }
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


    }
}
