using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using The_Masquerade.Models;
using The_Masquerade.Models.Views;

namespace The_Masquerade.Controllers
{
    [Authorize(Roles = "Narrator, Player")]
    public class CharactersController : Controller
    {
        private LogWriter Log = new LogWriter("%_" + System.Web.HttpContext.Current.Session.SessionID.ToString(),
            Convert.ToInt32(WebConfigurationManager.AppSettings["LogLevel"]));
        private The_MasqueradeEntities db = new The_MasqueradeEntities();

        public void UpdateRecentItems(Character input)
        {
            try
            {
                var recentItems = (Queue<Character>)(System.Web.HttpContext.Current.Session["RecentItems"]);
                bool finded = false;

                foreach (var item in recentItems)
                {
                    if (item.System_id == input.System_id)
                        finded = true;
                }

                if (!finded)
                    recentItems.Enqueue(input);
                if (recentItems.Count > 6)
                    recentItems.Dequeue();
                System.Web.HttpContext.Current.Session["RecentItems"] = recentItems;
            }
            catch { }
        }

        #region List
        private List<string> Disciplines = new List<string> {
            "Animalism",
            "Assamite Sorcery",
            "Auspex",
            "Celerity",
            "Chimerstry",
            "Daimoinon",
            "Dark Thaumaturgy",
            "Dementation",
            "Dominate",
            "Flight",
            "Fortitude",
            "Koldunic Sorcery",
            "Melpominee",
            "Mortis",
            "Mytherceria",
            "Necromancy",
            "Nihilistics",
            "Obeah",
            "Obfuscate",
            "Obtenebration",
            "Potence",
            "Presence",
            "Protean",
            "Quietus",
            "Sanguinus",
            "Serpentis",
            "Setite Sorcery",
            "Temporis",
            "Thanatosis",
            "Thaumaturgy",
            "Thaumaturgical Countermagic",
            "Valeren",
            "Vicissitue",
            "Visceratika"
        };

        private List<string> Backgrounds = new List<string> {
            "Allies",
            "Alternate Identity",
            "Black Hand Membership",
            "Contanct",
            "Domain",
            "Fame",
            "Generation",
            "Herd",
            "Influence",
            "Mentor",
            "Resources",
            "Retainers",
            "Rituals",
            "Status",
            "Mentor"
        };

        private List<SelectListItem> Clans = new List<SelectListItem> {
            new SelectListItem { Text = "Assamite", Value = "Assamite"},
            new SelectListItem { Text = "Baali", Value = "Baali"},
            new SelectListItem { Text = "Blood Brothers", Value = "Blood Brothers"},
            new SelectListItem { Text = "Brujah", Value = "Brujah"},
            new SelectListItem { Text = "Caitiff", Value = "Caitiff"},
            new SelectListItem { Text = "Cappadocian", Value = "Cappadocian"},
            new SelectListItem { Text = "Followers of Set", Value = "Followers of Set"},
            new SelectListItem { Text = "Gangrel", Value = "Gangrel"},
            new SelectListItem { Text = "Gargoyle", Value = "Caitiff"},
            new SelectListItem { Text = "Giovanni", Value = "Giovanni"},
            new SelectListItem { Text = "Harbingers of Skulls", Value = "Harbingers of Skulls"},
            new SelectListItem { Text = "Inconnus", Value = "Caitiff"},
            new SelectListItem { Text = "Kiasyd", Value = "Kiasyd"},
            new SelectListItem { Text = "Lasombra", Value = "Lasombra"},
            new SelectListItem { Text = "Malkavian", Value = "Malkavian"},
            new SelectListItem { Text = "Nagaraja", Value = "Caitiff"},
            new SelectListItem { Text = "Nosferatu", Value = "Nosferatu"},
            new SelectListItem { Text = "Ravnos", Value = "Ravnos"},
            new SelectListItem { Text = "Salubri", Value = "Salubri"},
            new SelectListItem { Text = "Samedi", Value = "Caitiff"},
            new SelectListItem { Text = "Toreador", Value = "Toreador"},
            new SelectListItem { Text = "Tremere", Value = "Tremere"},
            new SelectListItem { Text = "Tzimisce", Value = "Tzimisce"},
            new SelectListItem { Text = "Ventrue", Value = "Ventrue"}
        };

        private List<SelectListItem> Generations = new List<SelectListItem> {
            new SelectListItem { Text = "4a", Value = "4a"},
            new SelectListItem { Text = "5a", Value = "5a"},
            new SelectListItem { Text = "6a", Value = "6a"},
            new SelectListItem { Text = "7a", Value = "7a"},
            new SelectListItem { Text = "8a", Value = "8a"},
            new SelectListItem { Text = "9a", Value = "9a"},
            new SelectListItem { Text = "10a", Value = "10a"},
            new SelectListItem { Text = "11a", Value = "11a"},
            new SelectListItem { Text = "12a", Value = "12a"},
            new SelectListItem { Text = "13a", Value = "13a"},
            new SelectListItem { Text = "14a", Value = "14a"}
        };

        #endregion

        // GET: Characters
        public ActionResult Index(int? id)
        {
            var characters = db.Characters.Where(c => c.Parent_id == id).Include(c => c.Player).Include(c => c.Story);
            ViewBag.Player_id = id;
            ViewBag.Player = db.Players.Find(id).UserName;
            return View(characters.ToList());
        }

