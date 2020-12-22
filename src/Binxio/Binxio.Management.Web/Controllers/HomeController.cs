﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Binxio.Management.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Binxio.Abstractions;

namespace Binxio.Management.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProjectManager pm;

        public HomeController(ILogger<HomeController> logger, IProjectManager pm)
        {
            _logger = logger;
            this.pm = pm;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult TestBg()
        {
            return Json(pm.Create(new Common.Projects.ProjectCreateModel { }).Result);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
