using System.Threading.Tasks;
using WTP.Domain.Dtos;

namespace WTP.Data.Interfaces
{
    public interface IProgressPlan
    {
        public Task AddPlan(ProgressPlanDto progressPlan);
    }
}