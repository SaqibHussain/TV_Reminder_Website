using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Net;
using System.IO;

namespace Updater
{
    static class UpdateShows
    {
        //Delcare a connection string by pulling the connection string from the App.Config file
        static string cs = ConfigurationManager.ConnectionStrings["TVDB"].ConnectionString;

        public static void Update()
        {
            //Get all the shows
            DataView dv = getShows();
            //For every show in the dataview
            foreach (DataRowView rowView in dv)
            {
                //Create a new data row to pull the values of the columns we need to
                DataRow row = rowView.Row;
                Console.WriteLine(row["ShowName"]);
                //Call the update method on that show
                updateShow(row["ShowName"].ToString(), row["ShowID"].ToString());
            }
        }

        //Method to get all the shows in the database
        public static DataView getShows()
        {
            using (SqlConnection conn = new SqlConnection(cs))
            {
                string cmd = "select ShowID, ShowName, Status, RemindersSent, NextEpAirTime FROM tblTVShows";
                SqlDataAdapter da = new SqlDataAdapter(cmd, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                DataView dv = new DataView(dt);
                return dv;
            }
        }

        //Method called on each show to update its information
        public static void updateShow(string showName, string ShowID)
        {
            //Search is initialised to true
            bool search = true;
            //Search attempts initiliased to 0
            int searchattempts = 0;
            //A string builder is used to build the input from the website query
            StringBuilder sb = new StringBuilder();
            //Used on every read operation
            byte[] buf = new byte[8192];
            //Create the web page we will be requesting
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://services.tvrage.com/tools/quickinfo.php?show=" + showName);
            //While we are still trying to search
            while (search)
            {
                try
                {
                    //Set request parameters
                    request.Proxy = null;
                    request.Timeout = 10000;
                    //Create and get the http response
                    HttpWebResponse response = (HttpWebResponse)
                    request.GetResponse();
                    //Read the data via the response stream
                    Stream resStream = response.GetResponseStream();
                    //Initalise a temp string to be null
                    string tempString = null;
                    //Initialise count to be 0
                    int count = 0;
                    do
                    {
                        //Fill the buffer with data from the stream
                        count = resStream.Read(buf, 0, buf.Length);
                        //Make sure there is some data to read
                        if (count != 0)
                        {
                            //Translate from bytes to ASCII text
                            tempString = Encoding.ASCII.GetString(buf, 0, count);
                            //Build the string
                            sb.Append(tempString);
                        }
                    }
                    //While there is still more data to read
                    while (count > 0); 

                    //Print out the page source to the console
                    Console.Write(sb);

                    //Create a string array for every new line in the string builder
                    string[] sba = sb.ToString().Split('\n');

                    //Check if the show with the given name resturned any results
                    //String to compare with
                    string NothingFound = "No Show Results Were Found";
                    //Perform a check to see if results contain the check string
                    int FoundCheck = sba[0].IndexOf(NothingFound);
                    //If the string was found within the results then output a message to the console
                    if (FoundCheck == 0)
                    {
                        Console.WriteLine("This show was not found");
                    }
                    //Otherwise the string wasn't found so there must be some results
                    else
                    {
                        //For every line in the results
                        foreach (string s in sba)
                        {
                            //Print out the line
                            Console.WriteLine(s);
                            //Split the line on space to retrive the header
                            string[] header = s.Replace('@', ' ').Split(new Char[] { ' ' });
                            //Split the line on @ to retrieve the information
                            string[] info = s.Split(new Char[] { '@' });
                            //Replace @ with ^ and split the line on ^ to retrieve episode information
                            string[] episodeinfo = s.Replace('@', '^').Split(new Char[] { '^' });

                            //Get and update the Show Status if the line header is Status
                            if (header[0] == "Status")
                            {
                                //Call the update method passing the show status
                                updateShowStatus(ShowID, info[1]);
                            }
                            //Get the and update Latest Episode if the line header is Latest Episode
                            if (header[0] == "Latest" && header[1] == "Episode")
                            {
                                //Call the update methods for the latest episode attributes
                                updateShowLastEpName(ShowID, episodeinfo[2]);
                                updateShowLastEpNumber(ShowID, episodeinfo[1]);
                                DateTime LastEpDate;
                                //Try to parse the date before calling the update method
                                if (DateTime.TryParse(episodeinfo[3], out LastEpDate))
                                {
                                    updateShowLastEpDate(ShowID, LastEpDate);
                                }
                            }
                            //Get and update the Next Episode information if the line header is Next Episode
                            if (header[0] == "Next" && header[1] == "Episode")
                            {
                                //Call the update methods for the next episode attributes
                                updateShowNextEpName(ShowID, episodeinfo[2]);
                                updateShowNextEpNumber(ShowID, episodeinfo[1]);
                                DateTime LastEpDate;
                                //Try to parse the date before calling the update method
                                if (DateTime.TryParse(episodeinfo[3], out LastEpDate))
                                {
                                    updateShowNextEpDate(ShowID, LastEpDate);
                                }
                            }
                            //Get the next air date and time if the line header is RFC3339
                            if (header[0] == "RFC3339")
                            {
                                //Convert the value into a DateTime data type and call the update method only if the date already stored in the database is not the same
                                if (!sameDate(ShowID, Convert.ToDateTime(episodeinfo[1]))) {
                                    updateShowNextEpAirTime(ShowID, Convert.ToDateTime(episodeinfo[1]));
                                }
                            }
                            //If all finishes successfully, set search to false to stop any further attempts to search for data
                            search = false;
                        }
                    }
                }
                //Catch a web execption, most likely a timeout
                catch (WebException e)
                {
                    //Log expection
                    Console.WriteLine(e.Message);
                    //Recreate the request
                    request = (HttpWebRequest) WebRequest.Create("http://services.tvrage.com/tools/quickinfo.php?show=" + showName);
                    //Increament search attempts
                    searchattempts++;
                    //If this is going to be the second time searching then set search to false to only allow one more search attempt and log to the console
                    if (searchattempts > 2)
                    {
                        search = false;
                        Console.WriteLine("3 request attempts have timed out.");
                    }
                }
            }
        }

        public static void updateShowStatus(string showID, string status)
        {
            using (SqlConnection conn = new SqlConnection(cs))
            {
                string cmd = "UPDATE tblTVShows SET Status = '" + status + "' WHERE ShowID = " + showID + ";";
                SqlCommand command = new SqlCommand(cmd, conn);
                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
            }
        }
        public static void updateShowLastEpName(string showID, string LastEpName)
        {
            using (SqlConnection conn = new SqlConnection(cs))
            {
                string cmd = "UPDATE tblTVShows SET LatestEpTitle = '" + LastEpName.Replace("'", "") + "' WHERE ShowID = " + showID + ";";
                SqlCommand command = new SqlCommand(cmd, conn);
                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
            }
        }
        public static void updateShowLastEpNumber(string showID, string LastEpNumber)
        {
            using (SqlConnection conn = new SqlConnection(cs))
            {
                string cmd = "UPDATE tblTVShows SET LatestEpNumber = '" + LastEpNumber + "' WHERE ShowID = " + showID + ";";
                SqlCommand command = new SqlCommand(cmd, conn);
                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
            }
        }
        public static void updateShowLastEpDate(string showID, DateTime LastEpDate)
        {
            using (SqlConnection conn = new SqlConnection(cs))
            {
                string cmd = "UPDATE tblTVShows SET LatestEpAirDate = '" + LastEpDate.Date.ToString("d") + "' WHERE ShowID = " + showID + ";";
                SqlCommand command = new SqlCommand(cmd, conn);
                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
            }
        }
        public static void updateShowNextEpName(string showID, string NextEpName)
        {
            using (SqlConnection conn = new SqlConnection(cs))
            {
                string cmd = "UPDATE tblTVShows SET NextEpTitle = '" + NextEpName.Replace("'", "") + "' WHERE ShowID = " + showID + ";";
                SqlCommand command = new SqlCommand(cmd, conn);
                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
            }
        }
        public static void updateShowNextEpNumber(string showID, string NextEpNumber)
        {
            using (SqlConnection conn = new SqlConnection(cs))
            {
                string cmd = "UPDATE tblTVShows SET NextEpNumber = '" + NextEpNumber + "' WHERE ShowID = " + showID + ";";
                SqlCommand command = new SqlCommand(cmd, conn);
                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
            }
        }
        public static void updateShowNextEpDate(string showID, DateTime NextEpDate)
        {
            using (SqlConnection conn = new SqlConnection(cs))
            {
                string cmd = "UPDATE tblTVShows SET NextEpAirDate = '" + NextEpDate.Date.ToString("d") + "' WHERE ShowID = " + showID + ";";
                SqlCommand command = new SqlCommand(cmd, conn);
                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
            }
        }
        public static void updateShowNextEpAirTime(string showID, DateTime NextEpAirDate)
        {
            using (SqlConnection conn = new SqlConnection(cs))
            {
                string cmd = "UPDATE tblTVShows SET NextEpAirTime = '" + NextEpAirDate + "' WHERE ShowID = " + showID + ";";
                SqlCommand command = new SqlCommand(cmd, conn);
                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
                //Update the reminders to not sent yet because we have just added a new air time
                cmd = "UPDATE tblTVShows SET RemindersSent = 'NO' WHERE ShowID = " + showID + ";";
                command = new SqlCommand(cmd, conn);
                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
            }
        }
        public static bool sameDate(string showID, DateTime NewDate)
        {
            using (SqlConnection conn = new SqlConnection(cs))
            {
                string cmd = "select NextEpAirTime from tblTVShows where ShowID='" + showID + "'";
                SqlCommand command = new SqlCommand(cmd, conn);
                conn.Open();
                string s = command.ExecuteScalar().ToString();
                conn.Close();
                DateTime oldDate;
                if (DateTime.TryParse(s, out oldDate))
                {
                    // = Convert.ToDateTime();
                    if (oldDate.CompareTo(NewDate) == 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

    }
}
