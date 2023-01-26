using PartTracking.Context.Models.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;
using PartTracking.Context.Models.DTO;

namespace PartTracking.Service.Repository
{
    public interface IPartMasterRepository : IGenericRepository<PartMaster>
    {
        List<SelectListItem> GetPartMasterSelectList();
        string SP_AddPartMasterWithPartDetail(PartMasterPartDetailsAddVM partMasterpartDetail);
        string SP_EditPartMasterWithPartDetail(PartMasterPartDetailsEditVM partMasterpartDetail);
    }
}
