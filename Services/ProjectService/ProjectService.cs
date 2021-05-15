using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Entities;
using WebApi.Helpers;
using WebApi.Services.ProjectService.Dto;

namespace WebApi.Services.ProjectService
{
    public class ProjectService : IProjectService
    {
        private readonly DataContext _context;
        public ProjectService(
            DataContext context   
        )
        {
            _context = context;
        }


        public async Task<ProjectDto> Create(ProjectDto ProjectData)
        {
            var project = new Project()
            {
                Description = ProjectData.Description,
                Name = ProjectData.Name
            };

            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();
            ProjectData.Id = project.Id;
            
            return ProjectData;
        }

        public async Task<List<ProjectDto>> GetAll()
        {
            var projects = _context.Projects
                .Select( x => new ProjectDto {
                    Description = x.Description,
                    Name = x.Name,
                    Id = x.Id
            });

            return projects.ToList();
        }

        public async Task<ProjectDto> GetById(int id)
        {
            var project = _context.Projects.Find(id);
            if (project == null)
                return null;

            var projectDto = new ProjectDto()
            {
                Description = project.Description,
                Name = project.Name,
                Id = project.Id
            };

            return projectDto;
        }

        public async Task Delete(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project != null)
            {
                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();
            }
        }

        public async Task  Update(int id, ProjectDto projectData)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project != null)
            {
                project.Name = projectData.Name;
                project.Description = projectData.Description;
                _context.Projects.Update(project);
                await _context.SaveChangesAsync();

            }

        }


    }
}
