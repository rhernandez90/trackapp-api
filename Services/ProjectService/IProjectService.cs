using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Services.ProjectService.Dto;

namespace WebApi.Services.ProjectService
{
    public interface IProjectService
    {
        Task<ProjectDto> Create(ProjectDto ProjectData);
    }
}
