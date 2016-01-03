using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace The_Masquerade.Models.Views
{
    public class PlayersModel
    {
        private The_MasqueradeEntities db = new The_MasqueradeEntities();

        public Player Player { get; set; }
        public List<Story> AllStories { get; set; }
        public List<Story> UserStories { get; set; }
        public int[] SelectedStories { get; set; } 

        public PlayersModel(string userId)
        {
			this.Player = new Player();
			this.UserStories = new List<Story>();
            this.AllStories = db.Stories.Where(s => s.Narrator_id == userId).ToList();
        }

        public PlayersModel(string userId, int playerId)
        {
            this.Player = db.Players.SingleOrDefault(p => p.System_Id == playerId);
            this.AllStories = db.Stories.Where(s => s.Narrator_id == userId).ToList();
            this.UserStories = this.Player.Stories.ToList();
			this.SelectedStories = this.UserStories.Select(x => x.System_id).ToArray();
        }

        public PlayersModel()
        { }
    }
}