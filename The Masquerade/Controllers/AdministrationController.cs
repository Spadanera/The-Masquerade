using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using The_Masquerade.Models;

namespace The_Masquerade.Controllers
{
    public class AdministrationController : Controller
    {
        private LogWriter Log = new LogWriter("%_" + System.Web.HttpContext.Current.Session.SessionID.ToString(),
            Convert.ToInt32(WebConfigurationManager.AppSettings["LogLevel"]));
        private The_MasqueradeEntities db = new The_MasqueradeEntities();
        // GET: Administration
        public ActionResult Index()
        {
            AdministrationModel Admin = new AdministrationModel();
            return View(Admin);
        }

        public ActionResult Narrator()
        {
            NarratorModel Narrator = new NarratorModel(User.Identity.Name);
            return View(Narrator);
        }

        public ActionResult Player()
        {
            var email = User.Identity.Name;
            ViewBag.Player_id = db.Players.SingleOrDefault(p => p.eMail == email).System_Id;
            var stories = db.Players.SingleOrDefault(p => p.eMail == email).Stories.ToList();
            var storiesList = new List<string>();
            foreach (var s in stories)
                storiesList.Add(s.Name);
            ViewBag.Stories = storiesList;
            var model = db.Characters.Where(c => c.Player.eMail == email).ToList();
            return View(model);
        }
    }
}