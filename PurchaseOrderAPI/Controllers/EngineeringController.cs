using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PartTracking.Context.Models.DTO;
using PartTracking.Service.UOfW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseOrderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EngineeringController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;

        public EngineeringController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("allParts")]
        public IActionResult GetAllParts(string searchString)
        {
            try
            {
                // throw new Exception();
                if (!String.IsNullOrEmpty(searchString))
                {
                    var parts = _unitOfWork.PartMasters.Find(x => x.PartCode.Contains(searchString.ToLower()));
                    return Ok(parts);
                }
                else
                {
                    var parts = _unitOfWork.PartMasters.GetAll().ToList();
                    return Ok(parts);
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Server Error!");
            }
        }


        [HttpGet]
        [Route("getPartDetails/{selectedPartId:int}")]
        public IActionResult GetPartDetails(int selectedPartId)
        {
            var partMaster = _unitOfWork.PartMasters.GetById(selectedPartId);
            var partDetail = _unitOfWork.PartDetails.Find(x => x.PartMasterId == selectedPartId);

            if (partMaster != null && partDetail != null && partDetail.Count() > 0)
            {
                PartMasterPartDetailsView partMasterpartDetails = new PartMasterPartDetailsView()
                {
                    PartMasterId = partMaster.PartMasterId,
                    PartCode = partMaster.PartCode,
                    PartName = partMaster.PartName,
                    Quantity = partMaster.Quantity,
                    PartDesc = partDetail.FirstOrDefault().PartDesc,
                    PartDetailId = partDetail.FirstOrDefault().PartDetailId,
                    PartDrgFile = partDetail.FirstOrDefault().PartDrgFile
                };
                return Ok(partMasterpartDetails);
            }
            else
            {
                return BadRequest();
            }
        }


        [HttpGet]
        [Route("partEdit/{selectedPartId:int}")]
        public IActionResult PartEdit(int selectedPartId)
        {
            var partMaster = _unitOfWork.PartMasters.GetById(selectedPartId);
            var partDetail = _unitOfWork.PartDetails.Find(x => x.PartMasterId == selectedPartId);
            if (partMaster != null && partDetail != null && partDetail.Count() > 0)
            {
                PartMasterPartDetailsEditVM partMasterpartDetailsEdit = new PartMasterPartDetailsEditVM()
                {
                    PartMasterId = partMaster.PartMasterId,
                    PartCode = partMaster.PartCode,
                    PartName = partMaster.PartName,
                    Quantity = (int?)partMaster.Quantity,
                    PartDesc = partDetail.FirstOrDefault().PartDesc,
                    PartDetailId = partDetail.FirstOrDefault().PartDetailId,
                    PartDrgFile = partDetail.FirstOrDefault().PartDrgFile,
                    PreviousPartDrgFile = partDetail.FirstOrDefault().PartDrgFile,
                    PreviousPartCode = partMaster.PartCode
                };
                return Ok(partMasterpartDetailsEdit);
            }
            else
            {
                return BadRequest();
            }
        }

    }
}
