using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Entities;
using WebApi.Services.ProjectService.Dto;

namespace WebApi.Services.ProjectService
{
    public interface IProjectService
    {
        Task<ProjectDto> Create(ProjectDto ProjectData);
        Task<List<ProjectDto>> GetAll();
        Task<ProjectDto> GetById(int id);
        Task Delete(int id);
        Task Update(int id, ProjectDto ProjectData);
    }
}
