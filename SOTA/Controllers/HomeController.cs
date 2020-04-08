﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SOTA.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SOTA.Controllers
{
    public class HomeController : Controller
    {

       
        SotaContext db;

        public HomeController(SotaContext context)
        {
            db = context;
        }
         [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SpecifikacList()
        {
            return View();
        }

        public IActionResult SpecifikacAdd()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Open()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
