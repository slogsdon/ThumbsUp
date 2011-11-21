using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ThumbsUp
{
    public partial class Site : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            UserLbl.Text = (new LdapAuthentication(Context.User.Identity.Name.Split('\\')[0])).
                GetUserDisplayName(Context.User.Identity.Name.Split('\\')[1]);

            List<string> allowed = new List<string>{"lhuether", "slogsdon"};
            
            if (allowed.Contains(Context.User.Identity.Name.Split('\\')[1])) {
                ListHyperLink.Visible = true;
            }
        }
        public AccessDataSource SubmissionsAccessDS
        {
            get
            {
                AccessDataSource ads = new AccessDataSource();
                ads.DataFile = Request.ApplicationPath + "/App_Data/db.mdb";
                return ads;
            }
        }
        public Label Message
        {
            get
            {
                return message;
            }
        }
    }
}