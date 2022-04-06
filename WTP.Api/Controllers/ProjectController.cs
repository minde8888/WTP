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
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("v1/api/[controller]")]
    public class ProjectController : Controller
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ProjectService _projectService;
        private readonly IMapper _mapper;

        public ProjectController(IProjectRepository projectRepository,
            IMapper mapper,
            ProjectService projectService)

        {
            _projectRepository = projectRepository;
            _mapper = mapper;
            _projectService = projectService;
        }

        [HttpPost]
        [Authorize(Roles = "Manager, Admin")]
        public async Task<IActionResult> AddNewProject([FromForm] ProjectDto project)
        {
            try
            {
                var id = await _projectRepository.AddProject(project);
                var projectToReturn = _projectService.GetOneProject(id);
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
        public async Task<ActionResult<List<ProjectDto>>> Get(String id)
        {
            try
            {
                var projectId = new Guid(id);
                if (projectId == Guid.Empty)
                    return BadRequest();

                var project = await _projectRepository.GetProjectAsync(projectId);
                if (project == null)
                    return NotFound();

                var getProject = _mapper.Map<List<ProjectDto>>(project);

                return Ok(getProject);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Could not find Project info ");
            }
        }

        [HttpGet]
        //[Authorize(Roles = "Manager, Admin")]
        public async Task<ActionResult<List<ProjectDto>>> GetAll()
        {
            try
            {
                return await _projectRepository.GetAllProjects();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                  "Error Get project data from the database");
            }
        }

        [HttpPut("Update")]
        [Authorize(Roles = "Manager, Admin")]
        public ActionResult<List<ProjectDto>> Update([FormData] ProjectDto project)
        {
            if (project.ProjectId == Guid.Empty)
                return BadRequest("This project can not by updated");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                _projectRepository.UpdateProjectAsync(project);
                var projectToReturn = _projectService.GetOneProject(project.ProjectId);
                return Ok(projectToReturn);
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

        [HttpPost("Delete")]
        [Authorize(Roles = "Manager, Admin")]
        public async Task<ActionResult> DeleteManager([FromBody] List<object> ids)
        {
            foreach (var p in ids)
            {
                var id = new Guid(p.ToString());

                if (id == Guid.Empty)
                    return BadRequest();

                await _projectRepository.RemoveProjectAsync(id);
            }
            return Ok();
        }
    }
}