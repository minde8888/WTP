using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ImagesService _imagesService;
        private readonly IMapper _mapper;

        public ProjectController(IProjectRepository projectRepository,
            IWebHostEnvironment hostEnvironment,
            ImagesService imagesService,
            IMapper mapper)

        {
            _projectRepository = projectRepository;
            _hostEnvironment = hostEnvironment;
            _imagesService = imagesService;
            _mapper = mapper;
        }

        [HttpPost]
        public IActionResult AddNewProject([FromBody] ProjectDto project)
        {
            try
            {
                _projectRepository.AddProject(project);
                return CreatedAtAction("Get", new { project.ProjectId }, project);
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
        [Authorize(Roles = "Manager, Admin")]
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

        [HttpPut("Update/{id}")]
        [Authorize(Roles = "Manager, Admin")]
        public async Task<ActionResult<List<ProjectDto>>> Update([FromBody] ProjectDto project)
        {
            if (project.ProjectId == Guid.Empty)
                return BadRequest("This project can not by updated");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                _projectRepository.UpdateProjectAsync(project);
                return CreatedAtAction("Get", new { project.ProjectId }, project);
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

        [HttpDelete("Delete/{id}")]
        [Authorize(Roles = "Manager, Admin")]
        public async Task<ActionResult> DeleteManager(String id)
        {
            var projectId = new Guid(id);
            if (projectId == Guid.Empty)
                return BadRequest();

            await _projectRepository.RemoveProjectAsync(projectId);
            return Ok();
        }
    }
}