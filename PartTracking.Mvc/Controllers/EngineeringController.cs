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
using PartTracking.Mvc.Extentions;

namespace PartTracking.Mvc.Controllers
{
    public class EngineeringController : Controller
    {
        private readonly ILogger<EngineeringController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public EngineeringController(ILogger<EngineeringController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
 
        [HttpGet]
        public IActionResult Index(string searchString)
        {
            List<PartMaster> lstPartMaster = SessionHelper.GetObjectFromJson<List<PartMaster>>(HttpContext.Session, "_Parts");
            if (lstPartMaster == null)
            {
                lstPartMaster = _unitOfWork.PartMasters.GetAll().ToList();
                SessionHelper.SetObjectAsJson(HttpContext.Session, "_Parts", lstPartMaster);
            }        
            
            if (!String.IsNullOrEmpty(searchString))
            {
                lstPartMaster = lstPartMaster
                                    .Where(x => x.PartCode.ToLower().Contains(searchString.ToLower()) || x.PartName.ToLower().Contains(searchString.ToLower())).ToList();
                                    
                return View(lstPartMaster);
            }
            return View(lstPartMaster);
        }

        [HttpGet]
        public IActionResult CreatePart()
        {
            return View();
        }

        [HttpPost("AddPartMasterPartDetail")]
        public async Task<IActionResult> AddPartMasterPartDetail(List<IFormFile> files, PartMasterPartDetailsAddVM partMasterpartDetail)
        {
            string spResponse = "";

            if (ModelState.IsValid)
            {
                List<string> drgFiles = new List<string>();
                try
                {        
                    // add @ file system
                    long size = files.Sum(f => f.Length);

                    if (size < 1)
                    {
                        // add ModelState error for file upload
                        // file not attached
                        ModelState.AddModelError("PartDrgFile","Drawing File Not Attached!");
                        return View("CreatePart", partMasterpartDetail);
                    }

                    var filePaths = new List<string>();
                    foreach (var formFile in files)
                    {                        
                        if (formFile.Length > 0)
                        {
                            var uniqueFileName = GetUniqueFileName(formFile.FileName);
                            string SavePath = Path.Combine(Directory.GetCurrentDirectory(), "Parts", uniqueFileName);
                            filePaths.Add(SavePath);
                            using (var stream = new FileStream(SavePath, FileMode.Create))
                            {
                                await formFile.CopyToAsync(stream);
                            }
                            drgFiles.Add(uniqueFileName);
                        }
                    }

                    // add @ context
                    // need transaction for PartMaster and PartDetail
                    // add @ PartMaster and return PartMaster 
                    // add @ PartDetail using returned PartMaster
                    // sp covers all above,,,
                    partMasterpartDetail.PartDrgFile = drgFiles[0];
                    spResponse = _unitOfWork.PartMasters.SP_AddPartMasterWithPartDetail(partMasterpartDetail);
                    TempData["SPResponse"] = spResponse;
                    // return RedirectToAction("Index");
                    ModelState.Clear();
                    return RedirectToAction("CreatePart");
                    // return Ok(new { count = files.Count, size, filePaths });
                }
                catch(IOException ioEx)
                {
                    TempData["SPResponse"] = "FAIL : IO EXCEPTION!";
                }
                catch (Exception ex)
                {
                    TempData["SPResponse"] = "FAIL : GENERAL EXCEPTION!";
                }
            }
            return View("CreatePart", partMasterpartDetail);
        }
        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                      + "_"
                      + Guid.NewGuid().ToString().Substring(0, 4)
                      + Path.GetExtension(fileName);
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyPartCode(string PartCode)
        {
            var found = _unitOfWork.PartMasters.Find(x => x.PartCode == PartCode);
            if (found != null && found.Count()>0)
            {
                return Json(data: "[ " + PartCode + " ] is already used!!");
            }
            else
            {
                return Json(true);
            }
        }
    
        [HttpGet]
        [Route("Engineering/PartDetails/{id:int}")]
        public IActionResult PartDetails(int id)
        {            
            var partMaster = _unitOfWork.PartMasters.GetById(id);
            var partDetail = _unitOfWork.PartDetails.Find(x => x.PartMasterId == id);
            if(partMaster!=null && partDetail != null && partDetail.Count()>0)
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
                return View(partMasterpartDetails);
            }
            else
            {
                return View(new PartMasterPartDetailsView());
            }            
        }

        [HttpGet]
        [Route("Engineering/PartEdit/{id:int}")]
        public IActionResult PartEdit(int id)
        {
            var partMaster = _unitOfWork.PartMasters.GetById(id);
            var partDetail = _unitOfWork.PartDetails.Find(x => x.PartMasterId == id);
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
                return View(partMasterpartDetailsEdit);
            }
            else
            {
                return View(new PartMasterPartDetailsEditVM());
            }
        }

        [HttpPost("EditPartMasterPartDetail")]
        public async Task<IActionResult> EditPartMasterPartDetail(List<IFormFile> files, PartMasterPartDetailsEditVM partMasterpartDetail)
        {
            string spResponse = "";

            if (ModelState.IsValid)
            {
                List<string> drgFiles = new List<string>();
                try
                {
                    if (files.Count()>0 && files!=null)
                    {
                        // add @ file system
                        long size = files.Sum(f => f.Length);

                        var filePaths = new List<string>();
                    
                        // drawing file updated
                        foreach (var formFile in files)
                        {
                            if (formFile.Length > 0)
                            {
                                var uniqueFileName = GetUniqueFileName(formFile.FileName);
                                string SavePath = Path.Combine(Directory.GetCurrentDirectory(), "Parts", uniqueFileName);
                                filePaths.Add(SavePath);
                                using (var stream = new FileStream(SavePath, FileMode.Create))
                                {
                                    await formFile.CopyToAsync(stream);
                                }
                                drgFiles.Add(uniqueFileName);
                            }
                        }
                        partMasterpartDetail.PartDrgFile = drgFiles[0];
                    }
                    else
                    {
                        // drawing file as it was
                        partMasterpartDetail.PartDrgFile = partMasterpartDetail.PreviousPartDrgFile;
                    }
               
                    
                    spResponse = _unitOfWork.PartMasters.SP_EditPartMasterWithPartDetail(partMasterpartDetail);
                    TempData["SPResponse"] = spResponse;
                    ModelState.Clear();
                    return RedirectToAction("Index");
                }
                catch (IOException ioEx)
                {
                    TempData["SPResponse"] = "FAIL : IO EXCEPTION!";
                }
                catch (Exception ex)
                {
                    TempData["SPResponse"] = "FAIL : GENERAL EXCEPTION!";
                }
            }
            return View("PartEdit", partMasterpartDetail);
        }
        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyPartCodeEditOperation(string PreviousPartCode, string PartCode)
        {
            if(PartCode==PreviousPartCode)
                return Json(true);

            var found = _unitOfWork.PartMasters.Find(x => x.PartCode == PartCode);
            if (found != null && found.Count() > 0)
            {                
                return Json(data: "[ " + PartCode + " ] is already used!!");
            }
            else
            {
                return Json(true);
            }
        }

    }
}
