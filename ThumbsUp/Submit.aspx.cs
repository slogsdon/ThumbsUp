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
    public partial class Submit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Header.Title = Page.Header.Title + " - Submit";

            lblName.Text = "Hello " + Context.User.Identity.Name + ".";
            lblAuthType.Text = "You were authenticated using " + Context.User.Identity.AuthenticationType + ".";

            BindDropDownList();
            BindRadioList();
        }

        public void BindDropDownList()
        {
            string query = "(&(objectCategory=person))";
            string[] columns = new string[] { "displayName", "sAMAccountName", "userAccountControl" };
            string ldapPath = "LDAP://dc=classroom,dc=org";

            DataSet ds = (new LdapAuthentication(ldapPath)).FindUsers(query, columns, ldapPath, true);
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
            dvSelectOUUsers.Sort = "displayName ASC";

            selectOUUsers.DataSource = users;
            selectOUUsers.DataTextField = "displayName";
            selectOUUsers.DataValueField = "sAMAccountName";
            selectOUUsers.DataBind();
        }
        public void BindRadioList()
        {
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
    }
}