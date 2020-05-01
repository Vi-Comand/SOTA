using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SOTA.Controllers
{
    public class RabotaController : Controller
    {
        public IActionResult RabotaAdd()
        {
            return View();
        }
    }
}