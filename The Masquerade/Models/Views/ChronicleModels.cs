using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace The_Masquerade.Models.Views
{
    public class ChronicleModel
    {
        private The_MasqueradeEntities db = new The_MasqueradeEntities();
        public List<Character> PlayersCharacters { get; set; }
        public List<Character> NpcCharacters { get; set; }
        public List<Chronicle> Chronicles { get; set; }

        public ChronicleModel() { }

        public ChronicleModel(int id)
        {
            this.Chronicles = db.Chronicles.Where(c => c.Story_id == id).OrderByDescending(c => c.Status).ThenByDescending(c => c.System_id).ToList();
            this.PlayersCharacters = db.Characters.Where(c => c.Story_id == id).Where(c => c.Alive == "Alive").Where(c => c.Player.Type == "Player").ToList();
            this.NpcCharacters = db.Characters.Where(c => c.Story_id == id).Where(c => c.Alive == "Alive").Where(c => c.Player.Type == "NPC Group").ToList();
        }
    }

    public class HystoryModel
    {
        private The_MasqueradeEntities db = new The_MasqueradeEntities();
        public Chronicle Chronicle { get; set; }
        public List<Session_Details> Sessions { get; set; }

        public HystoryModel() { }

        public HystoryModel(int chronicle_id, int character_id)
        {
            this.Chronicle = db.Chronicles.Find(chronicle_id);
            this.Sessions = db.Session_Details.Where(s => s.Session.Chronicle_id == chronicle_id).Where(s => s.Character_id == character_id).OrderByDescending(s => s.Session.Date).ToList();
        }
    }
}