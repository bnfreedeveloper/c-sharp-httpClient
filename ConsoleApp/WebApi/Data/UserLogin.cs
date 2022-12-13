using System.ComponentModel.DataAnnotations;

namespace WebApi.Data
{
    public class UserLogin
    {
        [Required]
        public string UserName { get; set; } 
        [Required]
        public string Password { get; set; }
    }
}
