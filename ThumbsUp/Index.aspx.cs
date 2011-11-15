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
        protected DBItem[] _dbItemList;
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
                Controls.Add(new LiteralControl("<div id=\"submissions\">"));

                DBItem[] list = GetSubmissions();

                foreach (DBItem item in list)
                {
                    Controls.Add(new LiteralControl("\t<div class=\"submission\">"));
                    Controls.Add(new LiteralControl("\t\t<span class=\"date\">"));
                    Controls.Add(new LiteralControl("\t\t\t" + item.Date));
                    Controls.Add(new LiteralControl("\t\t</span>"));
                    Controls.Add(new LiteralControl("\t\t<p>"));
                    Controls.Add(new LiteralControl("\t\t\t" + item.Description));
                    Controls.Add(new LiteralControl("\t\t</p>"));
                    Controls.Add(new LiteralControl("\t\t<div>"));
                    Controls.Add(new LiteralControl("\t\t\t<img src=\"" + Server.MapPath(_thumbImg) + "\" alt=\"thumb\" />"));
                    Controls.Add(new LiteralControl("\t\t\t<span class=\"vote_count\">"));
                    Controls.Add(new LiteralControl("\t\t\t\t" + (item.Votes.Split(',').Length - 1)));
                    Controls.Add(new LiteralControl("\t\t\t</span>"));
                    Controls.Add(new LiteralControl("\t\t</div>"));
                    Controls.Add(new LiteralControl("\t</div>"));
                }

                Controls.Add(new LiteralControl("</div>"));
            }
        }

        protected DBItem[] GetSubmissions()
        {
            string query = "SELECT [ID], [User], [Date], [Person], [Rating], [Description], [Votes] FROM Submissions;";
            ExecuteQuery(query, (reader) => GetSubmissionsQueryCallback(reader));

            return _dbItemList;
        }

        protected DBItem[] GetSubmissionsQueryCallback(OleDbDataReader reader)
        {
            DBItem[] returnList = new DBItem[] { };

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    DBItem item = new DBItem();

                    item.ID = reader.GetInt16(reader.GetOrdinal("ID"));
                    item.User = reader.GetString(reader.GetOrdinal("User"));
                    item.Date = reader.GetString(reader.GetOrdinal("Date"));
                    item.Person = reader.GetString(reader.GetOrdinal("Person"));
                    item.Rating = reader.GetString(reader.GetOrdinal("Rating"));
                    item.Description = reader.GetString(reader.GetOrdinal("Description"));
                    item.Votes = reader.GetString(reader.GetOrdinal("Votes"));

                    returnList[returnList.Length] = item;
                }
            }
            return returnList;
        }

        protected DBItem[] GetVotes(int id)
        {
            string query = "SELECT [Votes] FROM Submissions WHERE [ID] = " + id + ";";
            ExecuteQuery(query, GetVotesQueryCallback);
            
            return _dbItemList;
        }

        protected DBItem[] GetVotesQueryCallback(OleDbDataReader reader)
        {
            DBItem[] returnList = new DBItem[] { };

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    DBItem item = new DBItem();
                    item.Votes = reader.GetString(reader.GetOrdinal("Votes"));
                    returnList[returnList.Length] = item;
                }
            }
            return returnList;
        }

        protected bool ExecuteQuery(string query, Func<OleDbDataReader, DBItem[]> callback = null)
        {
            string db = _db;
            
            bool rowsAffected = false;

            OleDbConnection connection = new OleDbConnection("Provider=Microsoft.Jet.Oledb.4.0" +
                "Data Source=" + Server.MapPath("db.mdb"));
            try
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand(query, connection);
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
                catch (Exception e)
                {
                    // cannot excute query
                }
                connection.Close();
            }
            catch (Exception e)
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
        private string _Date;
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
        public string Date
        {
            get
            {
                return _Date;
            }
            set
            {
                _Date = value;
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