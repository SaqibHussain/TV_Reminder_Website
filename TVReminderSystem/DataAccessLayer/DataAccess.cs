using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

using System.Data;
using System.Configuration;
using System.Data.SqlClient;

using System.Net;
using System.Xml;
using System.IO;

namespace DataAccessLayer
{
    public class DataAccess
    {
        #region Declarations for connection string
        //Setting up the connection string from the Web config file
        private string cs = ConfigurationManager.ConnectionStrings["TVDB"].ConnectionString;
        #endregion

        #region Methods for login and sign up
        //Method to check the submitted login information
        public bool login(string email, string password)
        {
            using (SqlConnection conn = new SqlConnection(cs))
            {
                string cmd = "select Count(*) from tblUser where Email='" + email + "' AND Password='" + password + "'";
                SqlCommand command = new SqlCommand(cmd, conn);
                conn.Open();
                int i = Convert.ToInt32(command.ExecuteScalar());
                conn.Close();
                if (i > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        //Method to get the UserID to store into the auth ticken when logged in
        public string getID(string email)
        {
            using (SqlConnection conn = new SqlConnection(cs))
            {
                string cmd = "select UserID from tblUser where Email='" + email + "'";
                SqlCommand command = new SqlCommand(cmd, conn);
                conn.Open();
                string s = command.ExecuteScalar().ToString();
                conn.Close();
                return s;
            }
        }
        //Method to check if email already exists in database
        public bool emailExists(string email)
        {
            using (SqlConnection conn = new SqlConnection(cs))
            {
                string cmd = "select Count(*) from tblUser where Email='" + email + "'";
                SqlCommand command = new SqlCommand(cmd, conn);
                conn.Open();
                int i = Convert.ToInt32(command.ExecuteScalar());
                conn.Close();
                if (i > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        //Method to check if mobile number already exists in database
        public bool mobileExists(string mobile)
        {
            using (SqlConnection conn = new SqlConnection(cs))
            {
                string cmd = "select Count(*) from tblUser where Mobile='" + mobile + "'";
                SqlCommand command = new SqlCommand(cmd, conn);
                conn.Open();
                int i = Convert.ToInt32(command.ExecuteScalar());
                conn.Close();
                if (i > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        //Sign up with mobile number
        public bool signUp(string fname, string sname, string email, string mobile, string password)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(cs))
                {
                    string cmd = "INSERT INTO tblUser VALUES ('" + fname + "', '" + sname + "', '" + email + "', '" + mobile + "', '" + password + "');";
                    SqlCommand command = new SqlCommand(cmd, conn);
                    conn.Open();
                    command.ExecuteNonQuery();
                    conn.Close();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        //Sign up without mobile number
        //public bool signUp(string fname, string sname, string email, string password)
        //{
        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(cs))
        //        {
        //            string cmd = "INSERT INTO tblUser ([FirstName], [Surname], [Email], [Password]) VALUES ('" + fname + "', '" + sname + "', '" + email + "', '" + password + "');";
        //            SqlCommand command = new SqlCommand(cmd, conn);
        //            conn.Open();
        //            command.ExecuteNonQuery();
        //            conn.Close();
        //        }
        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}
        #endregion

        #region Methods for pulling user information
        public string getUserName(string ID)
        {
            DataView dv = getUserInfo(ID);
            string name = dv[0][0].ToString() + " " + dv[0][1].ToString();
            return name;
        }
        public string getEmail(string ID)
        {
            DataView dv = getUserInfo(ID);
            string email = dv[0][2].ToString();
            return email;
        }
        public string getMobile(string ID)
        {
            DataView dv = getUserInfo(ID);
            string mobile = dv[0][3].ToString();
            return mobile;
        }
        public DataView getUserInfo(string ID)
        {
            using (SqlConnection conn = new SqlConnection(cs))
            {
                string cmd = "select FirstName, Surname, Email, Mobile from tblUser where UserID = '" + ID + "'";
                SqlDataAdapter da = new SqlDataAdapter(cmd, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                DataView dv = new DataView(dt);
                return dv;
            }
        }
        #endregion

        #region Methods for search section
        //Searching for shows from API using search text
        public DataTable searchShows(string s)
        {
            DataTable dt = new DataTable();

            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create("http://services.tvrage.com/feeds/search.php?show=buffy");
            myRequest.Method = "GET";  // or GET - depends 

            myRequest.ContentType = "text/xml; encoding=utf-8";
            //myRequest.ContentLength = data.Length;

            //using (Stream reqStream = myRequest.GetRequestStream())
            //{
            //    // Send the data.
            //    reqStream.Write(data, 0, data.Length);
            //    reqStream.Close();
            //}

            // Get Response
            WebResponse myResponse;

            myResponse = myRequest.GetResponse();
            XmlDocument _xmlDoc = new XmlDocument();

            using (Stream responseStream = myResponse.GetResponseStream())
            {
                _xmlDoc.Load(responseStream);
            }

            //Declare new instance of dataset
            DataSet ds = new DataSet();
            // Set the reader settings.
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;
            settings.IgnoreProcessingInstructions = true;
            settings.IgnoreWhitespace = true;
            // Create a resolver with default credentials.
            XmlUrlResolver resolver = new XmlUrlResolver();
            resolver.Credentials = System.Net.CredentialCache.DefaultCredentials;
            // Set the reader settings object to use the resolver.
            settings.XmlResolver = resolver;
            // Create the XmlReader object.
            XmlReader reader = XmlReader.Create("http://services.tvrage.com/feeds/search.php?show=" + s, settings);
            //Read into dataset
            ds.ReadXml(reader);
            //Set the data table equal to the first table in the data set
            dt = ds.Tables[0];
            //Return the datat table
            return dt;
            
        }
        //Pulling data for a show using TVRageID
        public DataTable getShowInfo(string ID)
        {
            //Declare new instance of data table
            DataTable dt = new DataTable();
            //Declare new instance of dataset
            DataSet ds = new DataSet();
            // Set the reader settings.
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;
            settings.IgnoreProcessingInstructions = true;
            settings.IgnoreWhitespace = true;
            // Create a resolver with default credentials.
            XmlUrlResolver resolver = new XmlUrlResolver();
            resolver.Credentials = System.Net.CredentialCache.DefaultCredentials;
            // Set the reader settings object to use the resolver.
            settings.XmlResolver = resolver;
            // Create the XmlReader object.
            XmlReader reader = XmlReader.Create("http://services.tvrage.com/feeds/showinfo.php?sid=" + ID, settings);
            //Read into dataset
            ds.ReadXml(reader);
            //Set the data table equal to the first table in the data set
            dt = ds.Tables[0];
            //Return the datat table
            return dt;

        }
        //Get current shows from database
        public DataTable getShows()
        {
            //Use the word "using" for opening up database automatically without wrrying about having to close the connection
            using (SqlConnection conn = new SqlConnection(cs))
            {
                string cmd = "select ShowID, ShowName, Status from tblTVShows";
                SqlDataAdapter da = new SqlDataAdapter(cmd, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                DataView dv = new DataView(dt);
                return dt;
            }
        }
        public int getNoOfSubs(string ID)
        {
            //Use the word "using" for opening up database automatically without wrrying about having to close the connection
            using (SqlConnection conn = new SqlConnection(cs))
            {
                string cmd = "select count(*) from tblSubscription where ShowID =" + ID + ";";
                SqlCommand command = new SqlCommand(cmd, conn);
                conn.Open();
                int i = Convert.ToInt32(command.ExecuteScalar());
                conn.Close();
                return i;
            }
        }
        #endregion

        #region Methods for interacting with user profile.
        //Method to try and subscribe the user and check if they are already subscribed
        public string trySubscribe(string TVRageID, string userID)
        {
            using (SqlConnection conn = new SqlConnection(cs))
            {
                //Check if the show being subscribed to already exists in the database
                string cmd = "select Count(*) from tblTVShows where TVRageID='" + TVRageID + "'";
                SqlCommand command = new SqlCommand(cmd, conn);
                conn.Open();
                int i = Convert.ToInt32(command.ExecuteScalar());
                conn.Close();
                if (i > 0)
                {
                    //Already exists in the database so check if the user is already subscribed to the show
                    cmd = "select count(*) from tblSubscription S, tblTVShows A where S.ShowID = A.ShowID AND S.UserID =" + userID + " AND A.TVRageID =" + TVRageID + ";";
                    command = new SqlCommand(cmd, conn);
                    conn.Open();
                    i = Convert.ToInt32(command.ExecuteScalar());
                    conn.Close();
                    //The user is already subscribed to the show
                    if (i > 0)
                    {
                        return "Already subscribed!";
                    }
                    //The user is not already subscribed so subscribe them
                    else
                    {
                        //Successfully subscribed
                        if (subscribe(TVRageID, userID))
                        {
                            return "Subscribed!";
                        }
                        //There was a problem
                        else 
                        {
                            return "There was a problem!";
                        }
                    }
                    
                }
                //Doesn't exist in the database so add the new show to the database and subscribe the user to the show
                else
                {
                    //If the show was added successfully
                    if (addShow(TVRageID))
                    {
                        //Subscribe the user
                        if (subscribe(TVRageID, userID))
                        {
                            return "Subscribed!";
                        }
                        //There was a problem
                        else
                        {
                            return "There was a problem!";
                        }
                    }
                        //The show wasnt addedd successfully
                    else
                    {
                        return "Error adding show";
                    }
                }
            }
        }
        //Subscribe a user to a given TVRageID
        private bool subscribe(string TVRageID, string userID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(cs))
                {
                    string cmd = "INSERT INTO tblSubscription VALUES (" + userID + ", " + getShowID(TVRageID) + ");";
                    SqlCommand command = new SqlCommand(cmd, conn);
                    conn.Open();
                    command.ExecuteNonQuery();
                    conn.Close();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        //Return the database ShowID for a given TVRageID
        private string getShowID(string TVRageID)
        {
            using (SqlConnection conn = new SqlConnection(cs))
            {
                string cmd = "select ShowID from tblTVShows where TVRageID='" + TVRageID + "'";
                SqlCommand command = new SqlCommand(cmd, conn);
                conn.Open();
                string s = command.ExecuteScalar().ToString();
                return s;
            }
        }
        //Add the new show to the database
        private bool addShow(string TVRageID) {
            try
            {
                using (SqlConnection conn = new SqlConnection(cs))
                {
                    DataTable dt = getShowInfo(TVRageID);
                    //string s = dt.Rows[0]["showID"].ToString() ;
                    string cmd = "INSERT INTO tblTVShows ([TVRageID], [ShowName], [Status]) VALUES (" + Convert.ToInt32(dt.Rows[0]["showid"]) + ", '" +
                    dt.Rows[0]["showname"].ToString() + "', '" + dt.Rows[0]["status"].ToString() + "');";
                    SqlCommand command = new SqlCommand(cmd, conn);
                    conn.Open();
                    command.ExecuteNonQuery();
                    conn.Close();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        //Check if an email exists minus the current email of the user
        public bool emailExists(string oldEmail, string newEmail)
        {
            using (SqlConnection conn = new SqlConnection(cs))
            {
                string cmd = "select Count(*) from tblUser where Email='" + newEmail + "' AND Email != '" + oldEmail + "'";
                SqlCommand command = new SqlCommand(cmd, conn);
                conn.Open();
                int i = Convert.ToInt32(command.ExecuteScalar());
                conn.Close();
                if (i > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        //Check if an mobile number exists minus the current mobile number of the user
        public bool mobileExists(string oldMobile, string newMobile)
        {
            using (SqlConnection conn = new SqlConnection(cs))
            {
                string cmd = "select Count(*) from tblUser where Mobile='" + newMobile + "' AND Mobile != '" + oldMobile + "'";
                SqlCommand command = new SqlCommand(cmd, conn);
                conn.Open();
                int i = Convert.ToInt32(command.ExecuteScalar());
                conn.Close();
                if (i > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        //Method to update user
        public void updateUser(string ID, string fname, string sname, string email, string mobile)
        {
            using (SqlConnection conn = new SqlConnection(cs))
            {
                string cmd = "UPDATE tblUser SET FirstName = '" + fname +
                    "', Surname = '" + sname +
                    "', Email = '" + email +
                    "', Mobile = '" + mobile +
                    "' WHERE UserID = " + ID + ";";

                SqlCommand command = new SqlCommand(cmd, conn);
                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
            }
        }
        //Method to return all the users' subscriptions
        public DataView getSubscriptions(string ID)
        {
            using (SqlConnection conn = new SqlConnection(cs))
            {
                string cmd = "SELECT S.ShowID AS ShowID, S.ShowName AS ShowName, S.NextEpNumber AS NextEpNo, S.NextEpTitle AS NextEpTitle, S.NextEpAirDate AS NextEpDate FROM tblTVShows S, tblSubscription B WHERE B.UserID = " + ID + " AND S.ShowID = B.ShowID;";
                SqlDataAdapter da = new SqlDataAdapter(cmd, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                DataView dv = new DataView(dt);
                return dv;
            }
        }
        //Method to unsubscribe a user from a subscription
        public void unsubscribe(string userID, string showID)
        {
            using (SqlConnection conn = new SqlConnection(cs))
            {
                string cmd = "DELETE from tblSubscription WHERE UserID = " + userID + " AND ShowID = " + showID + ";";
                SqlCommand command = new SqlCommand(cmd, conn);
                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
            }
        }
        #endregion

    }
}
