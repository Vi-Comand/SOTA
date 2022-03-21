using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SOTA.Models;
using System;
using System.Linq;

namespace SOTA.Controllers
{
    [Authorize]
    public class UchenController : Controller
    {
        SotaContext db;

        public UchenController(SotaContext context)
        { nf=1;
            db = context;
        }
        // [HttpGet]
        //[Route("Rabota/RabotaAdd/")]
        [Authorize]
        public IActionResult NaznacRabotaList()
        {

            var login = HttpContext.User.Identity.Name;
            Users user = db.Users.Where(p => p.Name == login).First();
            ViewBag.rl = user.Role;
            FormirRabotaTablList formir = new FormirRabotaTablList(user, db);
            RabotaUchenList rabotaList = new RabotaUchenList();

            rabotaList = formir.GetSpisokRabotUchen();


            return View(rabotaList);
        }
    }
}
