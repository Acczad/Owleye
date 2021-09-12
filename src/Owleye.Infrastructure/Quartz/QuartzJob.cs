using System.Threading.Tasks;
using MediatR;
using Owleye.Application.Dto.Messages;
using Quartz;
using Owleye.Domain;
using Owleye.Infrastructure.Service;
using Owleye.Application.Sensors.Queries.GetSensorsList;
using Owleye.Application.Dto;
using Owleye.Shared.Model;

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


            var sensors = await mediator.Send<QueryPagedResult<SensorDto>>(query);

            await mediator.Publish(new EndPointsCheckNotification
            {
                EndPointList = sensors.Data
            });
        }
    }

}