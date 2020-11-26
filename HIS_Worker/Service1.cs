using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;

namespace HIS_Worker
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            
        }

        protected override void OnStop()
        {
        }
        private Timer Scheduler;

        public void ScheduleService()
        {
            try
            {
                Scheduler = new Timer(new TimerCallback(SchedularCallback));
                string mode= ConfigurationManager.AppSettings["Mode"].ToUpper();
                this.WriteToFile($"Simple Service Mode : {mode}");


                ///Set Default Time.
                ///
                DateTime scheduledTime = DateTime.MinValue;

                if (mode == "DAILY")
                {
                    // Get the Scheduled Time from AppSettings.
                    scheduledTime = DateTime.Parse(ConfigurationManager.AppSettings["ShceduledTime"]);

                    if(DateTime.Now > scheduledTime)
                    {
                        //If Time is passed set schedule for the next day.
                        scheduledTime = scheduledTime.AddDays(1);
                    }
                }

                if (mode == "INTERVAL")
                {
                    //Get the interval in minutes from AppSettings
                    int intervalMinutes = Convert.ToInt32(ConfigurationManager.AppSettings["IntervalMinutes"]);

                    //Set the scheduled time by adding the interval to current Time.
                    scheduledTime = scheduledTime.AddMinutes(intervalMinutes);

                    if (DateTime.Now > scheduledTime)
                    {
                        //If Scheduled Time is passed set Schedule for the next Interval.
                        scheduledTime = scheduledTime.AddMinutes(intervalMinutes);
                    }
                }

                TimeSpan timeSpan = scheduledTime.Subtract(DateTime.Now);
                string schedule = string.Format("{0} day(s) {1} hour(s) {2} minute(s) {3} seconds(s)", timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
                this.WriteToFile($"Simple Service to run after : {schedule}");

                //Get the difference in minutes between Scheduled and Current Time.
                int dueTime = Convert.ToInt32(timeSpan.TotalMilliseconds);
                //Chenge the Timer's Due Time.
                Scheduler.Change(dueTime, Timeout.Infinite);
                
            }
            catch (Exception ex)
            {
                WriteToFile("Simple Service Error on: {0} " + ex.Message + ex.StackTrace);

                //Stop the Windows Service.
              using(System.ServiceProcess.ServiceController serviceController=new ServiceController())
                {
                    serviceController.Stop();
                }
            }
        }

        private void SchedularCallback(object e)
        {
            this.WriteToFile("Simple Service Log: {0}");
            this.ScheduleService();
        }

        private void WriteToFile(string text)
        {
            string path = "C:\\ServiceLog.txt";
            using(StreamWriter writer=new StreamWriter(path, true))
            {
                writer.WriteLine(string.Format(text, DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt")));
                writer.Close();
            }
        }
    }
}
