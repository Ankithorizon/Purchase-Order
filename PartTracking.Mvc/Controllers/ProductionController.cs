using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using PartTracking.Context.Models.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PartTracking.Context.Models.DTO;
using PartTracking.Mvc.Models;
using PartTracking.Service.UOfW;
using PartTracking.Service.Utility;
using System.Diagnostics;
using System.Threading;

namespace PartTracking.Mvc.Controllers
{
    public class ProductionController : Controller
    {
        private readonly ILogger<ProductionController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public ProductionController(ILogger<ProductionController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
 
        [HttpGet]
        public IActionResult Index(string searchString)
        {
            var customerWorkOrders = _unitOfWork.CustomerWorkOrders.GetCustomerWorkOrders();

            if (!String.IsNullOrEmpty(searchString))
            {
                var searchedCustomerWorkOrders = customerWorkOrders.Where(s => s.WorkOrderId.ToString().Contains(searchString));                
                return View(searchedCustomerWorkOrders);
            }
            return View(customerWorkOrders);
        }

        [HttpGet]
        [Route("Production/PullPartFromWarehouse/{id:int}")]
        public IActionResult PullPartFromWarehouse(int id)
        {
            var customerWorkOrders = _unitOfWork.CustomerWorkOrders.GetCustomerWorkOrders();
            var customerWorkOrder = customerWorkOrders.Where(x => x.WOId == id).FirstOrDefault();
            return View(customerWorkOrder);            
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PullPartFromWarehouse([Bind("WOId,WorkOrderId,CustomerOrderId,PartMasterId,PartQuantityRequired,PartQuantityAtWarehouse,BalanceAfterPull,PullQuantity")] CustomerWorkOrderView _customerWorkOrder)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    PullingQuantity pullingQuantity = new PullingQuantity()
                    {
                        WOId = _customerWorkOrder.WOId,
                        CustomerOrderId = _customerWorkOrder.CustomerOrderId,
                        PartQuantityAtWarehouse = (int)_customerWorkOrder.PartQuantityAtWarehouse,
                        PartMasterId = _customerWorkOrder.PartMasterId,
                        PartQuantityPulled = _customerWorkOrder.PullQuantity,
                        PartQuantityRequired = _customerWorkOrder.PartQuantityRequired,
                        WorkOrderId = _customerWorkOrder.WorkOrderId,
                        BalanceAfterPull = _customerWorkOrder.BalanceAfterPull
                    };
                    if (_unitOfWork.CustomerWorkOrders.RunPullingQuantityProcess(pullingQuantity)==0)
                    {
                        TempData["EFResponse"] = "SUCCESS!";
                    }
                    else
                    {
                        TempData["EFResponse"] = "FAIL : EF Exception!";
                    }                    
                }
                catch (Exception ex)
                {
                    TempData["EFResponse"] = "FAIL : GENERAL EXCEPTION!";
                }
                return RedirectToAction("Index");
            }

            var customerWorkOrders = _unitOfWork.CustomerWorkOrders.GetCustomerWorkOrders();
            var customerWorkOrder = customerWorkOrders.Where(x => x.WOId == _customerWorkOrder.WOId).FirstOrDefault();
            return View("PullPartFromWarehouse", customerWorkOrder);
        }

    }
}
