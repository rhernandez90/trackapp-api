using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Entities
{
    public class Persons : BaseEntity
    {
        public int Id { get; set; }

        [Required]
        [Column(TypeName ="varchar(25)")]
        public string FirstName { get; set; }

        [Required]
        [Column(TypeName = "varchar(25)")]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set;  }
    }
}
