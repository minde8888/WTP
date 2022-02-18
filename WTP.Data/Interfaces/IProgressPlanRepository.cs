﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WTP.Domain.Dtos;
using WTP.Domain.Entities;

namespace WTP.Data.Interfaces
{
    public interface IProgressPlanRepository
    {
        public Task AddPlan(ProgressPlanDto progressPlan);
        public Task<List<ProgressPlan>> GetProgressPlanAsync(Guid Id);
        public Task<List<ProgressPlanDto>> GetAllProgressPlans();
    }
}