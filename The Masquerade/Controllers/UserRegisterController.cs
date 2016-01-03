using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using The_Masquerade.Models;

namespace The_Masquerade.Controllers
{
    public class UserRegisterController : Controller
    {
        private The_MasqueradeEntities db = new The_MasqueradeEntities();

        // GET: Invitation
        public ActionResult Invitation(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var player = db.Players.Find(id);
            var model = new UserRegisterModels(player);
            return View(model);
        }

        //POST: Invitation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Invitation(UserRegisterModels model)
        {
            model.SendInvitation();
            //return RedirectToAction("ThankYou");
            return PartialView("ThankYou");
        }

        public PartialViewResult ThankYou()
        {
            return PartialView();
        }

    }
}