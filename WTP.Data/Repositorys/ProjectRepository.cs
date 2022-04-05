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
    public class ProjectRepository : IProjectRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ProjectRepository(IMapper mapper, AppDbContext context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Guid> AddProject(ProjectDto project)
        {
            var projectToSave = _mapper.Map<Project>(project);
            _context.Project.Add(projectToSave);
   
            ProgressPlan progress = new();
            progress.ProjectId = projectToSave.ProjectId;
            _context.ProgressPlan.Add(progress);

            await _context.SaveChangesAsync();
            return projectToSave.ProjectId;
        }

        public async Task<List<Project>> GetProjectAsync(Guid Id)
        {
            return await _context.Project
                .Include(m => m.Manager)
                .Where(x => x.ProjectId == Id).ToListAsync();
        }

        public async Task<List<ProjectDto>> GetAllProjects()
        {
            var project = await _context.Project
                .Include(m => m.Manager)
                .ToListAsync(); ;

            var prosjectToReturn = _mapper.Map<List<ProjectDto>>(project);
            return prosjectToReturn;
        }

        public async Task RemoveProjectAsync(Guid id)
        {
            var project = _context.Project.Where(x => x.ProjectId == id).FirstOrDefault();
            project.IsDeleted = true;
            var progress = _context.ProgressPlan.Where(x => x.ProjectId == id).FirstOrDefault();
            progress.IsDeleted = true;

            await _context.SaveChangesAsync();
        }

        public async Task UpdateProjectAsync(ProjectDto project)
        {
            var prosjectToReturn = _context.Project
               .Where(x => x.ProjectId == project.ProjectId).FirstOrDefault();

            if (prosjectToReturn != null)
            {
                prosjectToReturn.Number = project.Number ?? prosjectToReturn.Number;
                prosjectToReturn.Title = project.Title ?? prosjectToReturn.Title;
                prosjectToReturn.Place = project.Place ?? prosjectToReturn.Place;
                prosjectToReturn.Status = project.Status ?? prosjectToReturn.Status;
                prosjectToReturn.DateUpdated = DateTime.UtcNow;
            }

            _context.Entry(prosjectToReturn).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}