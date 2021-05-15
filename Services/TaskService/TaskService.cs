using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Entities;
using WebApi.Helpers;
using WebApi.Services.TaskService.Dto;

namespace WebApi.Services.TaskService
{


    public class TaskService : ITaskService
    {
        private readonly DataContext _context;
        public TaskService(
            DataContext context    
        )
        {
            _context = context;
        }


        public async Task<RequestResponseDto> Create(TaskDto taskData)
        {
            var task = new Tasks()
            {
                TaskName = taskData.TaskName,
                Description = taskData.Description,
                ProjectId = taskData.ProjectId,
                StartDate = taskData.StartDate,
                EndDate = taskData.EndDate,
                Note = taskData.Note
            };

            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();

            return new RequestResponseDto { Key = task.Id, Data = task };
        }

        Task<RequestResponseDto> ITaskService.Create(TaskDto taksData)
        {
            throw new NotImplementedException();
        }

        Task<RequestResponseDto> ITaskService.Delete(int id)
        {
            throw new NotImplementedException();
        }

        Task<RequestResponseDto> ITaskService.GetAll()
        {
            throw new NotImplementedException();
        }

        Task<RequestResponseDto> ITaskService.GetById(int id)
        {
            throw new NotImplementedException();
        }

        Task<RequestResponseDto> ITaskService.Update(int id, TaskDto taskData)
        {
            throw new NotImplementedException();
        }
    }
}
