using System;
using System.Collections.Generic;


namespace WTP.Domain.Entities
{
    public class Employee : BaseEntity
    {
        public Guid? ManagerId { get; set; }
        public Guid? ProjectId { get; set; }
        public ICollection<ProgressPlan> ProgressPlan { get; set; }
    }
}
