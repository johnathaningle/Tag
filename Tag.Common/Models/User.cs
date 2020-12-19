using System.ComponentModel.DataAnnotations;

namespace Tag.Common.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}