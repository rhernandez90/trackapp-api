using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Services.ProjectService.Dto
{
    public class ProjectDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string BackgroundColor { get; set; }
        public int PendingTasks { get; set; }
        public int InprogressTasks { get; set; }
        public int DoneTasks { get; set; }
        public int RejectedTasks { get; set; }
        public int OverdueTasks { get; set; }
    }
}
