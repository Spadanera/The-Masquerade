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
    [Authorize(Roles="Narrator")]
    public class StoriesController : Controller
    {
        private The_MasqueradeEntities db = new The_MasqueradeEntities();

        // GET: Stories
        public ActionResult Index(string id)
        {
            var user = db.AspNetUsers.SingleOrDefault(u => u.Email == User.Identity.Name);
            var stories = db.Stories.Include(s => s.AspNetUser).Where(st => st.Narrator_id == user.Id).OrderByDescending(s => s.System_id);
            return View(stories.ToList());
        }

        // GET: Stories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Story story = db.Stories.Find(id);
            if (story == null)
            {
                return HttpNotFound();
            }
            return View(story);
        }

        // GET: Stories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Stories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "System_id,Name,Description")] Story story)
        {
            if (ModelState.IsValid)
            {
                story.Narrator_id = db.AspNetUsers.SingleOrDefault(u => u.Email == User.Identity.Name).Id;
                db.Stories.Add(story);
                db.SaveChanges();
                return RedirectToAction("Narrator", "Administration", null);
            }

            ViewBag.Narrator_id = new SelectList(db.AspNetUsers, "Id", "Email", story.Narrator_id);
            return View(story);
        }

        // GET: Stories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Story story = db.Stories.Find(id);
            if (story == null)
            {
                return HttpNotFound();
            }
            return View(story);
        }

        // POST: Stories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "System_id,Name,Description")] Story story)
        {
            if (ModelState.IsValid)
            {
                story.Narrator_id = db.AspNetUsers.SingleOrDefault(u => u.Email == User.Identity.Name).Id;
                db.Entry(story).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Narrator", "Administration", null);
            }
            return View(story);
        }

        // GET: Stories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Story story = db.Stories.Find(id);
            if (story == null)
            {
                return HttpNotFound();
            }
            return View(story);
        }

        // POST: Stories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Story story = db.Stories.Find(id);
            DeleteModels.Delete(story);
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
