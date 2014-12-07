using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Net.Mail;
using Twilio;

namespace Updater
{
    public static class SendReminders
    {
        //Delcare a connection string by pulling the connection string from the App.Config file
        static string cs = ConfigurationManager.ConnectionStrings["TVDB"].ConnectionString;



        public static void Reminders()
        {
            //Get all the shows using the update shows getShows method
            DataView dv = UpdateShows.getShows();
            //For every show in the dataview
            foreach (DataRowView rowView in dv)
            {
                //Create a new data row to pull the values of the columns we need to
                DataRow row = rowView.Row;
                Console.WriteLine(row["ShowName"]);

                string showID = row["ShowID"].ToString();
                string showName = row["ShowName"].ToString();

                //If the show is not cancelled
                if (!isCancelled(row["Status"].ToString()))
                {
                    Console.WriteLine("Not cancelled");
                    //If the reminders have not already been sent
                    if (!remindersSent(row["RemindersSent"].ToString()))
                    {
                        Console.WriteLine("Not Sent");
                        //If the time span is less than an hour
                        if (airTime(row["NextEpAirTime"].ToString()))
                        {
                            Console.WriteLine("Air Time Less than an hour");
                            //If the show has any subscribers
                            if (hasSubscribers(showID))
                            {
                                Console.WriteLine("Show Has Subscribers");
                                //Get the list of users who subscribe to that show and email/text
                                DataView users = getSubscribers(showID);
                                foreach (DataRowView userRowView in users)
                                {
                                    DataRow user = userRowView.Row;
                                    string userID = user["UserID"].ToString();
                                    //Send the email
                                    sendEmail(getEmail(userID), showName);
                                    Console.WriteLine("Email send to UserID" + userID);
                                    //If the user has a mobile number
                                    string mobile = getMobile(userID);
                                    if (mobile != string.Empty)
                                    {
                                        sendText(mobile, showName);
                                        Console.WriteLine("Text send to UserID" + userID);
                                    }

                                }
                            }
                            else
                            {
                                Console.WriteLine("No Subscribers");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Air Time not Less than an hour");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Sent");
                    }
                }
                else
                {
                    Console.WriteLine("Cancelled");
                }
            }

        }

        //Method to check if the given string contains cancelled
        public static bool isCancelled(string status)
        {
                 //If the Status is not null or empty check if the show is cancelled
                if (status != null && status != string.Empty)
                {
                    //String to compare with
                    string NothingFound = "Canceled/Ended";
                    //Perform a check to see if the string contains the check string
                    int FoundCheck = status.IndexOf(NothingFound);
                    //If the string was found within the results then the show is cancelled so we return true
                    if (FoundCheck == 0)
                    {
                        return true;
                    }
                    //Otherwise the show is not cancelled so return false;
                    else
                    {
                        return false;
                    }
                }
                    //Otherwsie no information on whether show is cancelled or not so return true to not send any reminders
                else
                {
                    return true;
                }
        }
        //Method to check if the given string is a YES or NO
        public static bool remindersSent(string remindersSent)
        {
            //If the Status is not null or empty check if the show is cancelled
            if (remindersSent != null && remindersSent != string.Empty)
            {
                //String to compare with
                string NothingFound = "Canceled/Ended";
                //Perform a check to see if the string contains the check string
                int FoundCheck = remindersSent.IndexOf(NothingFound);
                //If the string was found within the results then the show is cancelled so we return true
                if (FoundCheck == 0)
                {
                    return true;
                }
                //Otherwise the reminders have not been sent so return false
                else
                {
                    return false;
                }
            }
            //Otherwsie no information on whether the reminders have been sent so send them
            else
            {
                return true;
            }
            
        }
        //Method to check if the given time is under an hour when the current date and time is subtracted
        public static bool airTime(string airTime)
        {
            DateTime dt;
            //If parsed successfully
            if (DateTime.TryParse(airTime, out dt))
            {
                //If the hours until the air time are less than 24
                if (dt.Subtract(DateTime.Now).TotalHours < 24)
                {
                    return true;
                }
                //Hours are still more than 24
                else
                {
                    return false;
                }
            }
            //Parse Failed, return false 
            else
            {
                return false;
            }
        }
        //Method to check if a show has subscribers
        public static bool hasSubscribers(string showID)
        {
            using (SqlConnection conn = new SqlConnection(cs))
            {
                string cmd = "select Count(*) from tblSubscription where ShowID =" + showID + ";";
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
        //Method to return all of the subscribers for a show
        public static DataView getSubscribers(string showID)
        {
            using (SqlConnection conn = new SqlConnection(cs))
            {
                string cmd = "select UserID from tblSubscription where ShowID =" + showID + ";";
                SqlDataAdapter da = new SqlDataAdapter(cmd, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                DataView dv = new DataView(dt);
                return dv;
            }
        }
        //Method to get the email of a user
        public static string getEmail(string userID)
        {
            using (SqlConnection conn = new SqlConnection(cs))
            {
                string cmd = "select Email from tblUser where UserID='" + userID + "'";
                SqlCommand command = new SqlCommand(cmd, conn);
                conn.Open();
                string s = command.ExecuteScalar().ToString();
                conn.Close();
                return s;
            }
        }
        //Method to send an email to a user
        public static void sendEmail(string email, string showName)
        {
            SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.Timeout = 10000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("TVReminderSystem@gmail.com", "tvsystem");
            MailMessage mm = new MailMessage("donotreply@domain.com", email, "Reminder for" + showName, showName + " is set to air in less than 24 hours! Don't forget to watch");
            mm.BodyEncoding = UTF8Encoding.UTF8;
            mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            client.Send(mm);
            Console.WriteLine(email + " " + showName);
        }
        //Method to send a Text to a user
        public static string getMobile(string userID)
        {
            using (SqlConnection conn = new SqlConnection(cs))
            {
                string cmd = "select Mobile from tblUser where UserID =" + userID + ";";
                SqlCommand command = new SqlCommand(cmd, conn);
                conn.Open();
                string s = command.ExecuteScalar().ToString();
                conn.Close();
                return s;
            }
        }
        //Method to send a text
        public static void sendText(string number, string showName)
        {
            string ACCOUNT_SID = "****************************";
            string AUTH_TOKEN = "***********************";

            TwilioRestClient client = new TwilioRestClient(ACCOUNT_SID, AUTH_TOKEN);

            client.SendSmsMessage("**************1", number, "This is your reminder for " + showName + ", which is set to air in less than 24 hours!");
        }
    }
}
