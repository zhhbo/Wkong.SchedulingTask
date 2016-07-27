using System;
using Orchard.Data.Migration;
using Orchard.Environment.Extensions;
namespace Wkong.SchedulingTask {
    [OrchardFeature("Wkong.SchedulingTask")]
    public class Migrations : DataMigrationImpl {
         
        public int Create() {
            SchemaBuilder.CreateTable("SchedulingTaskRecord", table => table
                .Column<int>("Id", c => c.Identity().PrimaryKey())
                .Column<string>("Message", c => c.WithLength(64))
                .Column<string>("TaskName", c => c.WithLength(64))
                .Column<string>("Parameters", c => c.Unlimited())
                .Column<int>("Priority", c => c.WithDefault(0))
                .Column<int>("Frequency", c => c.WithDefault(0))
                .Column<int>("SpaceNum", c => c.WithDefault(0))
                .Column<bool>("CanExecute", c => c.WithDefault(false))
                .Column<DateTime>("ScheduledUtc")
                .Column<DateTime>("CreatedUtc")
                );

            return 1;
        }
    }
}