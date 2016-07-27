using Orchard;
using Orchard.Events;
namespace Wkong.SchedulingTask
{
   public interface IStaticService:IEventHandler
    {
       void CreateFile( string relativeUrl);
       void CreateJob(string relativeUrl);
    }
}
