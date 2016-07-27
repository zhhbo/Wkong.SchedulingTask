using Orchard.Events;
using Wkong.SchedulingTask.Models;
using System;
using Orchard;
namespace Wkong.SchedulingTask.Services {
    public interface ISchedulingTaskService:IDependency {
        SchedulingTaskRecord Enqueue(string taskName, string message, int priority, DateTime scheduledUtc, object parameters);
        SchedulingTaskRecord EditTask(int id, object parameters);
        SchedulingTaskRecord EditTask(SchedulingTaskRecord task);
        SchedulingTaskRecord Enqueue(string taskName, string message, int priority, DateTime scheduledUtc, int Frequency, int SpaceNum);
        SchedulingTaskRecord Enqueue(string taskName, string message, int priority, DateTime scheduledUtc, int Frequency, int SpaceNum, object parameters);
    }
}