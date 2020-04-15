using System;

namespace Zeze.Domin.Models
{
    public class BaseTraceEntity : BaseEntity
    {
        public BaseTraceEntity()
        {
            LastUpdatedOnUtc = DateTime.UtcNow;
            CreatedOnUtc = DateTime.UtcNow;
            EntityStatus = DataEntityStatus.Normal;
        }

        public DateTime LastUpdatedOnUtc { get; set; }

        public DateTime CreatedOnUtc { get; set; }

        public int CreatedBy { get; set; }

        public int LastUpdatedBy { get; set; }

        public DataEntityStatus EntityStatus { get; set; }
    }

    public enum DataEntityStatus
    {
        Normal = 0,

        Deleted = 1
    }
}
