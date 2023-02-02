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
                    return Ok(warehouseOrders.OrderBy(x => x.OrderMasterId).ToList());
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
                    return Ok(warehouseOrders.OrderBy(x => x.OrderMasterId).ToList());
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Server Error!");
            }
        }
    }
}
