

using Quartz;
using Quartz.Impl;

namespace BingImageSender.Jobs
{
    public class EmailScheduler
    {
        public static void Start(string email)
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();
            scheduler.Context.Put("email", email);

            IJobDetail job = JobBuilder.Create<EmailSender>().Build();
            

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("trigger", "group")   
                .StartNow()                           
                .WithSimpleSchedule(x => x            
                    .WithIntervalInMinutes(1)         
                    .WithRepeatCount(2))              
                .Build();                             

            scheduler.ScheduleJob(job, trigger);      
        }
    }
}