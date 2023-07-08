using System.Threading.Tasks;
using MediatR;
using Owleye.Application.Dto.Messages;
using Quartz;
using Owleye.Domain;
using Owleye.Infrastructure.Service;
using Owleye.Application.Sensors.Queries.GetSensorsList;

namespace Owleye.Application
{
    [DisallowConcurrentExecution]
    public class QuartzJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            var mediator = ServiceLocator.Resolve<IMediator>();

            JobDataMap dataMap = context.JobDetail.JobDataMap;
            SensorInterval interval = (SensorInterval)dataMap["Interval"];

            var query = new GetSensorsListByIntervalQuery
            {
                SensorInterval = interval
            };

            var sensors =  (mediator.Send(query)).Result;

            await mediator.Publish(new EndPointsCheckNotification
            {
                Sensors = sensors.Data
            });
        }
    }

}