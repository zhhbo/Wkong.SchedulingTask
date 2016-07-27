using Newtonsoft.Json;
using Orchard.Data;
using Wkong.SchedulingTask.Models;
using Orchard.Services;
using System;
namespace Wkong.SchedulingTask.Services {
    public class SchedulingTaskService : ISchedulingTaskService {
        private readonly IClock _clock;
        private readonly IRepository<SchedulingTaskRecord> _schedulingTaskRepository;
        private readonly ITransactionManager _transactionManager;
        public SchedulingTaskService(
            IClock clock, 
            ITransactionManager transactionManager,
            IRepository<SchedulingTaskRecord> schedulingTaskRepository) {
            _clock = clock;
            _schedulingTaskRepository = schedulingTaskRepository;
            _transactionManager = transactionManager;
        }

        public SchedulingTaskRecord Enqueue(string taskName,string message, int priority,DateTime scheduledUtc,object parameters ) {

            var schedulingTask = new SchedulingTaskRecord {
                Parameters = JsonConvert.SerializeObject(parameters),
                Message = message,
                CreatedUtc = _clock.UtcNow,
                Priority = priority,
                ScheduledUtc=scheduledUtc,
                CanExecute=true,
                TaskName=taskName
            };

            _schedulingTaskRepository.Create(schedulingTask);
            _transactionManager.RequireNew();
            return schedulingTask;
        }
        public SchedulingTaskRecord EditTask(int id, object parameters)
        {
            var schedulingTask = _schedulingTaskRepository.Get(id);

            schedulingTask.Parameters = JsonConvert.SerializeObject(new { parameters = parameters }); ;//
                 schedulingTask.CanExecute=true;


            _schedulingTaskRepository.Update(schedulingTask);
            _transactionManager.RequireNew();
            return schedulingTask;
        }
        public SchedulingTaskRecord EditTask(SchedulingTaskRecord task)
        {
            _schedulingTaskRepository.Update(task);
            return task;
        }
        public SchedulingTaskRecord Enqueue(string taskName, string message, int priority, DateTime scheduledUtc, int Frequency, int SpaceNum)
        {

            var schedulingTask = new SchedulingTaskRecord
            {
                //Parameters = JsonConvert.SerializeObject(parameters),
                Message = message,
                CreatedUtc = _clock.UtcNow,
                Priority = priority,
                ScheduledUtc = scheduledUtc,
                TaskName = taskName,
                CanExecute = false,
                Frequency = Frequency,
                SpaceNum = SpaceNum
            };

            _schedulingTaskRepository.Create(schedulingTask);
            _transactionManager.RequireNew();
            return schedulingTask;
        }
        public SchedulingTaskRecord Enqueue(string taskName, string message, int priority, DateTime scheduledUtc, int Frequency, int SpaceNum, object parameters)
        {

            var schedulingTask = new SchedulingTaskRecord
            {
                Parameters = JsonConvert.SerializeObject(new { parameters = parameters }),
                Message = message,
                CreatedUtc = _clock.UtcNow,
                Priority = priority,
                ScheduledUtc = scheduledUtc,
                TaskName = taskName,
                CanExecute = true,
                Frequency = Frequency,
                SpaceNum = SpaceNum
            };

            _schedulingTaskRepository.Create(schedulingTask);
            _transactionManager.RequireNew();
            return schedulingTask;
        }
    }
}