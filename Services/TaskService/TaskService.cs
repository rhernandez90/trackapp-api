﻿using Microsoft.EntityFrameworkCore;
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
            var tasks = await _context.Tasks
            .Select(x => new TaskDto
            {
                TaskName = x.TaskName,
                Description = x.Description,
                Status = x.Status,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                Note = x.Note,
                ProjectId = x.ProjectId
            })
            .ToListAsync();

            return new RequestResponseDto { Data = tasks };  
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
                task.TaskName = taskData.TaskName;
                task.Description = taskData.Description;
                task.ProjectId = taskData.ProjectId;
                task.StartDate = taskData.StartDate;
                task.EndDate = taskData.EndDate;
                task.Note = taskData.Note;
                task.Status = taskData.Status;
                task.CompleteDate = task.CompleteDate;
                _context.Tasks.Update(task);
                await _context.SaveChangesAsync();

                return new RequestResponseDto { Key = task.Id, Data = task };
            }
            return null;
        }
    }
}