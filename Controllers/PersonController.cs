using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Services.PersonService;
using WebApi.Services.PersonService.Dto;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly IPersonService _personService;
        public PersonController(
            IPersonService personService
            
            )
        {
            _personService = personService;
        }

        [Authorize(Roles = "Role.Admin")]
        [HttpPost("")]
        public async Task<ActionResult> Create([FromBody] PersonDto taskData)
        {
            try
            {
                var person = await _personService.Create(taskData);
                return Ok(task);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"{ex.Message}, {ex.InnerException}" });
            }

        }
    }
}
