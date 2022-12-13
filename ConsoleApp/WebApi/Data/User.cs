using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Data
{
    public class User : IdentityUser
    {
       
        public string Password { get; set; }
        [Required]
        [Compare(nameof(Password),ErrorMessage ="password must match")] 
        public string ConfirmedPassword { get; set; }
        public string Role { get; set; } = "user";
    }
}
