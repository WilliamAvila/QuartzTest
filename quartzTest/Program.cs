using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Triggers;

namespace quartzTest
{
    class Program
    {
        private static IScheduler _scheduler;
        IScheduler _sched;
        static void Main(string[] args)
        {
            ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
            _scheduler = schedulerFactory.GetScheduler();
            _scheduler.Start();
            Console.WriteLine("Starting Scheduler");

            AddJob("Job1", DateBuilder.FutureDate(1, IntervalUnit.Day));
            AddJob("Job2", DateBuilder.FutureDate(2, IntervalUnit.Minute));
        }

        public static void AddJob(string jobId, DateTimeOffset date)
        {
            IMyJob myJob = new MyJob(); //This Constructor needs to be parameterless
            JobDetailImpl jobDetail = new JobDetailImpl("Job1", "Group1", myJob.GetType());
            jobDetail.Key = new JobKey(jobId);

            ITrigger trigger = (ISimpleTrigger)TriggerBuilder.Create()
                   .StartAt(date)
                   .ForJob(jobId)
                   .Build();
            _scheduler.ScheduleJob(jobDetail, trigger);
     
            DateTimeOffset? nextFireTime = trigger.GetNextFireTimeUtc();
            Console.WriteLine("Next Fire Time:" + nextFireTime.Value);
        }


}

    internal class MyJob : IMyJob
    {
        public void Execute(IJobExecutionContext context)
        {
            Console.WriteLine("In MyJob class");
            DoMoreWork();
        }

        public void DoMoreWork()
        {
            Console.WriteLine("Do More Work");
        }
    }

    internal interface IMyJob : IJob
    {
    }
}
