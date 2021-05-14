using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Helpers.Enums;

namespace WebApi.Entities
{
    public class Tasks : BaseEntity
    {
        public int Id { get; set; }

        [Required]
        public string TaskName { get; set; }

        public string Description { get; set; }

        public StatusEnum Status { get; set; }

        public DateTime StartDate { get; set; }
   
        public DateTime EndDate { get; set; }

        public DateTime CompleteDate { get; set; }

        public String Note { get; set; }

        public Tasks()
        {
            Status = StatusEnum.Pending;
        }



    }
}
