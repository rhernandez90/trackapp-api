using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Services.TaskService;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("controller")]
    public class TascksController : ControllerBase
    {
        private readonly ITaskService _tasksService;


        public TascksController(ITaskService tasksService)
        {
            _tasksService = tasksService;
        }


    }
}
