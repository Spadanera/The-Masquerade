using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using The_Masquerade.Models;
using The_Masquerade.Models.Views;

namespace The_Masquerade.Controllers
{
    [Authorize(Roles = "Narrator")]
    public class ChroniclesController : Controller
    {
        private The_MasqueradeEntities db = new The_MasqueradeEntities();

        // GET: Chronicles
        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                ViewBag.Story = db.Stories.SingleOrDefault(s => s.System_id == id).Name;
                ViewBag.Story_id = id;
                var chronicleModel = new ChronicleModel(Convert.ToInt32(id));
                return View(chronicleModel);
            }
        }

        // GET: Chronicles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chronicle chronicle = db.Chronicles.Find(id);
            if (chronicle == null)
            {
                return HttpNotFound();
            }
            return View(chronicle);
        }

        // GET: Chronicles/Create
        public ActionResult Create(int? id)
        {
            if (id == null)
                id = db.Stories.First(s => s.AspNetUser.Email == User.Identity.Name).System_id;
            ViewBag.Story_id = id;
            return View();
        }

        // POST: Chronicles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "System_id,Story_id,Name,Description,Status")] Chronicle chronicle)
        {
            if (ModelState.IsValid)
            {
                db.Chronicles.Add(chronicle);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = chronicle.Story_id });
            }

            ViewBag.Story_id = new SelectList(db.Stories, "System_id", "Narrator_id", chronicle.Story_id);
            return View(chronicle);
        }

        // GET: Chronicles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chronicle chronicle = db.Chronicles.Find(id);
            if (chronicle == null)
            {
                return HttpNotFound();
            }
            ViewBag.Story_id = new SelectList(db.Stories, "System_id", "Narrator_id", chronicle.Story_id);
            return View(chronicle);
        }

        // POST: Chronicles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "System_id,Story_id,Name,Description,Status")] Chronicle chronicle)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chronicle).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = chronicle.Story_id });
            }
            ViewBag.Story_id = new SelectList(db.Stories, "System_id", "Narrator_id", chronicle.Story_id);
            return View(chronicle);
        }

        // GET: Chronicles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chronicle chronicle = db.Chronicles.Find(id);
            if (chronicle == null)
            {
                return HttpNotFound();
            }
            ViewBag.Story_id = chronicle.Story_id;
            return View(chronicle);
        }

        // POST: Chronicles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Chronicle chronicle = db.Chronicles.Find(id);
            DeleteModels.Delete(chronicle);
            return RedirectToAction("Index", new { id = chronicle.Story_id });
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
