using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Net;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using The_Masquerade.Models;

namespace The_Masquerade.Controllers
{
    public class TreeMenuController : Controller
    {
        private The_MasqueradeEntities db = new The_MasqueradeEntities();
        // GET: TreeMenu
        public ActionResult Index()
        {
            AspNetUser user = db.AspNetUsers.SingleOrDefault(u => u.Email == User.Identity.Name);
            return PartialView(user);
        }
    }
}