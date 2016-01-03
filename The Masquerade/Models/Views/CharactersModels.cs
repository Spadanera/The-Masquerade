using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace The_Masquerade.Models.Views
{
    public class CharactersModel
    {
        private The_MasqueradeEntities db = new The_MasqueradeEntities();

        private List<Character_Details> physical = new List<Character_Details>
        {
            new Character_Details("Physical", "Strenght", 5, 0),
            new Character_Details("Physical", "Dexterity", 5, 0),
            new Character_Details("Physical", "Stamina", 5, 0),
        };
        private List<Character_Details> social = new List<Character_Details>
        {
            new Character_Details("Social", "Charisma", 5, 0),
            new Character_Details("Social", "Manipulation", 5, 0),
            new Character_Details("Social", "Apparance", 5, 0),
        };
        private List<Character_Details> mental = new List<Character_Details>
        {
            new Character_Details("Mental", "Perception", 5, 0),
            new Character_Details("Mental", "Intelligence", 5, 0),
            new Character_Details("Mental", "Wits", 5, 0),
        };

        private List<Character_Details> talents = new List<Character_Details>
        {
            new Character_Details("Talents", "Alertness", 5, 0),
            new Character_Details("Talents", "Athletics", 5, 0),
            new Character_Details("Talents", "Brawl", 5, 0),
            new Character_Details("Talents", "Dodge", 5, 0),
            new Character_Details("Talents", "Empathy", 5, 0),
            new Character_Details("Talents", "Expression", 5, 0),
            new Character_Details("Talents", "Intimidation", 5, 0),
            new Character_Details("Talents", "Leadership", 5, 0),
            new Character_Details("Talents", "Streetwise", 5, 0),
            new Character_Details("Talents", "Subterfuge", 5, 0),
        };
        private List<Character_Details> skills = new List<Character_Details>
        {
            new Character_Details("Skills", "Animal Ken", 5, 0),
            new Character_Details("Skills", "Crafts", 5, 0),
            new Character_Details("Skills", "Drive", 5, 0),
            new Character_Details("Skills", "Etiquette", 5, 0),
            new Character_Details("Skills", "Firearms", 5, 0),
            new Character_Details("Skills", "Melee", 5, 0),
            new Character_Details("Skills", "Performance", 5, 0),
            new Character_Details("Skills", "Security", 5, 0),
            new Character_Details("Skills", "Stealth", 5, 0),
            new Character_Details("Skills", "Survival", 5, 0),
        };
        private List<Character_Details> knowledges = new List<Character_Details>
        {
            new Character_Details("Knowledges", "Academics", 5, 0),
            new Character_Details("Knowledges", "Computer", 5, 0),
            new Character_Details("Knowledges", "Finance", 5, 0),
            new Character_Details("Knowledges", "Investigation", 5, 0),
            new Character_Details("Knowledges", "Law", 5, 0),
            new Character_Details("Knowledges", "Linguistics", 5, 0),
            new Character_Details("Knowledges", "Medicine", 5, 0),
            new Character_Details("Knowledges", "Occult", 5, 0),
            new Character_Details("Knowledges", "Politics", 5, 0),
            new Character_Details("Knowledges", "Science", 5, 0)
        };


        public Character Character { get; set; }
        public List<Character_Details> Physical { get; set; }
        public List<Character_Details> Social { get; set; }
        public List<Character_Details> Mental { get; set; }
        public List<Character_Details> Talents { get; set; }
        public List<Character_Details> Skills { get; set; }
        public List<Character_Details> Knowledges { get; set; }
        public List<Character_Details> Disciplines { get; set; }
        public List<Character_Details> Backgrounds { get; set; }
        public List<Character_Details> Merits { get; set; }
        public List<Character_Details> Flaws { get; set; }
        public List<Character_Details> Virues { get; set; }
        public Character_Details Path { get; set; }
        public Character_Details WillPower { get; set; }
        public Character_Details WillPowerTemp { get; set; }
        public Character_Details BloodPool { get; set; }
        public Character_Details Health { get; set; }

        public CharactersModel()
        {
            this.Character = new Character();
            this.Physical = physical;
            this.Social = social;
            this.Mental = mental;
            this.Talents = talents;
            this.Skills = skills;
            this.Disciplines = null;
            this.Knowledges = knowledges;
            this.WillPower = new Character_Details("Extra", "Will Power", 10, 0);
        }

        public CharactersModel(int id)
        {
            Character character = db.Characters.Find(id);
            this.Character = character;
            this.Physical = character.Character_Details.Where(m => m.Section == "Physical").ToList();
            this.Social = character.Character_Details.Where(m => m.Section == "Social").ToList();
            this.Mental = character.Character_Details.Where(m => m.Section == "Mental").ToList();
            this.Talents = character.Character_Details.Where(m => m.Section == "Talents").ToList();
            this.Skills = character.Character_Details.Where(m => m.Section == "Skills").ToList();
            this.Knowledges = character.Character_Details.Where(m => m.Section == "Knowledges").ToList();
            this.Disciplines = character.Character_Details.Where(m => m.Section == "Disciplines").ToList();
            this.Backgrounds = character.Character_Details.Where(m => m.Section == "Backgrounds").ToList();
            this.Merits = character.Character_Details.Where(m => m.Section == "Merits").ToList();
            this.Flaws = character.Character_Details.Where(m => m.Section == "Flaws").ToList();
            this.Path = character.Character_Details.SingleOrDefault(m => m.Name == "Path");
            this.WillPower = character.Character_Details.SingleOrDefault(m => m.Name == "WillPower");
            this.WillPowerTemp = character.Character_Details.SingleOrDefault(m => m.Name == "WillPowerTemp");
            this.BloodPool = character.Character_Details.SingleOrDefault(m => m.Name == "BloodPool");
            this.Health = character.Character_Details.SingleOrDefault(m => m.Name == "Health");
            this.Virues = character.Character_Details.Where(m => m.Section == "Virtues").ToList();
        }

        public void SetMaxBloodPool()
        {
            var Generation = db.Characters.Find(this.Character.System_id).Generetion;
            var BloodPool = db.Character_Details.Where(c => c.Character_id == this.Character.System_id).SingleOrDefault(c => c.Name == "BloodPool");
            bool SixPool = false;
            switch (Generation)
            {
                case "4a":
                    BloodPool.MaxValue = 50;
                    SixPool = true;
                    break;
                case "5a":
                    BloodPool.MaxValue = 40;
                    SixPool = true;
                    break;
                case "6a":
                    BloodPool.MaxValue = 30;
                    SixPool = true;
                    break;
                case "7a":
                    BloodPool.MaxValue = 20;
                    SixPool = true;
                    break;
                case "8a":
                    BloodPool.MaxValue = 15;
                    break;
                case "9a":
                    BloodPool.MaxValue = 14;
                    break;
                case "10a":
                    BloodPool.MaxValue = 13;
                    break;
                case "11a":
                    BloodPool.MaxValue = 12;
                    break;
                case "12a":
                    BloodPool.MaxValue = 11;
                    break;
                case "13a":
                    BloodPool.MaxValue = 10;
                    break;
                case "14a":
                    BloodPool.MaxValue = 10;
                    break;
                default:
                    BloodPool.MaxValue = 20;
                    break;
            }
            if (BloodPool.ActualValue > BloodPool.MaxValue)
                BloodPool.ActualValue = BloodPool.MaxValue;
            db.Entry(BloodPool).State = EntityState.Modified;

            var DetailsToMod = new List<Character_Details>();

            DetailsToMod = db.Character_Details.Where(c => c.Section == "Physical" || c.Section == "Social" || c.Section == "Mental" ||
                c.Section == "Talents" || c.Section == "Skills" || c.Section == "Knowledges" || c.Section == "Disciplines").ToList();

            foreach (var d in DetailsToMod)
            {
                if (SixPool)
                    d.MaxValue = 6;
                else
                    d.MaxValue = 5;
                db.Entry(d).State = EntityState.Modified;
            }
            db.SaveChanges();
        }
    }
}