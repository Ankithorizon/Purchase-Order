using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;
using PartTracking.Context.Models.Models;
using PartTracking.Service.Repository;
using Microsoft.AspNetCore.Mvc.Rendering;
using PartTracking.Context.Models.DTO;

namespace PartTracking.Service.Service
{
    public class PartMasterRepository : GenericRepository<PartMaster>, IPartMasterRepository
    {
        public PartMasterRepository(PartMgtContext context) : base(context)
        {      
         
        }
        public List<SelectListItem> GetPartMasterSelectList()
        {
            List<SelectListItem> datas = new List<SelectListItem>();

            foreach(var part in _context.PartMaster)
            {
                datas.Add(new SelectListItem()
                {
                    Value = part.PartMasterId+"",
                    Text = part.PartCode + " - [ "+ part.PartName +" ] "
                });
            }
            return datas;
        }

        public string SP_AddPartMasterWithPartDetail(PartMasterPartDetailsAddVM partMasterpartDetail)
        {
            var partCodeParam = new SqlParameter("@PartCode", partMasterpartDetail.PartCode);
            var partNameParam = new SqlParameter("@PartName", partMasterpartDetail.PartName);
            var partDescParam = new SqlParameter("@PartDesc", partMasterpartDetail.PartDesc);
            var partDrgFileParam = new SqlParameter("@PartDrgFile", partMasterpartDetail.PartDrgFile);

            var partMasterIdParam = new SqlParameter("@id", SqlDbType.Int);
            partMasterIdParam.Direction = ParameterDirection.Output;
            _context.Database.ExecuteSqlRaw("exec AddPartMasterWithPartDetail @PartCode,@PartName, @PartDesc, @PartDrgFile, @id out",
                            partCodeParam, partNameParam, partDescParam, partDrgFileParam, partMasterIdParam);

            if (Convert.ToInt32(partMasterIdParam.Value) > 0)
            {
                // success
                return "SUCCESS!";
            }
            else
            {
                // fail
                return "FAIL!... STORED PROCEDURE ERROR!";
            }            
        }

        public string SP_EditPartMasterWithPartDetail(PartMasterPartDetailsEditVM partMasterpartDetail)
        {
            var partCodeParam = new SqlParameter("@PartCode", partMasterpartDetail.PartCode);
            var partNameParam = new SqlParameter("@PartName", partMasterpartDetail.PartName);
            var partDescParam = new SqlParameter("@PartDesc", partMasterpartDetail.PartDesc);
            var partDrgFileParam = new SqlParameter("@PartDrgFile", partMasterpartDetail.PartDrgFile);
            var partMasterIdParam = new SqlParameter("@PartMasterId", partMasterpartDetail.PartMasterId);
            var partDetailIdParam = new SqlParameter("@PartDetailId", partMasterpartDetail.PartDetailId);
            var retCode = new SqlParameter("@retCode", SqlDbType.Int);
            retCode.Direction = ParameterDirection.Output;

            try
            {
                _context.Database.ExecuteSqlRaw("exec EditPartMasterWithPartDetail @PartCode,@PartName, @PartDesc, @PartDrgFile, @PartMasterId, @PartDetailId, @retCode out",
                        partCodeParam, partNameParam, partDescParam, partDrgFileParam, partMasterIdParam, partDetailIdParam, retCode);

                if(Convert.ToInt32(retCode.Value) == 0)
                {
                    // success
                    return "SUCCESS!";
                }
                else
                {
                    // fail
                    return "FAIL!... STORED PROCEDURE ERROR!";
                }
            }
            catch(Exception ex)
            {
                return "FAIL!... STORED PROCEDURE ERROR!";
            }        
        }
    }
}
