using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Wkong.SchedulingTask.ViewModels
{

    public class SchedulingTaskViewModel
    {
        public int Id { get; set; }
        public  int Priority { get; set; }
       // public  string Message { get; set; }
        public  string TaskName { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }

        /// <summary>
        /// 频率，时,当天，天，周，月，
        /// 值：-1，0，1，2，3
        /// </summary>
        public  int Frequency { get; set; }
        /// <summary>
        /// 间隔时长，每隔多长时间
        /// </summary>
        public  int SpaceNum { get; set; }
        public IList<SchedulingTaskEntry> SchedulingTasks { get; set; }

    }
    public class SchedulingTaskEntry
    {
        public bool Selected { get; set; }
        public string TaskName { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string MessageName { get; set; }
    }
}
