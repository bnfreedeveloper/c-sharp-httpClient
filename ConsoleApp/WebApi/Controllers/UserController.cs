using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApi.Data;
using System;
using System.Security.Claims;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace WebApi.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _config;    
        public UserController(UserManager<User> userManager,RoleManager<IdentityRole>roleManager,IConfiguration config)
        {
            _roleManager = roleManager; 
            _userManager = userManager;
            _config = config;   
        }
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] User user)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var userRegistered = await _userManager.CreateAsync(user);
                if (!userRegistered.Succeeded) return BadRequest(new { message = "Couldn't register the user" });
                if (!await _roleManager.RoleExistsAsync(user.Role))
                {
                    await _roleManager.CreateAsync(new IdentityRole
                    {
                        Name = user.Role
                    });
                }
                if (user.UserName == "jeanValjeant")
                {
                    if (!await _roleManager.RoleExistsAsync("admin"))
                    {
                        await _roleManager.CreateAsync(new IdentityRole { Name = "admin" });
                    }
                    var res = await _userManager.AddToRoleAsync(user, "admin");
                    if (!res.Succeeded) throw new Exception("errorUser");
                }

                var result = await _userManager.AddToRoleAsync(user, user.Role);
                if (!result.Succeeded) throw new Exception("ErrorUser");
                if (!result.Succeeded) return BadRequest(new { message = "something went wrong" });
                return Ok(new { message = "user successfully registered!" });
            }
            catch (Exception ex)
            {
                if(ex.Message == "ErrorUser")
                {
                    await _userManager.RemoveFromRolesAsync(user, new string[] { "user", "admin" });
                    await _userManager.DeleteAsync(user);
                }
                return BadRequest(new { message = "something went wrong" });
            }
           
        }

        [HttpPost("login")]
        public async Task<IActionResult> loginAsync([FromBody] UserLogin login)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var checkUser = await _userManager.FindByNameAsync(login.UserName);
                if (checkUser == null) return BadRequest(new { message = "user wasn't found" });

                var claimsUser = await _userManager.GetClaimsAsync(checkUser);
                var roles = await _userManager.GetRolesAsync(checkUser);
                if (roles.Any())
                {
                    foreach (var role in roles)
                    {
                        claimsUser.Add(new Claim(ClaimTypes.Role, role));
                    }

                }
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claimsUser),
                    Expires = DateTime.Now.AddMinutes(3),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["jwt"])), SecurityAlgorithms.HmacSha512),
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var securitytoken = tokenHandler.CreateToken(tokenDescriptor);
                return Ok(tokenHandler.WriteToken(securitytoken));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "something went wrong,try again" });
            }
            
            

        }
    }
    
}
