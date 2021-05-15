using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Entities;
using WebApi.Services.ProjectService;
using WebApi.Services.ProjectService.Dto;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
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
            var projects = _projectService.GetAll();
            return Ok(projects);
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

        [Authorize]
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
