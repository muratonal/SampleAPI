using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;

namespace SampleAPI
{
    public class StartSchedule
    {
        public static async void SchedulerSetup()
        {
            var _scheduler = await new StdSchedulerFactory().GetScheduler();
            await _scheduler.Start();

            var showDateTimeJob = JobBuilder.Create<FilmVeriCek>()
                .WithIdentity("TimeJob")
                .Build();
            var trigger = TriggerBuilder.Create()
                .WithIdentity("TimeJob")
                .StartNow()
                .WithSimpleSchedule(builder => builder.WithIntervalInHours(1).RepeatForever()) //.WithCronSchedule("*/1 * * * *")
                .Build();

            var result = await _scheduler.ScheduleJob(showDateTimeJob, trigger);
        }
    }
}
