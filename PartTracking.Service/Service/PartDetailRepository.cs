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
    public class PartDetailRepository : GenericRepository<PartDetail>, IPartDetailRepository
    {
        public PartDetailRepository(PartMgtContext context) : base(context)
        {      
         
        }     
    }
}
