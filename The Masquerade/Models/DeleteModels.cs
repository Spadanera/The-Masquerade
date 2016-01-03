using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace The_Masquerade.Models
{
    public static class DeleteModels
    {
        public static void Delete(AspNetUser user)
        {
            The_MasqueradeEntities db = new The_MasqueradeEntities();
            List<Story> tempListStory = new List<Story>();

            foreach (Story story in user.Stories)
                tempListStory.Add(story);
            foreach (Story story in tempListStory)
                Delete(story);

            var u = db.AspNetUsers.SingleOrDefault(us => us.Id == user.Id);
            db.AspNetUsers.Remove(u);
            db.SaveChanges();
        }

        public static void Delete(Story story)
        {
            The_MasqueradeEntities db = new The_MasqueradeEntities();
            List<Chronicle> tempListChronicle = new List<Chronicle>();
            List<Player> tempListPlayer = new List<Player>();

            foreach (Chronicle chronicle in story.Chronicles)
                tempListChronicle.Add(chronicle);
            foreach (Chronicle chronicle in tempListChronicle)
                Delete(chronicle);

            foreach (Player player in story.Players)
                tempListPlayer.Add(player);
            foreach (Player player in tempListPlayer)
                Delete(player);

            var s = db.Stories.SingleOrDefault(st => st.System_id == story.System_id);
            db.Stories.Remove(s);
            db.SaveChanges();
        }

        public static void Delete(Chronicle chronicle)
        {
            The_MasqueradeEntities db = new The_MasqueradeEntities();
            List<Session> tempList = new List<Session>();
            
            foreach (Session session in chronicle.Sessions)
                tempList.Add(session);
            foreach (Session session in tempList)
            {
                Delete(session);
            }

            var c = db.Chronicles.SingleOrDefault(cr => cr.System_id == chronicle.System_id);
            db.Chronicles.Remove(c);
            db.SaveChanges();
        }

        public static void Delete(Session session)
        {
            The_MasqueradeEntities db = new The_MasqueradeEntities();
            List<Session_Details> tempList = new List<Session_Details>();
            
            foreach (Session_Details session_details in session.Session_Details)
                tempList.Add(session_details);
            foreach (Session_Details session_details in tempList)
            {
                var sd = db.Session_Details.SingleOrDefault(s => s.System_id == session_details.System_id);
                db.Session_Details.Remove(sd);
                db.SaveChanges();

                var character = db.Characters.Find(session_details.Character_id);
                character.Experience_Points += -session_details.Experience_Points;
                db.Entry(character).State = EntityState.Modified;
                db.SaveChanges();
            }

            var session_d = db.Sessions.SingleOrDefault(se => se.System_id == session.System_id);
            db.Sessions.Remove(session_d);
            db.SaveChanges();
        }

        public static void Delete(Player player)
        {
            The_MasqueradeEntities db = new The_MasqueradeEntities();
            List<Character> tempList = new List<Character>();

            foreach (Character c in player.Characters)
                tempList.Add(c);
            foreach (Story s in player.Stories)
                db.Stories.SingleOrDefault(st => st.System_id == s.System_id).Players.Remove(player);

            foreach (Character character in tempList)
                Delete(character);

            db.Players.SingleOrDefault(pl => pl.System_Id == player.System_Id).Stories.Clear();
            db.SaveChanges();

            var p = db.Players.SingleOrDefault(pl => pl.System_Id == player.System_Id);
            db.Players.Remove(p);
            db.SaveChanges();
        }

        public static void Delete(Character character)
        {
            The_MasqueradeEntities db = new The_MasqueradeEntities();
            List<Character_Details> tempList = new List<Character_Details>();

            foreach (Character_Details cd in character.Character_Details)
                tempList.Add(cd);
            foreach (Character_Details cd in tempList)
            {
                var tmpCar = db.Character_Details.SingleOrDefault(car => car.System_id == cd.System_id);
                db.Character_Details.Remove(tmpCar);
                db.SaveChanges();
            }

            var c = db.Characters.SingleOrDefault(ch => ch.System_id == character.System_id);
            db.Characters.Remove(c);
            db.SaveChanges();
        }
    }
}