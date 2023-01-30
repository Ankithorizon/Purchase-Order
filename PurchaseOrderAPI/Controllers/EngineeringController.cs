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
    public class EngineeringController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private string[] permittedExtensions = { ".pdf" };

        public EngineeringController(IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;

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
        [HttpPost, DisableRequestSizeLimit]
        [Route("upload")]
        public IActionResult Upload([FromForm] PartEditDTO partEditDto)
        {            
            try
            {
                // throw new Exception();

                if (partEditDto == null)
                {                  
                    return BadRequest("Bad Request! Null Object!");
                }

                if (partEditDto.PartCode == null)
                {
                    return BadRequest("Bad Request! Null Object!");
                }
                if (partEditDto.PartName == null)
                {
                    return BadRequest("Bad Request! Null Object!");
                }
                if (partEditDto.PartDesc == null)
                {
                    return BadRequest("Bad Request! Null Object!");
                }                
            
                var file = partEditDto.PartFile;
                if (file == null)
                {
                    // file is not changed/edited                    
                    // return BadRequest("Bad Request! No file content found!");
                }                    
                else
                {
                    // file is changed/edited
                    // check for file type
                    // .pdf
                    var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
                    if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
                    {
                        return BadRequest("Bad Request! Invalid File-Type!");
                    }

                    string partFileStoragePath = _configuration.GetSection("PartFileUploadLocation").GetSection("Path").Value;

                    // unique random number to edit file name
                    var guid = Guid.NewGuid();
                    var bytes = guid.ToByteArray();
                    var rawValue = BitConverter.ToInt64(bytes, 0);
                    var inRangeValue = Math.Abs(rawValue) % DateTime.MaxValue.Ticks;

                    var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), partFileStoragePath);

                    if (file.Length > 0)
                    {
                        var fileName = inRangeValue + "_" + ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                        var fullPath = Path.Combine(pathToSave, fileName);

                        // file-system store
                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                    }
                    else
                    {
                        return BadRequest("Nothing To Upload !");
                    }
                }

                // db store
                // 

                return Ok(new APIResponse()
                {
                    ResponseCode = 0,
                    ResponseMessage = "Part Edited Successfully!"
                });

            }
            catch (FormatException)
            {              
                return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Server Error !");
            }
        }

    }
}
