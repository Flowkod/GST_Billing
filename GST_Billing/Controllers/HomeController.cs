using GST_Billing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GST_Billing.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
       {
      //      if(User.Identity.IsAuthenticated)
      //{
      //< span > @((User as GST_Billing.Models.MyPrincipal).User.User_ID) </ span >
      //}
            
            ViewBag.Title = "Dashboard";

            return View();
        }
    }
}