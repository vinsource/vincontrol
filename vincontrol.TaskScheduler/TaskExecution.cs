using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vincontrol.TaskScheduler
{
    public class TaskExecution
    {
        private const int OneDay = 1440;
        private const int Forever = 1440*365*3;

        public static bool CheckTasknameExist(string taskName)
        {
            var st = new ScheduledTasks(@"\\"+System.Environment.MachineName);
            var jobs = new List<string>(st.GetTaskNames());
            return jobs.Contains(taskName+".job");
        }
        
        public void CreateDailyTask(string taskName, string applicationPath, string parameter, DateTime runningTime, int frequency, string userDomain, string passwordDomain)
        {
            //Get a ScheduledTasks object for the local computer.
            var st = new ScheduledTasks();

            // Create a task
            Task t;
            try
            {
                t = st.CreateTask(taskName);
            }
            catch (ArgumentException)
            {
                //throw new Exception("Task name already exists");
                st.DeleteTask(taskName);
                t = st.CreateTask(taskName);
            }

            // Fill in the program info
            t.ApplicationName = applicationPath;
            t.Parameters = parameter;
            t.Comment = "";
            
            // Set the account under which the task should run.
            t.SetAccountInformation(@userDomain, passwordDomain);

            // Declare that the system must have been idle for ten minutes before 
            // the task will start
            t.IdleWaitMinutes = 10;

            // Allow the task to run for no more than 2 hours, 30 minutes.
            t.MaxRunTime = new TimeSpan(2, 30, 0);

            // Set priority to only run when system is idle.
            t.Priority = System.Diagnostics.ProcessPriorityClass.Idle;

            // Create a trigger to start the task every hour:minute.
            var dailyTrigger = new DailyTrigger(Convert.ToInt16(runningTime.Hour), Convert.ToInt16(runningTime.Minute)){DurationMinutes = Forever};
            if (frequency > 1)
            {
                dailyTrigger.IntervalMinutes = OneDay / frequency;
            }
            t.Triggers.Add(dailyTrigger);
            
            // Save the changes that have been made.
            t.Save(); 
            if (DateTime.Now.Hour == runningTime.Hour && DateTime.Now.Minute == runningTime.Minute) t.Run();
            // Close the task to release its COM resources.
            t.Close();
            // Dispose the ScheduledTasks to release its COM resources.
            st.Dispose();
        }

        public void UpdateDailyTask(string taskName, DateTime runningTime, int frequency)
        {
            // Get a ScheduledTasks object for the local computer.
            var st = new ScheduledTasks();

            // Open a task we're interested in
            Task task = st.OpenTask(taskName);

            // Be sure the task was found before proceeding
            if (task != null)
            {
                // Enumerate each trigger in the TriggerList of this task
                foreach (Trigger tr in task.Triggers)
                {
                    // If this trigger has a start time, change it to hour:minute.
                    if (tr is StartableTrigger)
                    {
                        (tr as StartableTrigger).StartHour = Convert.ToInt16(runningTime.Hour);
                        (tr as StartableTrigger).StartMinute = Convert.ToInt16(runningTime.Minute);
                        (tr as StartableTrigger).IntervalMinutes = OneDay / frequency;
                    }
                }
                task.Save();
                task.Close();
            }
            st.Dispose();
        }

        public void UpdateAccountInformation(string taskName, string userDomain, string passwordDomain)
        {
            // Get a ScheduledTasks object for the local computer.
            var st = new ScheduledTasks();

            // Open a task we're interested in
            Task task = st.OpenTask(taskName);

            // Be sure the task was found before proceeding
            if (task != null)
            {
                task.SetAccountInformation(userDomain, passwordDomain);
                task.Save();
                task.Close();
            }
            st.Dispose();
        }

        public void DeleteTask(string taskName)
        {
            // Get a ScheduledTasks object for the local computer.
            var st = new ScheduledTasks();

            // Delete a task we're interested in
            st.DeleteTask(taskName); 

            st.Dispose();
        }
    }
}
