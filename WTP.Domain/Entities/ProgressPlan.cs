using System;

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
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public DateTime? DateUpdated { get; set; }
        public Guid ManagerId { get; set; }
        public Manager Manager { get; set; }
    }
}
