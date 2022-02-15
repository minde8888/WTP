using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WTP.Domain.Dtos
{
    public class ProgressPlanDto
    {
        public Guid ProgressPlanId { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public string Index { get; set; }
        public Guid ManagerId { get; set; }
    }
}
