using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Helpers;
using WebApi.Helpers.Enums;
using WebApi.Services.TaskService.Dto;

namespace WebApi.Services.TaskService
{
    public interface ITaskService
    {
        Task<RequestResponseDto> Create(CreateTaskDto taksData);
        Task<RequestResponseDto> GetAll();
        Task<RequestResponseDto> GetOverdueTasks(int ProjectId);
        Task<RequestResponseDto> GetById(int id);
        Task<RequestResponseDto> GetByProject(int id, StatusEnum status);
        Task<RequestResponseDto> GetAllByProject(int id);
        Task<RequestResponseDto> Delete(int id);
        Task<RequestResponseDto> Update(int id, TaskDto taskData);
        Task<RequestResponseDto> AssignTask(int taskId, int PersonId);
    }
}
