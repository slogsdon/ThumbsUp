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

    public class DBItem
    {
        private int _ID;
        private string _User;
        private DateTime _DateTime;
        private string _Person;
        private string _Rating;
        private string _Description;
        private string _Votes;
        private List<DBItem> _IDList = new List<DBItem>();
        private List<DBItem> _RatingList = new List<DBItem>();

        public int ID
        {
            get
            {
                return _ID;
            }
            set
            {
                _ID = value;
            }
        }
        public string User
        {
            get
            {
                return _User;
            }
            set
            {
                _User = value;
            }
        }
        public DateTime DateTime
        {
            get
            {
                return _DateTime;
            }
            set
            {
                _DateTime = value;
            }
        }
        public string Person
        {
            get
            {
                return _Person;
            }
            set
            {
                _Person = value;
            }
        }
        public string Rating
        {
            get
            {
                return _Rating;
            }
            set
            {
                _Rating = value;
            }
        }
        public string Description
        {
            get
            {
                return _Description;
            }
            set
            {
                _Description = value;
            }
        }
        public string Votes
        {
            get
            {
                return _Votes;
            }
            set
            {
                _Votes = value;
            }
        }
        public List<DBItem> IDList
        {
            get
            {
                return _IDList;
            }
            set
            {
                _IDList = value;
            }
        }
        public List<DBItem> RatingList
        {
            get
            {
                return _RatingList;
            }
            set
            {
                _RatingList = value;
            }
        }
        public int VoteCount
        {
            get
            {
                return (_Votes.Split(',').Length - 1);
            }
        }
        public int RatingSum
        {
            get
            {
                int ret = 0;
                foreach (DBItem i in _RatingList)
                {
                    ret += Convert.ToInt32(i.Rating.ToString());
                }
                return ret;
            }
        }

        public string stripNames
        {
            get
            {
                string displayName = (new LdapAuthentication("classroom")).
                    GetUserDisplayName(_Person);
                List<string> excludes = new List<string>();
                string text = _Description;

                excludes.Add(_Person);
                foreach (string str in displayName.Split(' '))
                {
                    excludes.Add(str);
                    excludes.Add(str.ToLower());
                    excludes.Add(str.ToLowerInvariant());
                    excludes.Add(str.ToUpper());
                    excludes.Add(str.ToUpperInvariant());
                }

                foreach (string search in excludes)
                {
                    if (text.Contains(search))
                    {
                        text = text.Replace(search, "the employee");
                    }
                }

                return text;
            }
        }
    }
}