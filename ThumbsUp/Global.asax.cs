using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

using System.Security.Principal;

namespace ThumbsUp
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            /*
            string cookieName = FormsAuthentication.FormsCookieName;
            HttpCookie authCookie = Context.Request.Cookies[cookieName];

            if (null == authCookie)
            {
                // there is no authentication cookie
                return;
            }
            FormsAuthenticationTicket authTicket = null;
            try
            {
                authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            }
            catch (Exception ex)
            {
                //write the exception to the event log
                return;
            }

            if (null == authTicket)
            {
                // cookie failed to decrypt
                return;
            }
            //when the ticket was created, the UserData property was assigned a
            //pipe-delimited string of group names.
            string[] groups = authTicket.UserData.Split(new char[] { '|' });
            //create an identity
            GenericIdentity id = new GenericIdentity(authTicket.Name, "LdapAuthentication");
            //this principal flows throughout the request
            GenericPrincipal principal = new GenericPrincipal(id, groups);
            Context.User = principal;
            */
        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}