using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

    }
}
