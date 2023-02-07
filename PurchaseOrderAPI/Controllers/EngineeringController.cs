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
            try
            {
                // throw new Exception();

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
                    if (partDetail == null || partDetail.Count() < 1)
                    {
                        return BadRequest("Part-Details Not Found!");
                    }
                    else
                    {
                        return BadRequest("Bad Request!");
                    }
                }
            }
            catch(Exception ex)
            {
                return StatusCode(500, "Server Error !");
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
        [Route("partEditPost")]
        public IActionResult PartEditPost([FromForm] PartEditDTO partEditDto)
        {            
            try
            {
                // throw new Exception();

                if (ModelState.IsValid)
                {

                    string PartDrgFile = string.Empty;

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
                        // @file system store
                        try
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

                            // within same solution/project
                            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), partFileStoragePath);

                            // outside of current solution
                            var outsideOfCurrentSolution = Path.Combine(@"C:\\Users\\ankit_2\\source\\repos\\PartTracking", partFileStoragePath);

                            if (file.Length > 0)
                            {
                                var fileName = inRangeValue + "_" + ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                                // var fullPath = Path.Combine(pathToSave, fileName);
                                var fullPath = Path.Combine(outsideOfCurrentSolution, fileName);

                                PartDrgFile = fileName;

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
                        catch (IOException ioEx)
                        {
                            return BadRequest("FAIL : IO EXCEPTION!");
                        }
                    }


                    // @db store
                    try
                    {
                        var partMasterpartDetail = new PartMasterPartDetailsEditVM()
                        {
                            PartCode = partEditDto.PartCode,
                            PartDesc = partEditDto.PartDesc,
                            PartDetailId = partEditDto.PartDetailId,
                            PartDrgFile = PartDrgFile,
                            PartMasterId = partEditDto.PartMasterId,
                            PartName = partEditDto.PartName,
                            PreviousPartCode = partEditDto.PreviousPartCode,
                            PreviousPartDrgFile = partEditDto.PreviousPartDrgFile
                        };

                        // throw new Exception();
                        // sp call
                        // sp will check for unique part-code and returns -1
                        // if part-code is duplicate
                        // partcode is unique
                        // PartCode_unique constraint @ db
                        var spResponse = _unitOfWork.PartMasters.SP_EditPartMasterWithPartDetail(partMasterpartDetail);
                        if(spResponse== "SUCCESS!")
                        {
                            return Ok(new APIResponse()
                            {
                                ResponseCode = 0,
                                ResponseMessage = spResponse + " : Part Edited Successfully!"
                            });
                        }
                        else
                        {
                            return BadRequest(spResponse);
                        }
                     
                    }
                    catch (Exception ex)
                    {
                        var spResponse = "FAIL : GENERAL EXCEPTION!";
                        return BadRequest(spResponse);
                    }
                }
                else
                {
                    return BadRequest(ModelState);
                }
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

        [AcceptVerbs("GET", "POST")]
        [Route("remoteCheckPartCode")]
        public IActionResult VerifyPartCodeEditOperation(string PreviousPartCode, string PartCode)
        {
            // create part
            if (PreviousPartCode == null)
            {
                var found = _unitOfWork.PartMasters.Find(x => x.PartCode == PartCode);
                if (found != null && found.Count() > 0)
                {
                    return Ok(new APIResponse()
                    {
                        ResponseCode = -1,
                        ResponseMessage = "This Part-Code is already in use!"
                    });
                }
                else
                {
                    return Ok(new APIResponse()
                    {
                        ResponseCode = 0,
                        ResponseMessage = "New Part Code OK!"
                    });
                }
            }
            // edit part
            else
            {
                if (PartCode == PreviousPartCode)
                    return Ok(new APIResponse()
                    {
                        ResponseCode = 0,
                        ResponseMessage = "New Part-Code OK!"
                    });

                var found = _unitOfWork.PartMasters.Find(x => x.PartCode == PartCode);
                if (found != null && found.Count() > 0)
                {
                    return Ok(new APIResponse()
                    {
                        ResponseCode = -1,
                        ResponseMessage = "This Part-Code is already in use!"
                    });
                }
                else
                {
                    return Ok(new APIResponse()
                    {
                        ResponseCode = 0,
                        ResponseMessage = "New Part Code OK!"
                    });
                }
            }         
        }


        [HttpPost, DisableRequestSizeLimit]
        [Route("partCreatePost")]
        public IActionResult PartCreatePost([FromForm] PartCreateDTO partCreateDto)
        {
            try
            {
                // throw new Exception();


                if (ModelState.IsValid)
                {

                    string PartDrgFile = string.Empty;

                    if (partCreateDto == null)
                    {
                        return BadRequest("Bad Request! Null Object!");
                    }

                    if (partCreateDto.PartCode == null)
                    {
                        return BadRequest("Bad Request! Null Object!");
                    }
                    if (partCreateDto.PartName == null)
                    {
                        return BadRequest("Bad Request! Null Object!");
                    }
                    if (partCreateDto.PartDesc == null)
                    {
                        return BadRequest("Bad Request! Null Object!");
                    }

                    var file = partCreateDto.PartFile;

                    if (file == null)
                    {
                        return BadRequest("Bad Request! No file content found!");
                    }
                    else
                    {
                        // @file system store
                        try
                        {
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

                            // within same solution/project
                            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), partFileStoragePath);

                            // outside of current solution
                            var outsideOfCurrentSolution = Path.Combine(@"C:\\Users\\ankit_2\\source\\repos\\PartTracking", partFileStoragePath);

                            if (file.Length > 0)
                            {
                                var fileName = inRangeValue + "_" + ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                                // var fullPath = Path.Combine(pathToSave, fileName);
                                var fullPath = Path.Combine(outsideOfCurrentSolution, fileName);

                                PartDrgFile = fileName;

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
                        catch (IOException ioEx)
                        {
                            return BadRequest("FAIL : IO EXCEPTION!");
                        }
                    }


                    // @db store
                    try
                    {
                        // add @ context
                        // need transaction for PartMaster and PartDetail
                        // add @ PartMaster and return PartMaster 
                        // add @ PartDetail using returned PartMaster
                        // sp covers all above,,,
                        partCreateDto.PartDrgFile = PartDrgFile;


                        var partMasterPartDetailAddVM = new PartMasterPartDetailsAddVM()
                        {
                            PartCode = partCreateDto.PartCode,
                            PartDesc = partCreateDto.PartDesc,
                            PartDrgFile = PartDrgFile,
                            PartName = partCreateDto.PartName,
                        };

                        // throw new Exception();
                        // sp call
                        var spResponse = _unitOfWork.PartMasters.SP_AddPartMasterWithPartDetail(partMasterPartDetailAddVM);
                        if(spResponse== "SUCCESS!")
                        {
                            return Ok(new APIResponse()
                            {
                                ResponseCode = 0,
                                ResponseMessage = spResponse + " : Part Created Successfully!"
                            });
                        }
                        else
                        {
                            return BadRequest(spResponse);
                        }                    
                    }
                    catch (Exception ex)
                    {
                        var spResponse = "FAIL : GENERAL EXCEPTION!";
                        return BadRequest(spResponse);
                    }
                }
                else
                {
                    return BadRequest(ModelState);
                }
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
