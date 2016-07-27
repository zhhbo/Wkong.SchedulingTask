using System.Collections.Generic;
using Wkong.SchedulingTask.Models;
using Orchard;
namespace Wkong.SchedulingTask.Services {
    public interface ISchedulingTaskManager : IDependency {
        SchedulingTaskRecord GetTask(int id);
        void Delete(SchedulingTaskRecord job);
        IEnumerable<SchedulingTaskRecord> GetTasks(int startIndex, int count);
        IEnumerable<SchedulingTaskRecord> GetAllTask(int startIndex, int pageSize);
        int GetTasksCount();
        IEnumerable<ISchedulingTask> GetSchedulingTasks();
        ISchedulingTask GetSchedulingTaskByMessageName(string name);
    }
}