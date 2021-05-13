using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Entities
{
    public class PersonTasks  : BaseEntity
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public int TaskId { get; set; }

        [ForeignKey("PersonId ")]
        public Persons Person { get; set; }

        [ForeignKey("TaskId")]
        public Tasks Task { get; set; }

        public PersonTasks()
        {

        }
    }
}
