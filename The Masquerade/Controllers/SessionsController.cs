using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using The_Masquerade.Models;

namespace The_Masquerade.Controllers
{
    [Authorize(Roles = "Narrator")]
    public class SessionsController : Controller
    {
        private The_MasqueradeEntities db = new The_MasqueradeEntities();

        // GET: Sessions
        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chronicle chronicle = db.Chronicles.SingleOrDefault(c => c.System_id == id);
            Story story = db.Stories.SingleOrDefault(s => s.System_id == chronicle.Story_id);
            ViewBag.Chronicle = chronicle.Name;
            ViewBag.Chronicle_id = chronicle.System_id;
            ViewBag.Story = story.Name;
            ViewBag.Story_id = story.System_id;
            var sessions = db.Sessions.Include(s => s.Chronicle).Where(c => c.Chronicle_id == chronicle.System_id).OrderByDescending(c => c.Date);
            return View(sessions.ToList());
        }

        // GET: Sessions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Session session = db.Sessions.Find(id);
            if (session == null)
            {
                return HttpNotFound();
            }
            Chronicle chronicle = db.Chronicles.SingleOrDefault(c => c.System_id == session.Chronicle_id);
            ViewBag.Players = new SelectList(chronicle.Story.Players, "System_id", "UserName");
            return View(session);
        }

        // GET: Sessions/Create
        public ActionResult Create(int? id)
        {
            Chronicle chronicle = db.Chronicles.SingleOrDefault(c => c.System_id == id);
            ViewBag.Chronicle_id = chronicle.System_id;
            Session model = new Session(chronicle.Story_id);
            model.Date = DateTime.Today;
            ViewBag.Players = new SelectList(chronicle.Story.Players.Where(p => p.Type == "Player"), "System_id", "UserName");
            return View(model);
        }

        // POST: Sessions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Session session, List<Session_Details> session_details)
        {
            if (ModelState.IsValid)
            {
                session.Chronicle_id = Convert.ToInt32(TempData["Chronicle_id"]);
                db.Sessions.Add(session);
                db.SaveChanges();

                foreach (Session_Details details in session_details)
                {
                    details.Session_id = session.System_id;
                    details.Session = session;
                    db.Session_Details.Add(details);
                    db.SaveChanges();

                    var character = db.Characters.Find(details.Character_id);
                    character.Experience_Points += details.Experience_Points;
                    character.EP_Available += details.Experience_Points;
                    db.Entry(character).State = EntityState.Modified;
                    db.SaveChanges();
                }
                //db.SaveChanges();
                return RedirectToAction("Index", new { id = session.Chronicle_id });
            }
            
            ViewBag.Chronicle_id = new SelectList(db.Chronicles, "System_id", "Name", session.Chronicle_id);
            return View(session);
        }

        // GET: Sessions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Session session = db.Sessions.Find(id);
            if (session == null)
            {
                return HttpNotFound();
            }
            Chronicle chronicle = db.Chronicles.SingleOrDefault(c => c.System_id == session.Chronicle_id);
            ViewBag.Players = new SelectList(chronicle.Story.Players.Where(p => p.Type == "Player"), "System_id", "UserName");
            return View(session);
        }

        // POST: Sessions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[Bind(Include = "System_id,Chronicle_id,Date,Notes,Extra_Notes,Session_Player")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Session session, List<Session_Details> session_details)
        {
            if (ModelState.IsValid)
            {
                db.Entry(session).State = EntityState.Modified;
                db.SaveChanges();

                foreach (Session_Details details in session_details)
                {
                    var character = db.Characters.Find(details.Character_id);
                    character.Experience_Points += details.Experience_Points - Convert.ToInt32(TempData["EP_" + details.System_id.ToString()]);
                    character.EP_Available += details.Experience_Points - Convert.ToInt32(TempData["EP_" + details.System_id.ToString()]);
                    db.Entry(character).State = EntityState.Modified;
                    db.SaveChanges();

                    db.Entry(details).State = EntityState.Modified;
                    db.SaveChanges();
                }

                return RedirectToAction("Index", new { id = session.Chronicle_id });
            }
            ViewBag.Chronicle_id = new SelectList(db.Chronicles, "System_id", "Name", session.Chronicle_id);
            return View(session);
        }

        // GET: Sessions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Session session = db.Sessions.Find(id);
            if (session == null)
            {
                return HttpNotFound();
            }
            ViewBag.Chronicle_id = session.Chronicle_id;
            return View(session);
        }

        // POST: Sessions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Session session = db.Sessions.Find(id);
            DeleteModels.Delete(session);
            return RedirectToAction("Index", new { id = session.Chronicle_id });
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
