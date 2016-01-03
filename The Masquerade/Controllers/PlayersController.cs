using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Threading.Tasks;
using The_Masquerade.Models;
using The_Masquerade.Models.Views;
using System.Security.Claims;

namespace The_Masquerade.Controllers
{
    [Authorize(Roles = "Narrator")]
    public class PlayersController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public PlayersController()
        {
        }

        public PlayersController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private The_MasqueradeEntities db = new The_MasqueradeEntities();
        public static string[] PlayerTypes = { "Player", "NPC Group" };
        public static string[] Status = { "Enable", "Disable" };
        
        // GET: Players
        public ActionResult Index()
        {
            var user = db.AspNetUsers.SingleOrDefault(u => u.Email == User.Identity.Name);
            var players = db.Players.Include(p => p.AspNetUser).Where(st => st.Narrator_id == user.Id);
            return View(players.ToList());
        }

        // GET: Players/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            string Narrator_id = db.AspNetUsers.SingleOrDefault(u => u.Email == User.Identity.Name).Id;
            PlayersModel playerModel = new PlayersModel();
            playerModel.Player = db.Players.Find(id);
            playerModel.AllStories = db.Stories.Where(s => s.Narrator_id == Narrator_id).ToList();
            playerModel.UserStories = db.Players.Find(id).Stories.ToList();
            playerModel.SelectedStories = db.Players.Find(id).Stories.Select(x => x.System_id).ToArray();
            return View(playerModel);
        }

        // GET: Players/Create
        public ActionResult Create()
        {
            string Narrator_id = db.AspNetUsers.SingleOrDefault(u => u.Email == User.Identity.Name).Id;
            ViewBag.Narrator_id = Narrator_id;
            PlayersModel playerModel = new PlayersModel(Narrator_id);
            return View(playerModel);
        }

        // POST: Players/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PlayersModel playerModel)
        {
            if (ModelState.IsValid)
            {
                if (playerModel.Player.Type == "NPC Group")
                    playerModel.Player.eMail = "";
                playerModel.Player.Narrator_id = Convert.ToString(TempData["Narrator_id"]);
                playerModel.Player.Stories = playerModel.UserStories;
                foreach (int i in playerModel.SelectedStories)
                {
                    db.Stories.Find(i).Players.Add(playerModel.Player);
                }
                db.Players.Add(playerModel.Player);
                db.SaveChanges();

                return RedirectToAction("Narrator", "Administration", null);
            }

            ViewBag.Narrator_id = new SelectList(db.AspNetUsers, "Id", "Email", playerModel.Player.Narrator_id);
            return View(playerModel);
        }

        // GET: Players/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            string Narrator_id = db.AspNetUsers.SingleOrDefault(u => u.Email == User.Identity.Name).Id;
            PlayersModel playerModel = new PlayersModel();
            playerModel.Player = db.Players.Find(id);
            playerModel.AllStories = db.Stories.Where(s => s.Narrator_id == Narrator_id).ToList();
            playerModel.UserStories = db.Players.Find(id).Stories.ToList();
            playerModel.SelectedStories = db.Players.Find(id).Stories.Select(x => x.System_id).ToArray();
            return View(playerModel);
        }

        // POST: Players/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PlayersModel playerModel)
        {
            if (ModelState.IsValid)
            {
                if (playerModel.Player.Type == "NPC Group")
                    playerModel.Player.eMail = "";
                db.Entry(playerModel.Player).State = EntityState.Modified;
                db.SaveChanges();

                foreach (Story s in db.Stories)
                {
                    if (playerModel.SelectedStories != null)
                    {
                        if (s.Players.Contains(playerModel.Player) && !playerModel.SelectedStories.Contains(s.System_id))
                        {
                            s.Players.Remove(playerModel.Player);
                            db.Players.Find(playerModel.Player.System_Id).Stories.Remove(s);
                        }
                        else if (!s.Players.Contains(playerModel.Player) && playerModel.SelectedStories.Contains(s.System_id))
                        {
                            s.Players.Add(playerModel.Player);
                            db.Players.Find(playerModel.Player.System_Id).Stories.Add(s);
                        }
                    }
                    else
                    {
                        s.Players.Remove(playerModel.Player);
                        db.Players.Find(playerModel.Player.System_Id).Stories.Remove(s);
                    }
                }
                db.SaveChanges();

                return RedirectToAction("Narrator", "Administration", null);
            }
            
            ViewBag.Types = new SelectList(new GetSelectList(PlayerTypes));
            ViewBag.Status = new SelectList(new GetSelectList(Status));
            return View(playerModel);
        }

        // GET: Players/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Player player = db.Players.Find(id);
            if (player == null)
            {
                return HttpNotFound();
            }
            return View(player);
        }

        // POST: Players/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Player player = db.Players.Find(id);
            DeleteModels.Delete(player);
            return RedirectToAction("Narrator", "Administration", null);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
