using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Services.TaskService;
using WebApi.Services.TaskService.Dto;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _tasksService;


        public TaskController(ITaskService tasksService)    
        {
            _tasksService = tasksService;
        }

        [Authorize(Roles = "Role.Admin,Role.User")]
        [HttpPost("")]
        public async Task<ActionResult> Create([FromBody] CreateTaskDto taskData)
        {
            try
            {
                var task = await _tasksService.Create(taskData);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"{ex.Message}, {ex.InnerException}" });
            }

        }

        [Authorize(Roles = "Role.Admin")]
        [HttpGet("")]
        public async Task<ActionResult> GetAll()
        {
            var tasks = await _tasksService.GetAll();
            return Ok(tasks);
        }

        [Authorize]
        [HttpGet("{Id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var task = await _tasksService.GetById(id);
            if (task == null)
                return NotFound();

            return Ok(task);
        }

        [Authorize]
        [HttpPut("{Id}")]
        public async Task<ActionResult> Update(int Id, TaskDto taskData)
        {
            try
            {
                var task = await _tasksService.Update(Id, taskData);
                if (task == null)
                    return NotFound();

                return Ok(task);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Role.Admin")]
        [HttpDelete("{Id}")]
        public async Task<ActionResult> Delete(int Id)
        {
            try
            {
                var task = await _tasksService.Delete(Id);
                if (task == null)
                    return NotFound();

                return Ok(task);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("{TaskId}/AssignPerson/{PersonId}")]
        public async Task<ActionResult> AssignTask(int TaskId, int PersonId)
        {
            try
            {
                var task = await _tasksService.AssignTask(TaskId,PersonId);
                return Ok(task);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


    }

}
