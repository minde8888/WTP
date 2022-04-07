using System;


namespace WTP.Domain.Dtos
{
    public class ProjectDto
    {
        public Guid ProjectId { get; set; }
        public string Number { get; set; }
        public string Title { get; set; }
        public string Place { get; set; }
        public string Status { get; set; }
        public Guid? ManagerId { get; set; }
        public ProgressPlanDto ProgressPlanDto { get; set; }
    }
}
