using Owleye.Domain;
using Owleye.Infrastructure.Quartz;
using Owleye.Infrastructure.Service;
using Quartz.Impl;

namespace Owleye.Application
{
    public partial class QuartzBootStrap
    {
        public static async void Boot()
        {

            var scheduler = await StdSchedulerFactory.GetDefaultScheduler(); 
            await scheduler.Start();

            var schedulerService = ServiceLocator.Resolve<IQrtzSchedule>();

            schedulerService.schedule<QuartzJob>(scheduler, SensorInterval.OneMinute);
            schedulerService.schedule<QuartzJob>(scheduler, SensorInterval.ThirtySecond);
            schedulerService.schedule<QuartzJob>(scheduler, SensorInterval.FifteenMinutes);
            schedulerService.schedule<QuartzJob>(scheduler, SensorInterval.FiveMinute);
        }
    }

}