using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        public TaskService(
            DataContext context,
            IMapper mapper
        )
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<RequestResponseDto> Create(CreateTaskDto taskData)
        {
            var task = _mapper.Map<Tasks>(taskData); 
            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();
            return new RequestResponseDto { Key = task.Id, Data = task };
        }


        public async Task<RequestResponseDto> Delete(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task!= null)
            {
                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();
                return new RequestResponseDto { Key = id, Message = "Removed" };
            }

            return null;
        }

        public async Task<RequestResponseDto> GetAll()
        {
            var tasks = _context.Tasks.Include(x => x.Project);
            var taskList = _mapper.Map<List<TaskDto>>(tasks);
            return new RequestResponseDto { Data = taskList };  
        }

        public async  Task<RequestResponseDto> GetById(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
                return null;

            return new RequestResponseDto { Data = task }; ;
        }

        public async Task<RequestResponseDto> Update(int id, TaskDto taskData)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task != null)
            {
                task = _mapper.Map<Tasks>(taskData);
                _context.Tasks.Update(task);
                await _context.SaveChangesAsync();

                return new RequestResponseDto { Key = task.Id, Data = task };
            }
            return null;
        }
    }
}
