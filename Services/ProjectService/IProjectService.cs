using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Entities;
using WebApi.Helpers;
using WebApi.Services.ProjectService.Dto;

namespace WebApi.Services.ProjectService
{
    public interface IProjectService
    {
        Task<RequestResponseDto> Create(ProjectDto ProjectData);
        Task<RequestResponseDto> GetAll();
        Task<RequestResponseDto> GetById(int id);
        Task<RequestResponseDto> Delete(int id);
        Task<RequestResponseDto> Update(int id, ProjectDto ProjectData);
    }
}
