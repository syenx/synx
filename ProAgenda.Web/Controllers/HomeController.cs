using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProAgenda.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(int? year, int? month)
        {
            return RedirectToAction("Index", "Calendar");
            
        }
    }
}