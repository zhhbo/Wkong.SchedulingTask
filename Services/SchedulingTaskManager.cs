using System;
using System.Collections.Generic;
using System.Linq;
using Orchard.ContentManagement;
using Orchard.Data;
using Wkong.SchedulingTask.Models;
using Orchard.Settings;
using Orchard.Services;
using Orchard.Utility.Extensions;
namespace Wkong.SchedulingTask.Services {
    public class SchedulingTaskManager : ISchedulingTaskManager {
        private readonly IRepository<SchedulingTaskRecord> _schedulingTaskRepository;
        private readonly IClock _clock;
        private readonly IEnumerable<ISchedulingTask> _schedulingTasks;
        public SchedulingTaskManager(
            IRepository<SchedulingTaskRecord> schedulingTaskRepository, 
            IClock clock,
            IEnumerable<ISchedulingTask> schedulingTasks,
            ISiteService siteService) {
                _schedulingTaskRepository = schedulingTaskRepository;
                _clock = clock;
                _schedulingTasks = schedulingTasks;
              }


       public int GetTasksCount() {
            return _schedulingTaskRepository
                .Table
                .Count();
        }

        public IEnumerable<SchedulingTaskRecord> GetTasks(int startIndex, int pageSize) {
            var query = _schedulingTaskRepository
                .Fetch(x =>  x.ScheduledUtc<=_clock.UtcNow&&x.CanExecute)
                .OrderByDescending(x => x.Priority)
                .ThenByDescending(x => x.CreatedUtc);

            if(startIndex <0) {
                startIndex = 0;
                //query = query.Skip(startIndex).ToList();
            }
                
            //query = query.Take(pageSize);

            return query.Skip(startIndex).Take(pageSize).ToList();
        }
        public IEnumerable<SchedulingTaskRecord> GetAllTask(int startIndex, int pageSize)
        {
            IQueryable<SchedulingTaskRecord> query = _schedulingTaskRepository
                .Table
                .OrderByDescending(x => x.Priority)
                .ThenByDescending(x => x.CreatedUtc);

            if (startIndex > 0)
            {
                query = query.Skip(startIndex);
            }

            query = query.Take(pageSize);

            return query.ToList();
        }
        public SchedulingTaskRecord GetTask(int id) {
            return _schedulingTaskRepository.Get(id);
        }

        public void Delete(SchedulingTaskRecord job) {
            _schedulingTaskRepository.Delete(job);
        }
        public IEnumerable<ISchedulingTask> GetSchedulingTasks()
        {
            return _schedulingTasks.OrderBy(x => x.MessageName).ToReadOnlyCollection();
        }
        public ISchedulingTask GetSchedulingTaskByMessageName(string name)
        {
            return _schedulingTasks.FirstOrDefault(x => x.MessageName == name);
        }
    }
}