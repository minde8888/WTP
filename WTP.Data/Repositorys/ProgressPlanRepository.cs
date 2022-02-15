using AutoMapper;
using System.Threading.Tasks;
using WTP.Data.Context;
using WTP.Domain.Dtos;
using WTP.Domain.Entities;

namespace WTP.Data.Repositorys
{
    public class ProgressPlanRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public async Task AddPlan(ProgressPlanDto progressPlan)
        {
            var projectToSave = _mapper.Map<ProgressPlan>(progressPlan);
            await _context.AddAsync(projectToSave);
            await _context.SaveChangesAsync();
        }
    }
}
