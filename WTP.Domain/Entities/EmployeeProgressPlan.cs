using System;

namespace WTP.Domain.Entities
{
    public class EmployeeProgressPlan
    {
        public Guid EmployeesId { get; set; }
        public Employee Employee { get; set; }
        public Guid ProgressPlanId { get; set; }
        public ProgressPlan ProgressPlan { get; set; }

    }
}
