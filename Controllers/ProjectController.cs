using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Entities;
using WebApi.Services.ProjectService;
using WebApi.Services.ProjectService.Dto;
using WebApi.Services.TaskService;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;
        private readonly ITaskService _taskService;

        public ProjectController(IProjectService projectService, ITaskService taskService)
        {
            _projectService = projectService;
            _taskService = taskService;
        }

        [Authorize(Roles = "Role.Admin")]
        [HttpPost("")]
        public async Task<ActionResult> Create([FromBody] ProjectDto ProjectData)
        {
            try
            {
                var project = await _projectService.Create(ProjectData);
                return Ok(project);
                //return CreatedAtRoute("GetDocument", new { guid = project }, project);
            }
            catch(Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }

        [Authorize]
        [HttpGet("")]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                var projects = await _projectService.GetAll();
                return Ok(projects);
            }
            catch ( Exception exp )
            {
                return BadRequest(exp.Message);
            }

        }

        [Authorize]
        [HttpGet("{Id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var project = await _projectService.GetById(id);
            if (project == null)
                return NotFound();

            return Ok(project);
        }

        [Authorize]
        [HttpGet("{Id}/Tasks")]
        public async Task<ActionResult> GetTasks(int id)
        {
            var tasks = await _taskService.GetByProject(id);
            if (tasks == null)
                return NotFound();

            return Ok(tasks);
        }


        [Authorize(Roles = "Role.Admin")]
        [HttpPut("{Id}")]
        public async Task<ActionResult> Update(int Id, ProjectDto ProjectData)
        {
            try
            {
                var project = await _projectService.Update(Id,ProjectData);
                if (project == null)
                    return NotFound();

                return Ok(project);
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
                var project = await _projectService.Delete(Id);
                if (project == null)
                    return NotFound();

                return Ok(project);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


    }
}