        // GET: Characters/Details/5
        public ActionResult Details(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CharactersModel model = new CharactersModel(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            ViewBag.Clans = Clans;
            ViewBag.Generations = Generations;
            ViewBag.Disciplines = Disciplines;
            ViewBag.Backgrounds = Backgrounds;
            ViewBag.Story_id = new SelectList(db.Stories, "System_id", "Name");
            UpdateRecentItems(model.Character);
            return View(model);
        }

        // GET: Characters/Create
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.Clans = Clans;
            ViewBag.Generations = Generations;
            ViewBag.Player_id = id;
            ViewBag.Story_id = new SelectList(db.Players.Find(id).Stories, "System_id", "Name");
            var model = new CharactersModel();
            model.Character.Player = db.Players.Find(id);
            model.Character.Parent_id = Convert.ToInt32(id);
            return View(model);
        }

        // POST: Characters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CharactersModel characterModel, List<Character_Details> detailsList)
        {
            var player_id = Convert.ToInt32(TempData["Player_id"]);
            if (ModelState.IsValid)
            {
                characterModel.Character.Parent_id = player_id;
                Log.WriteLog("Creazione del character per il player: " + player_id.ToString(), 2);
                db.Characters.Add(characterModel.Character);
                db.SaveChanges();
                Log.WriteLog("New character salvato", 2);

                foreach (var item in detailsList)
                {
                    item.Character_id = characterModel.Character.System_id;
                    if (item.Section != "Deleted")
                    {
                        db.Character_Details.Add(item);
                        db.SaveChanges();
                        Log.WriteLog(string.Format("Salvato il dettagli {0} per il character {1}", item.Name, player_id.ToString()), 2);
                    }
                }
                characterModel.SetMaxBloodPool();
                Log.WriteLog(string.Format("Settato il MaxBloodPool per il character: {0}", characterModel.Character.Name), 2);
                return RedirectToAction("Index", new { id = TempData["Player_id"] });
            }

            ViewBag.Clans = Clans;
            ViewBag.Generations = Generations;
            ViewBag.Story_id = new SelectList(db.Stories, "System_id", "Name");
            ViewBag.Player_id = db.Players.SingleOrDefault(p => p.System_Id == player_id);
            ViewBag.Story_id = new SelectList(db.Stories, "System_id", "Name");
            return View(characterModel.Character);
        }

        // GET: Characters/Edit/5
        public ActionResult Edit(int id)
        {
            CharactersModel model = new CharactersModel(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            ViewBag.Clans = Clans;
            ViewBag.Generations = Generations;
            ViewBag.Disciplines = Disciplines;
            ViewBag.Backgrounds = Backgrounds;
            ViewBag.Story_id = new SelectList(db.Players.Find(model.Character.Parent_id).Stories, "System_id", "Name");
            UpdateRecentItems(model.Character);
            return View(model);
        }

        // POST: Characters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CharactersModel model, List<Character_Details> detailsList)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(model.Character).State = EntityState.Modified;
                    db.SaveChanges();
                    Log.WriteLog("Salvate le modifiche generali per il personaggio: " + model.Character.Name, 2);

                    for (int i = 0; i < detailsList.Count; i++)
                    {

                        if (detailsList[i].System_id != 0)
                        {
                            Log.WriteLog(string.Format("Salvataggio delle modfiche del dettaglio {0} per il character {1}",
                                detailsList[i].Name, detailsList[i].Character_id.ToString()), 2);
                            db.Entry(detailsList[i]).State = EntityState.Modified;
                        }
                        else
                        {
                            Log.WriteLog(string.Format("Inserimento del nuovo dettaglio {0} per il personaggio {1}",
                                detailsList[i].Name, model.Character.System_id), 2);
                            detailsList[i].Character_id = model.Character.System_id;
                            db.Character_Details.Add(detailsList[i]);
                        }
                    }
                    db.SaveChanges();
                    Log.WriteLog("Modifiche ai dettagli effettuate", 2);
                    model.SetMaxBloodPool();
                    Log.WriteLog(string.Format("Settato il MaxBloodPool per il character: {0}", model.Character.Name), 2);
                }
                catch (Exception ex)
                {
                    Log.WriteLog(string.Format("Errore nel salvatagio delle modifiche per il personaggio {0}: {1}",
                        model.Character.System_id, ex.Message), 1);
                }
                return RedirectToAction("Edit", new { id = model.Character.System_id });
            }
            ViewBag.Clans = Clans;
            ViewBag.Generations = Generations;
            ViewBag.Disciplines = Disciplines;
            ViewBag.Backgrounds = Backgrounds;
            ViewBag.Story_id = new SelectList(db.Stories, "System_id", "Name");
            return View(model);
        }

        // GET: Characters/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Character character = db.Characters.Find(id);
            if (character == null)
            {
                return HttpNotFound();
            }
            return View(character);
        }

        // POST: Characters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Character character = db.Characters.Find(id);
            DeleteModels.Delete(character);
            if (User.IsInRole("Narrator"))
                return RedirectToAction("Index", new { id = character.Parent_id });
            else
                return RedirectToAction("Player", "Administration");
        }

        public ActionResult Experience_Points(int story_id)
        {
            var model = db.Characters.Where(c => c.Story_id == story_id).Where(c => c.Alive == "Alive").ToList();
            ViewBag.Story_id = story_id;
            return View(model);
        }

        public ActionResult Hystory(int id)
        {
            var model = new List<HystoryModel>();
            var Story_id = db.Characters.Find(id).Story_id;
            var chronicles = db.Chronicles.Where(c => c.Story_id == Story_id).OrderByDescending(c => c.System_id).ToList();
            foreach (var chronicle in chronicles)
            {
                model.Add(new HystoryModel(chronicle.System_id, id));
            }
            ViewBag.Story_id = Story_id;
            return View(model);
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
