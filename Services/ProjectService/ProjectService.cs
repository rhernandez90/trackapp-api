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
            ProjectData.Id = project.Id;
            return ProjectData;
        }
    }
}
