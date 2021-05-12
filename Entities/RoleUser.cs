using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Entities
{
    public class RoleUser : BaseEntity
    {
        public int Id { get; set;}
        public int RoleId {get; set;}

        [ForeignKey("RoleId")]
        public Role Role { get; set;}
        
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set;}
        


    }
}