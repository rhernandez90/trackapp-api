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
        public async Task<ActionResult> Create([FromBody] PersonDto personData)
        {
            try
            {
                var person = await _personService.Create(personData);
                return Ok(person);
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
            var persons = await _personService.GetAll();
            return Ok(persons);
        }

        [Authorize]
        [HttpGet("{Id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var person = await _personService.GetById(id);
            if (person == null)
                return NotFound();

            return Ok(person);
        }

        [Authorize]
        [HttpPut("{Id}")]
        public async Task<ActionResult> Update(int Id, PersonDto taskData)
        {
            try
            {
                var person = await _personService.Update(Id, taskData);
                if (person == null)
                    return NotFound();

                return Ok(person);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
