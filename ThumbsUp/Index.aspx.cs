using System;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

using System.Data;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;

namespace ThumbsUp
{
    public partial class Index : System.Web.UI.Page
    {
        const int ACCOUNTDISABLE = 0x0002;

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Header.Title = Page.Header.Title + " - Index";

            lblName.Text = "Hello " + Context.User.Identity.Name + ".";
            lblAuthType.Text = "You were authenticated using " + Context.User.Identity.AuthenticationType + ".";

            BindDropDownList();

            string[] ratingRadioLabels = new string[] { "Label1", "Label2", "Label3", "Label4", "Label5",
                "Label6", "Label7", "Label8", "Label9" };
            int[] ratingRadioValues = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            if (ratingRadioLabels.Length == ratingRadioValues.Length)
            {
                for (int i = 0; i < ratingRadioValues.Length; i++)
                {
                    lbRating.Items.Add(new ListItem(ratingRadioLabels[i], ratingRadioValues[i].ToString()));
                }
            }
        }

        public void BindDropDownList()
        {
            string query = "(&(objectCategory=person))";
            string[] columns = new string[] { "cn", "sAMAccountName", "userAccountControl" };
            string ldapPath = "LDAP://dc=classroom,dc=org";

            DataSet ds = FindUsers(query, columns, ldapPath, true);
            DataTable users = new DataTable();
            foreach (string prop in columns)
            {
                users.Columns.Add(prop, typeof(string));
            }

            string[] exclude = new string[] { 
                "sh1pryor", "sh1lalley", "sh1smith", "sh1curtis", "sh1bailey", "sh1russell",
                "sh1morris", "sh1patterson", "sh1barron", "sh1vanfrank", "sh1bush", "sh1twilson",
                "sh1fqureshi", "sh1brunt", "sh1larsen", "sh1boren", "sh1tqureshi", "sh1garrone",
                "sh1swanson", "sh1jackson", "sh1vaughn", "sh1rnew", "testjl", "Members", "training",
                "sqlalerts", "exam", "sonictest", "impact", "commview", "ach", "ittest", "teachmoney",
                "hrtest", "helpdesk", "ibmdirector", "ittestaccount", "dosstest", "stapler",
                "Mastercard", "Test1", "ipads", "oci", "ULTEST", "TellerTest", "mshift"
            };

            foreach (DataTable dt in ds.Tables)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (!exclude.Contains(dr["sAMAccountName"]))
                    {
                        if (dr["sAMAccountName"].ToString() != Context.User.Identity.Name &&
                            (dr["userAccountControl"].ToString() == "512" ||
                            dr["userAccountControl"].ToString() == "1114624"))
                        {

                            users.ImportRow(dr);
                        }
                    }
                }
            }

            DataView dvSelectOUUsers = users.DefaultView;
            dvSelectOUUsers.Sort = "sAMAccountName ASC";

            selectOUUsers.DataSource = users;
            selectOUUsers.DataTextField = "cn";
            selectOUUsers.DataValueField = "sAMAccountName";
            selectOUUsers.DataBind();
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