using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data.OleDb;

namespace ThumbsUp
{
    public partial class List : System.Web.UI.Page
    {
        protected string _user;
        protected string _db;
        protected List<DBItem> _dbItemList;
        protected List<DBItem> _compactedList;
        protected string _thumbImg;

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Header.Title = Page.Header.Title + " - List";
            _user = Context.User.Identity.Name.Split('\\')[1].ToString();
            _db = Master.SubmissionsAccessDS.DataFile;

            if (Request.QueryString.HasKeys() && null != Request.QueryString.GetValues("VoteId"))
            {
                VoteUp(Request.QueryString.GetValues("VoteID")[0]);
            }

            SubmissionsRepeater.DataSource = GetSubmissions();
            SubmissionsRepeater.DataBind();
        }

        protected List<DBItem> GetSubmissions()
        {
            DateTime now = DateTime.Now;
            string query = "SELECT [ID], [User], [DateTime], [Person], [Rating], [Description], [Votes] FROM [Submissions] WHERE [DateTime] > #" +
                new DateTime(now.Year, now.Month, 1).ToString() + "#;";
            ExecuteQuery(query, GetSubmissionsQueryCallback);

            List<DBItem> sortedList = Compact(_dbItemList);

            sortedList.Sort(delegate(DBItem item1, DBItem item2) 
            {
                return item2.RatingSum.CompareTo(item1.RatingSum);
            });

            return sortedList;
        }

        protected List<DBItem> GetSubmissionsQueryCallback(OleDbDataReader reader)
        {
            List<DBItem> returnList = new List<DBItem>();

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

        protected string GetVotes(string id)
        {
            string query = "SELECT [Votes] FROM [Submissions] WHERE ([ID] = " + id + ");";
            ExecuteQuery(query, GetVotesQueryCallback);

            string result = "";

            foreach (DBItem item in _dbItemList)
            {
                result += item.Votes;
            }

            return result;
        }

        protected List<DBItem> GetVotesQueryCallback(OleDbDataReader reader)
        {
            List<DBItem> returnList = new List<DBItem>();

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

        protected void VoteUp(string id)
        {
            string list = GetVotes(id);
            string query = "UPDATE Submissions SET [Votes] = '" + list + "," + _user + "' WHERE ([ID] = " + id + ");";
            foreach (string voter in list.Split(','))
            {
                if (voter == _user)
                {
                    return;
                }
            }
            ExecuteQuery(query);
        }

        protected bool ExecuteQuery(string query, Func<OleDbDataReader, List<DBItem>> callback = null)
        {
            bool rowsAffected = false;

            OleDbConnection connection = new OleDbConnection("Provider=Microsoft.Jet.Oledb.4.0;" +
                "Data Source=" + Server.MapPath(_db));
            //try
            //{
            connection.Open();
            using (OleDbCommand command = new OleDbCommand(query, connection))
            {
                //try
                //{
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

                //}
                //catch (OleDbException e)
                //{
                //    Message.Text += "_dbItemList is null. " + e.Message + "\n";
                //}
            }
            connection.Close();
            //}
            //catch (OleDbException e)
            //{
            //    Message.Text += "_dbItemList is null. " + e.Message + "\n";
            //}
            return rowsAffected;
        }

        protected List<DBItem> Compact(List<DBItem> orig)
        {
            List<DBItem> returnList = new List<DBItem>();
            List<string> list = new List<string>();

            foreach (DBItem item in _dbItemList)
            {
                foreach (DBItem retItem in returnList)
                {
                    if (item.Person == retItem.Person)
                    {
                        DBItem j = new DBItem();

                        j.Rating = item.Rating;
                        j.ID = item.ID;
                        retItem.RatingList.Add(j);
                        retItem.IDList.Add(j);
                    }
                }
                if (!list.Contains(item.Person))
                {
                    DBItem i = new DBItem();
                    DBItem j = new DBItem();

                    i.Person = item.Person;
                    j.Rating = item.Rating;
                    j.ID = item.ID;
                    i.RatingList.Add(j);
                    i.IDList.Add(j);

                    list.Add(item.Person);
                    returnList.Add(i);
                }
            }

            return returnList;
        }
    }
}