using System.Collections.Generic;
using Orchard.Localization;
using Wkong.SchedulingTask.Models;
using Orchard;
namespace Wkong.SchedulingTask.Services
{
    public interface ISchedulingTask : IDependency
    {
        string Name { get; }
        LocalizedString Category { get; }
        LocalizedString Description { get; }

        string Form { get; }
        string MessageName { get; }
    }
}