using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using WTP.Data.Interfaces;
using WTP.Domain.Dtos;
using WTP.Services.Services;

namespace WTP.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("v1/api/[controller]")]
    public class ProgressPlanController : Controller
    {
        private readonly IProgressPlan _progressPlanRepository;
        private readonly ProgressPlanService _progressPlanService;
        private readonly IMapper _mapper;

        public ProgressPlanController(IProgressPlan progressPlanRepository, ProgressPlanService progressPlanService, IMapper mapper)
        {
            _progressPlanRepository = progressPlanRepository;
            _progressPlanService = progressPlanService;
            _mapper = mapper;
        }

        public IActionResult AddNewPlan([FromBody] ProgressPlanDto progressPlan)
        {
            try
            {
                progressPlan.ProgressPlanId = Guid.NewGuid();
                _progressPlanRepository.AddPlan(progressPlan);
                var projectToReturn = _progressPlanService.GetOnePlan(progressPlan); 
                return Ok(projectToReturn);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                "Error Add data to the database -> AddNewProjec");
            }
        }
    }
}
