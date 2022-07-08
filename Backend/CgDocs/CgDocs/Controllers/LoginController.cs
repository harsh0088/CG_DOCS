using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CgDocs.Models;
using CgDocs.RequestModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace cgDocs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        public static int id;
        public static string name;
        private IConfiguration _config;
        private readonly CgDocsContext _cgDocsContext;
        public LoginController(IConfiguration config, CgDocsContext _cgDocs)
        {
            _config = config;
            _cgDocsContext = _cgDocs;
        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult CreateToken([FromBody]LoginModel login)
        {
            IActionResult response = Unauthorized();
            var user = Authenticate(login);

            if (user != null)
            {
                var tokenString = BuildToken(user);
                response = Ok(new { token = tokenString, userid = id, username = name });
            }

            return response;
        }
        private string BuildToken(UserRequest user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              expires: DateTime.Now.AddMinutes(30),
              signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private UserRequest Authenticate(LoginModel login)
        {
            UserRequest user = null;
            var answer = _cgDocsContext.Users.FirstOrDefault(o => o.Username == login.Username);
            try
            {
                if (answer.Username != null && answer.UserPassword == login.Password)
                {
                    user = new UserRequest { Username = answer.Username, UserPassword = answer.UserPassword };
                    id = answer.UsersId;
                    name = answer.Username;
                }
            }
            catch (Exception e)
            {
                return null;
            }
            return user;
        }
        public class LoginModel
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }
}
