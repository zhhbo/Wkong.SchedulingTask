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
        /// Ƶ�ʣ�ʱ,���죬�죬�ܣ��£�
        /// ֵ��-1��0��1��2��3
        /// </summary>
        public virtual int Frequency { get; set; }
        /// <summary>
        /// ���ʱ����ÿ���೤ʱ��
        /// </summary>
        public virtual int SpaceNum { get; set; }
    }
}