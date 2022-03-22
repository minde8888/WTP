using System;
using System.Collections.Generic;

namespace WTP.Domain.Entities
{
    public class ProgressPlan
    {
        public Guid ProgressPlanId { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public string Index { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public DateTime? DateUpdated { get; set; }

        public ICollection<Employee> Employees { get; set; }
    }
}
