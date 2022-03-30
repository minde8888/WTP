using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        [HttpPost]
        [Authorize(Roles = "Manager, Admin")]
        public async Task<IActionResult> AddNewPlan([FromForm] ProgressPlanDto progressPlan)
        {
            try
            {
                await _progressPlanRepository.AddPlanAsync(progressPlan);
                var projectToReturn = _progressPlanService.GetOnePlan(progressPlan);
                return Ok(projectToReturn);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                ex);
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

        [HttpGet]
        [Authorize(Roles = "Manager, Admin")]
        public async Task<ActionResult<List<ProgressPlanDto>>> GetAll()
        {
            try
            {
                return await _progressPlanRepository.GetAllProgressPlansAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                  ex);
            }
        }

        [HttpPost("Delete")]
        [Authorize(Roles = "Manager, Admin")]
        public async Task<ActionResult> DeleteManager([FromBody] List<object> ids)
        {
            foreach (var p in ids)
            {
                var id = new Guid(p.ToString());

                if (id == Guid.Empty)
                    return BadRequest();

                await _progressPlanRepository.RemoveProgressPlanAsync(id);
            }
            return Ok();
        }

        [HttpPut("Update")]
        [Authorize(Roles = "Manager, Admin")]
        public ActionResult<List<ProgressPlanDto>> Update([FromForm] ProgressPlanDto progressPlan)
        {
            if (progressPlan.ProgressPlanId == Guid.Empty)
                return BadRequest("This project can not by updated");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                _progressPlanRepository.UpdateProgressPlanAsync(progressPlan);
                var planToReturn = _progressPlanService.GetOnePlan(progressPlan);
                return Ok(planToReturn);
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                   "Error save DB");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                   ex);
            }
        }
    }
}