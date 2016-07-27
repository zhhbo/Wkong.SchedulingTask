using Orchard;
namespace Wkong.SchedulingTask.Services {
    public interface ISchedulingTaskProcessor : ISingletonDependency {
        void ProcessTask();
    }
}