using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading;
using System.Timers;

namespace Updater
{
    class Program
    {

        //Delcare two start timers within the scope of the class to be used by both methods inside the class
        static Timer updateTimer = new Timer();
        static Timer remindTimer = new Timer();
        static void Main(string[] args)
        {
            //Set the event handler for the update timer and reminder timer
            updateTimer.Elapsed += new System.Timers.ElapsedEventHandler(update);
            remindTimer.Elapsed += new System.Timers.ElapsedEventHandler(remind);
            //Set the intervals at which to run the updates and reminder methods
            //Updates checked for every half an hour/30 minutes
            updateTimer.Interval = 1800000;
            //Reminders sent every ten minutes
            remindTimer.Interval = 600000;
            //Start the timers
            updateTimer.Start();
            remindTimer.Start();
            //Keep the application running
            while (true)
            {
            }
        }
        //Event that is called for updating
        private static void update(Object o, EventArgs e)
        {
            //Stop the timer
            updateTimer.Stop();
            //Perform updates
            UpdateShows.Update();
            //Restart the timer
            updateTimer.Enabled = true;
        }
        //Event that is called for reminding
        private static void remind(Object o, EventArgs e)
        {
            //Stop the timer
            remindTimer.Stop();
            //Send any reminders
            SendReminders.Reminders();
            //Restart the timer
            remindTimer.Enabled = true;
        }
    }
}
