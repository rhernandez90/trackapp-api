using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Entities;
using WebApi.Helpers;
using WebApi.Helpers.Enums;
using WebApi.Services.ProjectService.Dto;
using WebApi.Services.TaskService;

namespace WebApi.Services.ProjectService
{   
    public class ProjectService : IProjectService
    {
        private readonly DataContext _context;
        private readonly ITaskService _taskService;
        public ProjectService(
            DataContext context,
            ITaskService taskService
        )
        {
            _context = context;
            _taskService = taskService;
        }


        public async Task<RequestResponseDto> Create(ProjectDto ProjectData)
        {
            var project = new Project()
            {
                Description = ProjectData.Description,
                Name = ProjectData.Name,
                BackgroundColor = ProjectData.BackgroundColor
            };

            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();
            
            return new RequestResponseDto { Key = project.Id, Data = project };
        }

        public async Task<RequestResponseDto> GetAll()
        {
            var projects = await _context
                .Projects
                .Include(x => x.Tasks)
                .Select( x => new ProjectDto {
                    Description = x.Description,
                    Name = x.Name,
                    Id = x.Id,
                    BackgroundColor = x.BackgroundColor,
                    DoneTasks = x.Tasks.Count(x => x.Status == StatusEnum.Done),
                    InprogressTasks= x.Tasks.Count(x => x.Status == StatusEnum.InProgress),
                    PendingTasks = x.Tasks.Count(x => x.Status == StatusEnum.Pending),
                    RejectedTasks = x.Tasks.Count(x => x.Status == StatusEnum.rejected),
                    OverdueTasks = x.Tasks.Count(x => x.EndDate < DateTime.Now)
                })
            .ToListAsync();

            return new RequestResponseDto {  Data = projects };
        }



        public async Task<RequestResponseDto> GetById(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
                return null;

            return new RequestResponseDto { Data = project }; ;
        }

        public async Task<RequestResponseDto> Delete(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project != null)
            {
                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();
                return new RequestResponseDto { Key = id, Message = "Removed"};
            }

            return null;
        }

        public async Task<RequestResponseDto>  Update(int id, ProjectDto projectData)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project != null)
            {
                project.Name = projectData.Name;
                project.Description = projectData.Description;
                _context.Projects.Update(project);
                await _context.SaveChangesAsync();

                return new RequestResponseDto { Key = project.Id, Data = project };
            }
            return null;
        }



    }
}
