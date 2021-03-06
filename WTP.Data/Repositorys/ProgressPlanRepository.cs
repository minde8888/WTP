using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WTP.Data.Context;
using WTP.Data.Interfaces;
using WTP.Domain.Dtos;
using WTP.Domain.Entities;

namespace WTP.Data.Repositorys
{
    public class ProgressPlanRepository : IProgressPlanRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ProgressPlanRepository(AppDbContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<ProgressPlan> AddPlanAsync(ProgressPlanDto progressPlan)
        {
            var projectToSave = _mapper.Map<ProgressPlan>(progressPlan);
            _context.ProgressPlan.Add(projectToSave);
            await _context.SaveChangesAsync();
            return projectToSave;
        }

        public async Task<List<ProgressPlan>> GetProgressPlanAsync(Guid Id)
        {
            return await _context.ProgressPlan
                .Where(x => x.ProgressPlanId == Id).ToListAsync();
        }

        public async Task<List<ProgressPlanDto>> GetAllProgressPlansAsync()
        {
            var plan = await _context.ProgressPlan.ToListAsync();

            var planToReturn = _mapper.Map<List<ProgressPlanDto>>(plan);
            return planToReturn;
        }

        public async Task RemoveProgressPlanAsync(Guid id)
        {
            var progress = _context.ProgressPlan.Where(x => x.ProgressPlanId == id).FirstOrDefault();
            progress.IsDeleted = true;

            await _context.SaveChangesAsync();
        }

        public async Task<ProgressPlanReturnDto> UpdateProgressPlanAsync(ProgressPlanDto progressPlan)
        {
            var planToReturn = _context.ProgressPlan.Include(x => x.Employees)
               .Where(x => x.ProgressPlanId == progressPlan.ProgressPlanId).FirstOrDefault();

            if (planToReturn != null)
            {
                planToReturn.Name = progressPlan.Name ?? planToReturn.Name;
                planToReturn.Color = progressPlan.Color ?? planToReturn.Color;
                planToReturn.Start = progressPlan.Start ?? planToReturn.Start;
                planToReturn.End = progressPlan.End ?? planToReturn.End;
                planToReturn.Index = progressPlan.Index ?? planToReturn.Index;
                planToReturn.DateUpdated = DateTime.UtcNow;
            }

            if (progressPlan.EmployeesIds != null && progressPlan.EmployeesIds != "null")
            {
                var toRemove = _context.EmployeeProgressPlan
                    .Where(x => x.ProgressPlanId == progressPlan.ProgressPlanId);
                _context.EmployeeProgressPlan.RemoveRange(toRemove);
                _context.SaveChanges();

                string[] ids = progressPlan.EmployeesIds.Split(',');

                foreach (var id in ids)
                {
                    var employeeProgress = new EmployeeProgressPlan
                    {
                        EmployeesId = new Guid(id.ToString()),
                        ProgressPlanId = progressPlan.ProgressPlanId
                    };
                    _context.EmployeeProgressPlan.Add(employeeProgress);
                    await _context.SaveChangesAsync();
                }

                planToReturn = _context.ProgressPlan
                    .Include(e => e.Employees)
                    .Single(x => x.ProgressPlanId == progressPlan.ProgressPlanId);
            }

            _context.Entry(planToReturn).State = EntityState.Modified;
            await _context.SaveChangesAsync();




            var progressToReturn = _mapper.Map<ProgressPlanReturnDto>(planToReturn);

            return progressToReturn;
        }
    }
}