using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace ThumbsUp
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            txtDomain.Value = "classroom";
        }
        protected void Login_Click(object sender, EventArgs e)
        {
            string adPath = "LDAP://" + txtDomain.Value;

            LdapAuthentication adAuth = new LdapAuthentication(adPath);
            try
            {
                if (true == adAuth.IsAuthenticated(txtDomain.Value, txtUsername.Text, txtPassword.Text))
                {
                    string groups = adAuth.GetGroups();

                    //create the ticket, and add the groups
                    bool isCookiePersistent = chkPersist.Checked;
                    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, txtUsername.Text, DateTime.Now, DateTime.Now.AddMinutes(60), isCookiePersistent, groups);

                    //encrypt the ticket
                    string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

                    //create a cookie, and then add the encrypted ticket to the cookie as data.
                    HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

                    if (true == isCookiePersistent)
                    {
                        authCookie.Expires = authTicket.Expiration;
                    }

                    //add the cookie to the outgoing cookies collection
                    Response.Cookies.Add(authCookie);

                    //you can redirect now
                    Response.Redirect(FormsAuthentication.GetRedirectUrl(txtUsername.Text, false));
                }
                else
                {
                    errorLabel.Text = "Authentication did not succeed. Check your username and password.";
                }
            }
            catch (Exception ex)
            {
                errorLabel.Text = "Error authenticating. " + ex.Message;
            }
        }
    }
}