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
    public class TrackingController : Controller
    {
        private readonly ILogger<TrackingController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public List<SelectListItem> PartMasterSelectList { get; set; }

        public TrackingController(ILogger<TrackingController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
      
        public IActionResult Index()
        {
            PartMasterSelectList = _unitOfWork.PartMasters.GetPartMasterSelectList();
            ViewBag.Parts = PartMasterSelectList;
            ViewBag.Years = _unitOfWork.PartTrackingService.GetYears();
            ViewBag.Months = _unitOfWork.PartTrackingService.GetMonths();
            return View();
        }

        [HttpGet]
        public IActionResult GetWarehouseOrdersByPart(int Id, string year, string month)
        {
            PartTrackingData partTrackingData = new PartTrackingData();
            partTrackingData.Orders = new List<OrderTrackingData>();
            try
            {
                // throw new Exception();

                partTrackingData = _unitOfWork.PartTrackingService.GetPartOrdersData(Id, year,month);
                return PartialView("_partOrders", partTrackingData);
            }
            catch (Exception ex)
            {
                TempData["Exception"] = "Exception!";
                return PartialView("_partOrders", partTrackingData);
            }
        }

        [HttpGet]
        public IActionResult GetReceivingDetailsByOrder(int Id)
        {
            OrderTrackingData orderTrackingData = new OrderTrackingData();
            try
            {
                orderTrackingData = _unitOfWork.PartTrackingService.GetPartReceivingData(Id);
                return PartialView("_partReceiving", orderTrackingData);
            }
            catch (Exception ex)
            {
                TempData["Exception"] = "Exception!";
                return PartialView("_partReceiving", orderTrackingData);
            }
        }

        [HttpGet]
        public IActionResult GetWOData(int Id)
        {
            PartTrackingData partTrackingData = new PartTrackingData();
            partTrackingData.WorkOrders = new List<WOTrackingData>();
            try
            {
                partTrackingData = _unitOfWork.PartTrackingService.GetWOData(Id);
                return PartialView("_workOrders", partTrackingData);
            }
            catch (Exception ex)
            {
                TempData["Exception"] = "Exception!";
                return PartialView("_workOrders", partTrackingData);
            }
        }

        [HttpGet]
        public IActionResult GetPullingDataByWO(int Id)
        {
            WOTrackingData woTrackingData = new WOTrackingData();
            try
            {
                woTrackingData = _unitOfWork.PartTrackingService.GetPullingData(Id);
                return PartialView("_partPulling", woTrackingData);
            }
            catch (Exception ex)
            {
                TempData["Exception"] = "Exception!";
                return PartialView("_partPulling", woTrackingData);
            }
        }
    }
}