using System;
using System.Collections;
using System.Web;
using System.Web.UI;

using System.Data;
using System.Data.OleDb;

namespace ThumbsUp
{
    public partial class Index : System.Web.UI.Page
    {
        protected string _user;
        protected string _db;
        protected ArrayList _dbItemList;
        protected string _thumbImg;

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Header.Title = Page.Header.Title + " - Index";
            _user = Context.User.Identity.Name.Split('\\')[1].ToString();
            _db = SubmissionsAccessDS.DataFile;
            _thumbImg = "img/thumb.png";
            
            SubmissionsPlcHldr_Populate();
        }

        protected void SubmissionsPlcHldr_Populate()
        {
            using (SubmissionsPlcHldr)
            {
                Controls.Add(new LiteralControl("<div id=\"submissions\">\n"));

                ArrayList list = GetSubmissions();
                if (null != list)
                {
                    foreach (DBItem item in list)
                    {
                        Controls.Add(new LiteralControl("\t<div class=\"submission\">\n"));
                        Controls.Add(new LiteralControl("\t\t<span class=\"DateTime\">\n"));
                        Controls.Add(new LiteralControl("\t\t\t" + item.DateTime + "\n"));
                        Controls.Add(new LiteralControl("\t\t</span>\n"));
                        Controls.Add(new LiteralControl("\t\t<p>\n"));
                        Controls.Add(new LiteralControl("\t\t\t" + item.Description + "\n"));
                        Controls.Add(new LiteralControl("\t\t</p>\n"));
                        Controls.Add(new LiteralControl("\t\t<div>\n"));
                        Controls.Add(new LiteralControl("\t\t\t<img src=\"" + Server.MapPath(_thumbImg) + "\" alt=\"thumb\" />\n"));
                        Controls.Add(new LiteralControl("\t\t\t<span class=\"vote_count\">\n"));
                        Controls.Add(new LiteralControl("\t\t\t\t" + (item.Votes.Split(',').Length - 1) + "\n"));
                        Controls.Add(new LiteralControl("\t\t\t</span>\n"));
                        Controls.Add(new LiteralControl("\t\t</div>\n"));
                        Controls.Add(new LiteralControl("\t</div>\n"));
                    }
                }

                Controls.Add(new LiteralControl("</div>\n"));
            }
        }

        protected ArrayList GetSubmissions()
        {
            string query = "SELECT [ID], [User], [DateTime], [Person], [Rating], [Description], [Votes] FROM [Submissions];";
            ExecuteQuery(query, GetSubmissionsQueryCallback);

            return _dbItemList;
        }

        protected ArrayList GetSubmissionsQueryCallback(OleDbDataReader reader)
        {
            ArrayList returnList = new ArrayList();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    DBItem item = new DBItem();

                    item.ID = reader.GetInt32(reader.GetOrdinal("ID"));
                    item.User = reader.GetString(reader.GetOrdinal("User"));
                    item.DateTime = reader.GetDateTime(reader.GetOrdinal("DateTime"));
                    item.Person = reader.GetString(reader.GetOrdinal("Person"));
                    item.Rating = reader.GetString(reader.GetOrdinal("Rating"));
                    item.Description = reader.GetString(reader.GetOrdinal("Description"));
                    item.Votes = reader.GetString(reader.GetOrdinal("Votes"));

                    returnList.Add(item);
                }
            }
            return returnList;
        }

        protected ArrayList GetVotes(int id)
        {
            string query = "SELECT [Votes] FROM Submissions WHERE [ID] = " + id + ";";
            ExecuteQuery(query, GetVotesQueryCallback);
            
            return _dbItemList;
        }

        protected ArrayList GetVotesQueryCallback(OleDbDataReader reader)
        {
            ArrayList returnList = new ArrayList { };

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    DBItem item = new DBItem();
                    item.Votes = reader.GetString(reader.GetOrdinal("Votes"));
                    returnList.Add(item);
                }
            }
            return returnList;
        }

        protected bool ExecuteQuery(string query, Func<OleDbDataReader, ArrayList> callback = null)
        {
            bool rowsAffected = false;
            
            OleDbConnection connection = new OleDbConnection("Provider=Microsoft.Jet.Oledb.4.0;" +
                "Data Source=" + Server.MapPath(_db));
            try
            {
                connection.Open();
                using (OleDbCommand command = new OleDbCommand(query, connection))
                {
                    try
                    {
                        if (query.Substring(0, 6) == "SELECT")
                        {
                            if (callback != null)
                            {
                                _dbItemList = callback.Invoke(command.ExecuteReader());
                            }
                            rowsAffected = true;
                        }
                        else
                        {
                            rowsAffected = Convert.ToBoolean(command.ExecuteNonQuery());
                        }

                    }
                    catch (OleDbException e)
                    {
                        // cannot excute query
                    }
                }
                connection.Close();
            }
            catch (OleDbException e)
            {
                // cannot connect
            }
            return rowsAffected;
        }
    }

    public struct DBItem
    {
        private int _ID;
        private string _User;
        private DateTime _DateTime;
        private string _Person;
        private string _Rating;
        private string _Description;
        private string _Votes;

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
    }
}