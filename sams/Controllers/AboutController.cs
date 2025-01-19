using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using sams.Common;
using sams.Models;

namespace sams.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public ActionResult LogOutCustomer()
        {
            HttpContext.Session.SetObjectAsJson("LoggedInUser", new CustomerViewModel());
            ViewData["LoggedInUserName"] = null;
            return RedirectToAction("Index", "Home");
        }
    }
}