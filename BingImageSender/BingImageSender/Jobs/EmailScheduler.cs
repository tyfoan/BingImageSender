

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

            IJobDetail job = JobBuilder.Create<EmailSender>()
                .WithIdentity(email)
                .Build();


            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity(email)
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(20)        
                    .WithRepeatCount(3))              
                .Build();

            if (!scheduler.CheckExists(trigger.Key))
                scheduler.ScheduleJob(job, trigger);      
        }
        public static void Unsubscribe(string email)
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();

            scheduler.DeleteJob(new JobKey(email));

        }
    }
}