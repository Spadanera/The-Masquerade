using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace The_Masquerade.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Narrator"))
                    return RedirectToAction("Narrator", "Administration");
                else if (User.IsInRole("Player"))
                    return RedirectToAction("Player", "Administration");
                else
                    return RedirectToAction("Index", "Administration");
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Here you can find the way to contact me!";

            return View();
        }
    }
}