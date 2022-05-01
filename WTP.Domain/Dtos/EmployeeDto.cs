using System;
using System.Collections.Generic;

namespace WTP.Domain.Dtos
{
    public class EmployeeDto: BaseEntityDto
    {
        public Guid? ManagerId { get; set; }
        public Guid? ProjectId { get; set; }
        public  ICollection<ProgressPlanReturnDto> ProgressPlans { get; set; }
    }
}
