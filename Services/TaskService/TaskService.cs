using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Entities;
using WebApi.Helpers;
using WebApi.Helpers.Enums;
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

            if (taskData.AssignedPerson != null)
                await AssignTask(task.Id, (int)taskData.AssignedPerson);

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

            return new RequestResponseDto { Data = task };
        }

        public async Task<RequestResponseDto> GetByProject(int projectId, StatusEnum status)
        {
            var task = await _context.Tasks
                .Include( x=> x.PersonTasks)
                    .ThenInclude( x => x.Person)
                .Where( x => x.ProjectId == projectId && x.Status == status)
                .Select( s => new { 
                    s.Id,
                    s.Description,
                    s.TaskName,
                    s.Status,
                    StatusLabel = s.Status.ToString(),
                    IsOverdue = s.EndDate < DateTime.Now,
                    s.StartDate,
                    s.EndDate,
                    s.CompleteDate,
                    s.Note,
                    persons = s.PersonTasks.Select( v => new { 
                        v.Id,
                        Name = v.Person.FirstName + " " + v.Person.LastName
                    })
                })
                .ToListAsync();

            if (task == null)
                return null;

            return new RequestResponseDto { Data = task };
        }

        public async Task<RequestResponseDto> GetAllByProject(int projectId)
        {
            var task = await _context.Tasks
                .Include(x => x.PersonTasks)
                    .ThenInclude(x => x.Person)
                .Where(x => x.ProjectId == projectId)
                .Select(s => new {
                    s.Id,
                    s.Description,
                    s.TaskName,
                    s.Status,
                    StatusLabel = s.Status.ToString(),
                    IsOverdue = s.EndDate < DateTime.Now,
                    s.StartDate,
                    s.EndDate,
                    s.CompleteDate,
                    s.Note,
                    persons = s.PersonTasks.Select(v => new {
                        v.Id,
                        Name = v.Person.FirstName + " " + v.Person.LastName
                    })
                })
                .ToListAsync();

            if (task == null)
                return null;

            return new RequestResponseDto { Data = task };
        }

        public async Task<RequestResponseDto> Update(int id, TaskDto taskData)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task != null)
            {
                task.TaskName = taskData.TaskName;
                task.Description = taskData.Description;
                task.StartDate = taskData.StartDate;
                task.EndDate = taskData.EndDate;
                task.Note = taskData.Note;
                task.Status = taskData.Status;

                if (taskData.Status == StatusEnum.Done && task.CompleteDate == null)
                    task.CompleteDate = DateTime.UtcNow;
                

                _context.Tasks.Update(task);
                await _context.SaveChangesAsync();

                return new RequestResponseDto { Key = task.Id, Data = task };
            }
            return null;
        }

        public async Task<RequestResponseDto> AssignTask(int taskId, int PersonId)
        {
            var personTask = new PersonTasks()
            {
                PersonId = PersonId,
                TaskId = taskId
            };

            await _context.PersonTasks.AddAsync(personTask);
            await _context.SaveChangesAsync();
            return new RequestResponseDto { Key = personTask.Id, Data = personTask };
        }

        
    }
}
