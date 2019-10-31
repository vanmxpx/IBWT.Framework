﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IBWT.Framework.Scheduler.Cron;

namespace IBWT.Framework.Scheduler
{
    public class SchedulerHostedService : HostedService
    {
        public event EventHandler<UnobservedTaskExceptionEventArgs> UnobservedTaskException;

        private readonly List<SchedulerTaskWrapper> _scheduledTasks = new List<SchedulerTaskWrapper>();

        public SchedulerHostedService(IEnumerable<IScheduledTask> scheduledTasks)
        {

            foreach (var scheduledTask in scheduledTasks)
            {
                var schedule = CrontabSchedule.Parse(scheduledTask.Schedule);
                var nextRun = schedule.GetNextOccurrence(DateTime.Now);
                _scheduledTasks.Add(new SchedulerTaskWrapper
                {
                    Schedule = schedule,
                        Task = scheduledTask,
                        NextRunTime = nextRun
                });
            }
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await ExecuteOnceAsync(cancellationToken);

                await Task.Delay(TimeSpan.FromMinutes(1), cancellationToken);
            }
        }

        private async Task ExecuteOnceAsync(CancellationToken cancellationToken)
        {
            var taskFactory = new TaskFactory(TaskScheduler.Current);
            var referenceTime = DateTime.Now;

            var tasksThatShouldRun = _scheduledTasks.Where(t => t.ShouldRun(referenceTime)).ToList();

            foreach (var taskThatShouldRun in tasksThatShouldRun)
            {
                taskThatShouldRun.Increment();

                await taskFactory.StartNew(
                    async() =>
                    {
                        try
                        {
                            await taskThatShouldRun.Task.ExecuteAsync(cancellationToken);
                        }
                        catch (Exception ex)
                        {
                            var args = new UnobservedTaskExceptionEventArgs(
                                ex as AggregateException ?? new AggregateException(ex));

                            UnobservedTaskException?.Invoke(this, args);

                            if (!args.Observed)
                            {
                                throw;
                            }
                        }
                    },
                    cancellationToken);
            }
        }

        private class SchedulerTaskWrapper
        {
            public CrontabSchedule Schedule { get; set; }
            public IScheduledTask Task { get; set; }

            public DateTime LastRunTime { get; set; }
            public DateTime NextRunTime { get; set; }

            public void Increment()
            {
                LastRunTime = NextRunTime;
                NextRunTime = Schedule.GetNextOccurrence(NextRunTime);
            }

            public bool ShouldRun(DateTime currentTime)
            {
                return NextRunTime < currentTime && LastRunTime != NextRunTime;
            }
        }
    }
}