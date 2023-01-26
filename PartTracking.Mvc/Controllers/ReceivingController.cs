using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PartTracking.Context.Models.DTO;
using PartTracking.Context.Models.Models;
using PartTracking.Mvc.Models;
using PartTracking.Service.UOfW;
using PartTracking.Service.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PartTracking.Mvc.Controllers
{
    public class ReceivingController : Controller
    {
        private readonly ILogger<WarehouseController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public ReceivingController(ILogger<WarehouseController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index(string sortOrder, string searchString)
        {
            ViewData["RefCodeSortParm"] = String.IsNullOrEmpty(sortOrder) ? "refcode_desc" : "";
            ViewData["ReceiveDateSortParm"] = sortOrder == "Date" ? "receivedate_desc" : "Date";
            
            var receivedOrders = _unitOfWork.ReceiveParts.GetReceivePartHistory().OrderBy(x=>x.ReceivePartId);


            // search 
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
                return View(receivedOrders);
            }
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
            return View(receivedOrders);
        }

        [HttpPost("Receiving/GetOrderQuantity")]
        public ActionResult GetOrderQuantity(string refCode)
        {
            var order_ = _unitOfWork.OrderMasters.Find(x=>x.RefCode==refCode);
            if (order_!=null && order_.Count()==1)
            {
                return Json(new { qty = order_.FirstOrDefault().OrderQuantity, status = "Success!" });
            }
            else
            {
                return Json(new { qty = 0, status = "Invalid Input!" });
            }
        }
        
        public IActionResult ReceivePart()
        {          
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ReceivePart([Bind("RefCode, ReceiveQuantity")] ReceivePartView receivePart)
        {
            string spResponse = "";
            if (ModelState.IsValid)
            {
                try
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
                        spResponse = _unitOfWork.ReceiveParts.SP_ReceivePart(_receivePart);
                        spResponse = "Success!";
                        TempData["SPResponse"] = spResponse;
                        ModelState.Clear();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["SPResponse"] = "FAIL : Order Not Found!";
                    }
                }
                catch(Exception ex)
                {
                    TempData["SPResponse"] = "FAIL : GENERAL EXCEPTION!";
                }               
            }
            return View(receivePart);
        }


        // modal window as partial view
        [HttpGet]
        public IActionResult ReceivedPartDetails(int Id)
        {
            try
            {
                // throw new Exception();

                ReceivedPartDetailsView receivedPartDetailsView = new ReceivedPartDetailsView();

                // find ReceivePart
                var receivedPartDetail = _unitOfWork.ReceiveParts.GetById(Id);
                // var receivedPartDetail = _unitOfWork.ReceiveParts.GetById(-1);
                if (receivedPartDetail != null)
                {
                    // find OrderMaster
                    var orderMaster = _unitOfWork.OrderMasters.GetById(receivedPartDetail.OrderMasterId);
                    if (orderMaster != null)
                    {
                        // find PartMaster
                        var partMaster = _unitOfWork.PartMasters.GetById(receivedPartDetail.PartMasterId);
                        if (partMaster != null)
                        {
                            // prepare final object
                            receivedPartDetailsView.ReceivePartView = new ReceivePartView()
                            { 
                                ReceivePartId = receivedPartDetail.ReceivePartId,
                                 ReceiveQuantity = receivedPartDetail.ReceiveQuantity,
                                  ReceiveDate = receivedPartDetail.ReceiveDate
                            };
                            receivedPartDetailsView.OrderMasterView = new OrderMasterView()
                            {
                                 OrderMasterId = orderMaster.OrderMasterId,
                                  OrderQuantity =orderMaster.OrderQuantity,
                                   OrderDate = orderMaster.OrderDate,
                                    OrderStatus = orderMaster.OrderStatus,
                                     RefCode = orderMaster.RefCode
                            };
                            receivedPartDetailsView.PartMasterPartDetailsView = new PartMasterPartDetailsView()
                            {
                                 PartMasterId = partMaster.PartMasterId,
                                  PartCode = partMaster.PartCode,
                                   PartName = partMaster.PartName,
                                   Quantity = partMaster.Quantity                             
                            };
                            return PartialView("_ReceivedPartDetails", receivedPartDetailsView);
                        }
                        else{
                            TempData["Exception"] = "Part Not Found!";
                            return PartialView("_ReceivedPartDetails", receivedPartDetailsView);
                        }
                    }
                    else
                    {
                        TempData["Exception"] = "Order Not Found!";
                        return PartialView("_ReceivedPartDetails", receivedPartDetailsView);
                    }                    
                }
                else
                {
                    TempData["Exception"] = "Received Order Not Found!";
                    return PartialView("_ReceivedPartDetails", receivedPartDetailsView);
                }                
            }
            catch (Exception ex)
            {
                TempData["Exception"] = "General Exception!";
                return PartialView("_ReceivedPartDetails", null);
            }
        }

    }
}
