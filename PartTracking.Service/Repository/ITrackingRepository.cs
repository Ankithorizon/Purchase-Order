using Microsoft.AspNetCore.Mvc.Rendering;
using PartTracking.Context.Models.DTO;
using PartTracking.Context.Models.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PartTracking.Service.Repository
{
    public interface ITrackingRepository : IGenericRepository<PartMaster>
    {
        PartTrackingData GetPartOrdersData(int partMasterId, string year, string month);
        OrderTrackingData GetPartReceivingData(int orderMasterId);
        List<SelectListItem> GetYears();
        List<SelectListItem> GetMonths();
        PartTrackingData GetWOData(int partMasterId);
        WOTrackingData GetPullingData(int workOrderId);

    }
}
