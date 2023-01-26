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
    public class WarehouseController : Controller
    {
        private readonly ILogger<WarehouseController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public List<SelectListItem> PartMasterSelectList{ get; set; }

        public WarehouseController(ILogger<WarehouseController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index(string sortOrder, string searchString)
        {
            ViewData["PartNameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "partname_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            ViewData["OrderStatusSortParm"] = sortOrder == "OrderStatus" ? "orderstatus_desc" : "OrderStatus";
            var warehouseOrders = _unitOfWork.OrderMasters.GetWarehouseOrdersWithPartsInfo().OrderBy(x=>x.OrderMasterId);


            // search 
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
                return View(warehouseOrders.ToList());
            }

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
            return View(warehouseOrders.ToList());

            // var warehouseOrders = _unitOfWork.OrderMasters.GetWarehouseOrdersWithPartsInfo();
            // return View(warehouseOrders);
        }
        
        public IActionResult OrderPart()
        {
            PartMasterSelectList = _unitOfWork.PartMasters.GetPartMasterSelectList();
            ViewBag.Parts = PartMasterSelectList;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult OrderPart([Bind("PartMasterId,OrderQuantity")] OrderMaster orderMaster)
        {
            if (ModelState.IsValid)
            {
                // prepare final object
                orderMaster.OrderDate = DateTime.Now;
                orderMaster.RefCode = RefCodeGenerator.RandomString(6);
                orderMaster.OrderStatus = (int?)OrderStatusType.Confirmed;
                try
                {
                    orderMaster = _unitOfWork.OrderMasters.AddAndReturn(orderMaster);
                    TempData["RefCode"] = orderMaster.RefCode;
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException dbUpdateEx)
                {
                    TempData["Exception"] = "Database Exception!";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["Exception"] = "General Exception!";
                    return RedirectToAction("Index");
                }                
            }
            return View(orderMaster);
        }


        [HttpGet]
        [Route("Warehouse/OrderDetails/{id:int}")]
        public IActionResult OrderDetails(int id)
        {
            try
            {
                var orderMasterDetail = _unitOfWork.OrderMasters.GetById(id);
                int partMasterId = orderMasterDetail.PartMasterId;
                var partMasterDetail = _unitOfWork.PartMasters.Find(x => x.PartMasterId == partMasterId);
                if (orderMasterDetail != null && partMasterDetail != null && partMasterDetail.Count()==1)
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
                    return View(orderMaster);
                }
                else
                {
                    return View(new OrderMasterView());
                }
            }
            catch(Exception ex)
            {
                TempData["Exception"] = "General Exception!";
                return View(new OrderMasterView());
            }         
        }


        [HttpGet]
        [Route("Warehouse/OrderEdit/{id:int}")]
        public IActionResult OrderEdit(int id)
        {            
            var orderMasterDetail = _unitOfWork.OrderMasters.Find(x => x.OrderMasterId == id && x.OrderStatus == 0);
            if(orderMasterDetail!=null && orderMasterDetail.Count() == 1)
            {
                int partMasterId = orderMasterDetail.FirstOrDefault().PartMasterId;
                var partMasterDetail = _unitOfWork.PartMasters.Find(x => x.PartMasterId == partMasterId);
                if (orderMasterDetail != null && partMasterDetail != null && partMasterDetail.Count() == 1)
                {
                    var model = new OrderMasterEditVM()
                    {
                        OrderMasterId = id,
                        OrderQuantity = (int)(orderMasterDetail.FirstOrDefault().OrderQuantity),
                        PartMasterId = partMasterDetail.FirstOrDefault().PartMasterId,
                        PartMasterSelectList = _unitOfWork.PartMasters.GetPartMasterSelectList()
                    };
                    return View("OrderEdit", model);
                }
                else
                {
                    TempData["Exception"] = "General Exception!";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["Exception"] = "Order is already Received!";
                return RedirectToAction("Index");
            }                      
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult OrderEdit([Bind("OrderMasterId,PartMasterId,OrderQuantity")] OrderMasterEditVM orderMasterEdit)
        {
            string spResponse = "";
            if (ModelState.IsValid)
            {
                try
                {
                    // throw new Exception();

                    spResponse = _unitOfWork.OrderMasters.SP_EditOrderMaster(orderMasterEdit);
                    spResponse = "Success!";
                    TempData["SPResponse"] = spResponse;
                    ModelState.Clear();
                    return RedirectToAction("Index");
                }              
                catch (Exception ex)
                {
                    TempData["SPResponse"] = "FAIL : GENERAL EXCEPTION!";
                }
            }
            orderMasterEdit.PartMasterSelectList = _unitOfWork.PartMasters.GetPartMasterSelectList();
            return View("OrderEdit", orderMasterEdit);
        }

    }
}
