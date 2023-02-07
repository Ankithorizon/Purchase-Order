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

    }
}
