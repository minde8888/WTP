using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        private readonly IProgressPlanRepository _progressPlanRepository;
        private readonly ProgressPlanService _progressPlanService;
        private readonly IMapper _mapper;

        public ProgressPlanController(IProgressPlanRepository progressPlanRepository, ProgressPlanService progressPlanService, IMapper mapper)
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

        [HttpGet("id")]
        [Authorize(Roles = "Manager, Admin")]
        public async Task<ActionResult<List<ProgressPlanDto>>> Get(String id)
        {
            try
            {
                var projectId = new Guid(id);
                if (projectId == Guid.Empty)
                    return BadRequest();

                var progressPlan = await _progressPlanRepository.GetProgressPlanAsync(projectId);
                if (progressPlan == null)
                    return NotFound();

                var getProject = _mapper.Map<List<ProgressPlanDto>>(progressPlan);

                return Ok(getProject);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Could not find Project info ");
            }
        }
    }
}
