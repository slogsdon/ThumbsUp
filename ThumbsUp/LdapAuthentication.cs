using System;
using System.Text;
using System.Collections;
using System.Web;
using System.Web.Security;

using System.Security.Principal;
using System.Data;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;

namespace ThumbsUp
{
    public class LdapAuthentication
    {
        private string _path;
        private string _filterAttribute;
        private string _domain;
        private string _username;

        public LdapAuthentication(string path)
        {
            _path = path;
        }

        public bool IsAuthenticated(string domain, string username, string pwd)
        {
            _domain = domain;
            _username = username;

            string domainAndUsername = domain + @"\" + username;
            DirectoryEntry entry = new DirectoryEntry(_path, domainAndUsername, pwd);

            try
            {
                //bind to the native AdsObject to force authentication
                object obj = entry.NativeObject;

                DirectorySearcher search = new DirectorySearcher(entry);

                search.Filter = "(SAMAccountName=" + username + ")";
                search.PropertiesToLoad.Add("cn");
                SearchResult result = search.FindOne();

                if (null == result)
                {
                    return false;
                }

                //update the new path to the user in the directory
                _path = result.Path;
                _filterAttribute = (string)result.Properties["cn"][0];
            }
            catch (Exception ex)
            {
                throw new Exception("Error authenticating user. " + ex.Message);
            }

            return true;
        }

        public string GetGroups()
        {
            DirectorySearcher search = new DirectorySearcher(_path);
            search.Filter = "(cn=" + _filterAttribute + ")";
            search.PropertiesToLoad.Add("memberOf");
            StringBuilder groupNames = new StringBuilder();

            try
            {
                SearchResult result = search.FindOne();
                int propertyCount = result.Properties["memberOf"].Count;
                string dn;
                int equalsIndex, commaIndex;

                for (int propertyCounter = 0; propertyCounter < propertyCount; propertyCounter++)
                {
                    dn = (string)result.Properties["memberOf"][propertyCounter];
                    equalsIndex = dn.IndexOf("=", 1);
                    commaIndex = dn.IndexOf(",", 1);
                    if (-1 == equalsIndex)
                    {
                        return null;
                    }
                    groupNames.Append(dn.Substring((equalsIndex + 1), (commaIndex - equalsIndex) - 1));
                    groupNames.Append("|");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error obtaining group names. " + ex.Message);
            }
            return groupNames.ToString();
        }

        public string FriendlyDomainToLdapDomain(string friendlyDomainName) 
        {
            if (friendlyDomainName == null || friendlyDomainName == "") friendlyDomainName = _domain;

            string ldapPath = null;
            try
            {
                DirectoryContext objContext = new DirectoryContext(DirectoryContextType.Domain, friendlyDomainName);
                Domain objDomain = Domain.GetDomain(objContext);
                ldapPath = objDomain.Name;
            }
            catch (DirectoryServicesCOMException e)
            {
                ldapPath = e.Message.ToString();
            }
            return ldapPath;
        }

        public ArrayList EnumerateOU(string OuDn)
        {
            ArrayList alObjects = new ArrayList();
            try 
            {
                DirectoryEntry directoryObject = new DirectoryEntry("LDAP://" + OuDn);
                foreach (DirectoryEntry child in directoryObject.Children) 
                {
                    string childPath = child.Path.ToString();
                    childPath = childPath.Remove(0, 7);

                    if ("OU=" == childPath.Substring(0, 3))
                    {
                        alObjects.AddRange(EnumerateOU(childPath));
                    }
                    else
                    {
                        alObjects.Add(childPath);//remove ldap prefix
                    }

                    child.Close();
                    child.Dispose();
                }
                directoryObject.Close();
                directoryObject.Dispose();
            }
            catch (DirectoryServicesCOMException e) 
            {
                throw new DirectoryServicesCOMException("An error occured. " + e.Message);
            }
            return alObjects;
        }

        public DataSet FindUsers(string sFilter, string[] columns, string path, bool useCached)
        {
            //try to retrieve from cache first
            HttpContext context = HttpContext.Current;
            DataSet userDS = (DataSet)context.Cache[sFilter];

            if ((userDS == null) || (!useCached))
            {
                //setup the searching entries
                DirectoryEntry deParent = new DirectoryEntry(path);
                //deParent.Username = Config.Settings.UserName;
                //deParent.Password = Config.Settings.Password;
                deParent.AuthenticationType = AuthenticationTypes.Secure;

                DirectorySearcher ds = new DirectorySearcher(
                    deParent,
                    sFilter,
                    columns,
                    SearchScope.Subtree
                    );

                ds.PageSize = 1000;

                using (deParent)
                {
                    //setup the dataset that will store the results
                    userDS = new DataSet("userDS");
                    DataTable dt = userDS.Tables.Add("users");
                    DataRow dr;

                    //add each parameter as a column
                    foreach (string prop in columns)
                    {
                        dt.Columns.Add(prop, typeof(string));
                    }

                    using (SearchResultCollection src = ds.FindAll())
                    {
                        foreach (SearchResult sr in src)
                        {
                            dr = dt.NewRow();
                            foreach (string prop in columns)
                            {
                                if (sr.Properties.Contains(prop))
                                {
                                    dr[prop] = sr.Properties[prop][0];
                                }
                            }
                            dt.Rows.Add(dr);
                        }
                    }
                }
                //cache it for later, with sliding 3 minute window
                context.Cache.Insert(sFilter, userDS, null, DateTime.MaxValue, TimeSpan.FromSeconds(180));
            }
            return userDS;
        }
    }
}