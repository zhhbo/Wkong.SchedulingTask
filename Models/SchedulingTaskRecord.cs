using System;
using Orchard.Data.Conventions;

namespace Wkong.SchedulingTask.Models {
    public class SchedulingTaskRecord {
        public virtual int Id { get; set; }
        public virtual int Priority { get; set; }
        public virtual string Message { get; set; }
        public virtual string TaskName { get; set; }
        [StringLengthMax]
        public virtual string Parameters { get; set; }
        public virtual DateTime ScheduledUtc { get; set; }
        public virtual DateTime CreatedUtc { get; set; }
        public virtual bool CanExecute { get; set; }
        /// <summary>
        /// 频率，时,当天，天，周，月，
        /// 值：-1，0，1，2，3
        /// </summary>
        public virtual int Frequency { get; set; }
        /// <summary>
        /// 间隔时长，每隔多长时间
        /// </summary>
        public virtual int SpaceNum { get; set; }
    }
}