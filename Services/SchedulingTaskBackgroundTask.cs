using Orchard.Tasks;
using Orchard;
namespace Wkong.SchedulingTask.Services {
    public class SchedulingTaskBackgroundTask : Component, IBackgroundTask {
        private readonly ISchedulingTaskProcessor _schedulingTaskProcessor;
        public SchedulingTaskBackgroundTask(ISchedulingTaskProcessor SchedulingTaskProcessor)
        {
            _schedulingTaskProcessor = SchedulingTaskProcessor;
        }

        public void Sweep() {
            _schedulingTaskProcessor.ProcessTask();
        }
    }
}