using System;
using System.Configuration;
using NHibernate.Indexer;
using Quartz;
using Quartz.Impl;

namespace MyNHibernate.Infrastructure
{
    public class IndexScheduler
    {
        private IScheduler _Schedule;
        public IndexScheduler()
        {
            init();
        }

        public void Start()
        {
            _Schedule.Start();
        }

        public void Close()
        {
            _Schedule.Shutdown(true);
        }

        private void init()
        {
            int hour = Convert.ToInt32(ConfigurationManager.AppSettings["IndexStartHour"]);
            int min = Convert.ToInt32(ConfigurationManager.AppSettings["IndexStartMin"]);

            ISchedulerFactory sf = new StdSchedulerFactory();
            _Schedule = sf.GetScheduler();

            IJobDetail job = JobBuilder.Create<IndexJob>()
                .WithIdentity("job1", "group1")
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("trigger1", "group1")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(10)
                    .RepeatForever())
                .Build();

            ITrigger trigger2 = TriggerBuilder.Create()
                .WithIdentity("trigger2", "group1")
                .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(hour, min))
                .Build();

            ITrigger trigger3 = TriggerBuilder.Create()  //Build a trigger that will fire daily at 10:42 am:
                .WithIdentity("trigger3", "group1")
                .WithCronSchedule("0 42 10 * * ?")
                .Build();

            _Schedule.ScheduleJob(job, trigger);
        }

    }

    public class IndexJob : IJob
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(IndexJob));
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                Log.Info("start index .......................................");
                Index();
                Log.Info("index end .........................................");
            }
            catch (Exception ex)
            {
                Log.Error("start index exception:", ex);
            }
        }

        private void Index()
        {
            var cfg = NHibernateUtility.GetNHConfiguration();
            var sessionFactory = NHibernateUtility.GetSessionFactory();
            var indexPath = LuceneIndexHelper.GetIndexPath();
            using (var session = sessionFactory.OpenSession())
            {
                Indexer.CreateIndex(cfg, session, indexPath)
                       .Build();
            }

        }
    }
}