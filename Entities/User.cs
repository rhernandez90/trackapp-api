using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public int? PersonId { get; set; }

        [ForeignKey("PersonId")]
        public Persons Person { get; set; }
        public ICollection<RoleUser> RoleUsers { get; set; }
    }
}