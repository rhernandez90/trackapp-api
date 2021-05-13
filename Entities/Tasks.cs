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

        [Column(TypeName="Date")]
        public DateTime StartDate { get; set; }

        [Column(TypeName = "Date")]
        public DateTime EndDate { get; set; }

        public Tasks()
        {
            Status = StatusEnum.Pending;
        }



    }
}
