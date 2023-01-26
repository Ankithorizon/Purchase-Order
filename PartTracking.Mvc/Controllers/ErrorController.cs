using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PartTracking.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace PartTracking.Mvc.Controllers
{
    public class ErrorController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public ErrorController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public IActionResult Http(int statusCode)
        {
            if (statusCode == 404)
                return View("Error404");
            else
                return View("ErrorGeneral");
        }
    }
}
