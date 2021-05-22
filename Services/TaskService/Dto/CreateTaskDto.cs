using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Helpers.Enums;

namespace WebApi.Services.TaskService.Dto
{
    public class CreateTaskDto
    {
        [Required]
        public string TaskName { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public String Note { get; set; }
        public int ProjectId { get; set; }
        public int? AssignedPerson { get; set; }
    }
}
