using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    [Table("User")]

    public class User
    {
        [Key]
        public int IdUser { get; set; }
        public String UserName { get; set; }
        public String Password { get; set; }
        public string Token { get; set; }

    }
}
