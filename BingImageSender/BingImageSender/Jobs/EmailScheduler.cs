

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
            

            ITrigger trigger = TriggerBuilder.Create()  // создаем триггер
                .WithIdentity("trigger1", "group1")     // идентифицируем триггер с именем и группой
                .StartNow()                            // запуск сразу после начала выполнения
                .WithSimpleSchedule(x => x            // настраиваем выполнение действия
                    .WithIntervalInMinutes(1)          // через 1 минуту
                    .WithRepeatCount(2))                   // бесконечное повторение
                .Build();                               // создаем триггер

            scheduler.ScheduleJob(job, trigger);        // начинаем выполнение работы
        }
    }
}