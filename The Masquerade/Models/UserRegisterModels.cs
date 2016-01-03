using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace The_Masquerade.Models
{
    public class UserRegisterModels
    {
        private The_MasqueradeEntities db = new The_MasqueradeEntities();
        public Player Player { get; set; }
        public UserRegisterModels() { }
        public UserRegisterModels(Player player) { this.Player = player; }

        public void SendInvitation()
        {
            var Narrator = db.AspNetUsers.SingleOrDefault(u => u.Id == Player.Narrator_id).UserName;
            var from = "The Masqurade";
            var to = Player.eMail;
            var subject = "Join The Masquerade";
            var body = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath(@"~/Content/Template/Invitation.html"));
            body = body.Replace("#link", string.Format("http://the-masquerade.net/Account/RegisterPlayer/{0}", Player.System_Id));
            body = body.Replace("#Full_Name", Player.Full_Name);
            body = body.Replace("#Narrator", Narrator);

            Utilities.SendEmail(from, to, subject, body);
        }

        public void InvitationAccepted()
        {
            var from = "The Masquerade";
            var Narrator = db.AspNetUsers.SingleOrDefault(u => u.Id == Player.Narrator_id);
            var to = Narrator.Email;
            var subject = Player.UserName + " have accepted your invitation";
            var body = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath(@"~/Content/Template/Invitation.html"));
            body = body.Replace("#Full_Name", Player.Full_Name);
            body = body.Replace("#Narrator", Narrator.UserName);

            Utilities.SendEmail(from, to, subject, body);
        }
    }
}