using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace The_Masquerade.Models
{
    public class AdministrationModel
    {
        private The_MasqueradeEntities db = new The_MasqueradeEntities();
        private List<AspNetUser> users = new List<AspNetUser>();
        private List<AspNetRole> roles = new List<AspNetRole>();

        public List<AspNetUser> Users;
        public List<AspNetRole> Roles;

        public AdministrationModel()
        {
            this.Roles = roles;
            this.Users = users;

            foreach (AspNetUser user in db.AspNetUsers)
                this.Users.Add(user);
            foreach (AspNetRole role in db.AspNetRoles)
                this.Roles.Add(role);
        }
    }

    public class NarratorModel
    {
        private The_MasqueradeEntities db = new The_MasqueradeEntities();
        private List<Story> stories = new List<Story>();
        private List<Player> players = new List<Player>();

        public List<Story> Stories;
        public List<Player> Players;

        public NarratorModel(string name)
        {
            this.Stories = stories;
            this.Players = players;

            var id = db.AspNetUsers.SingleOrDefault(u => u.Email == name).Id;
            var tempPlayers = db.Players.Where(p => p.Narrator_id == id).OrderBy(p => p.Status).ThenByDescending(p => p.Type);
            foreach (Story story in db.Stories.Where(s => s.Narrator_id == id))
                this.Stories.Add(story);
            foreach (Player player in tempPlayers)
                this.Players.Add(player);
        }
    }
}