using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WTP.Domain.Entities
{
    public class EmployeeProgressPlan
    {
        public Guid EmployeesId { get; set; }
        public Employee Employees { get; set; }
        public Guid ProgressPlanId { get; set; }
        public ProgressPlan ProgressPlans { get; set; }

    }
}
