using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace ThumbsUp
{
    public partial class Index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Write("Hello, " + Server.HtmlEncode(User.Identity.Name));

            FormsIdentity id = (FormsIdentity)User.Identity;
            FormsAuthenticationTicket ticket = id.Ticket;

            Response.Write("<p/>TicketName: " + ticket.Name);
            Response.Write("<br/>Cookie Path: " + ticket.CookiePath);
            Response.Write("<br/>Ticket Expiration: " +
                            ticket.Expiration.ToString());
            Response.Write("<br/>Expired: " + ticket.Expired.ToString());
            Response.Write("<br/>Persistent: " + ticket.IsPersistent.ToString());
            Response.Write("<br/>IssueDate: " + ticket.IssueDate.ToString());
            Response.Write("<br/>UserData: " + ticket.UserData);
            Response.Write("<br/>Version: " + ticket.Version.ToString());
        }
    }
}