using Microsoft.Build.Framework;

namespace EmsApi.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public Role Role { get; set; }
    }
}
