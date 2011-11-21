using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

using System.Data;
using System.Data.OleDb;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;

namespace ThumbsUp
{
    public partial class Submit : System.Web.UI.Page
    {
        protected string _user;
        protected string _db;

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Header.Title = Page.Header.Title + " - Submit";
            _user = (Context.User.Identity.Name.Split('\\'))[1].ToString();// ldap auth just needs Context.User.Identity.Name
            _db = Master.SubmissionsAccessDS.DataFile;

            BindDropDownList();
            BindRadioList();

            if (IsPostBack && IsValid)
            {
                FormSubmit();
            }
        }

        public void FormSubmit()
        {
            Form1.Visible = false;
            ListItemCollection results = new ListItemCollection();
            string person = null;

            foreach (string key in Request.Form.Keys)
            {
                if (key.Substring(0, 2) != "__")
                {
                    string[] keyArray = key.Split('$');
                    results.Add(new ListItem(keyArray[keyArray.Length - 1], Request.Form[key]));
                    if (keyArray[keyArray.Length - 1] == "Person") person = Request.Form[key];
                }
            }
            OleDbConnection connection = new OleDbConnection("Provider=Microsoft.Jet.Oledb.4.0;" +
                        "Data Source=" + Server.MapPath(_db));
            connection.Open();

            string query = "INSERT INTO Submissions([User], [Person], [Rating], [Description], [Votes]) VALUES {1};";
            string values = "'" + _user + "', ";
            foreach (ListItem item in results)
            {
                //if (item.Text == "Description")
                //{
                //    string val = stripNames(item.Value, person);
                //    Master.Message.Text += val;
                //    values += "'" + val + "', ";
                //}
                //else
                //{
                    values += "'" + item.Value + "', ";
                //}
            }
            values += "'," + _user + "'";
            query = query.Replace("{1};", "(" + values + ");");

            OleDbCommand command = new OleDbCommand(query, connection);

            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Response.Write("error: " + e.Message + ". \nsql: " + query + ". \nvalues: " + values);
            }

            connection.Close();

            Response.Redirect("Index.aspx");
        }

        public string stripNames(string text, string person)
        {
            string displayName = (new LdapAuthentication(Context.User.Identity.Name.Split('\\')[0])).
                GetUserDisplayName(person);
            List<string> excludes = new List<string>();

            excludes.Add(person);
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
                text = ReplaceEx(text, person, "the employee");
            }

            return text;
        }

        private static string ReplaceEx(string original,
                    string pattern, string replacement)
        {
            int count, position0, position1;
            count = position0 = position1 = 0;
            string upperString = original.ToUpper();
            string upperPattern = pattern.ToUpper();
            int inc = (original.Length / pattern.Length) *
                      (replacement.Length - pattern.Length);
            char[] chars = new char[original.Length + Math.Max(0, inc)];
            while ((position1 = upperString.IndexOf(upperPattern,
                                              position0)) != -1)
            {
                for (int i = position0; i < position1; ++i)
                    chars[count++] = original[i];
                for (int i = 0; i < replacement.Length; ++i)
                    chars[count++] = replacement[i];
                position0 = position1 + pattern.Length;
            }
            if (position0 == 0) return original;
            for (int i = position0; i < original.Length; ++i)
                chars[count++] = original[i];
            return new string(chars, 0, count);
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
                        if (dr["sAMAccountName"].ToString() != _user && 
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