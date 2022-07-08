using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CgDocs.Models;
using CgDocs.RequestModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace cgDocs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly CgDocsContext _cgDocsContext;
        public UserController(CgDocsContext userInfo)
        {
            _cgDocsContext = userInfo;
        }

        // GET: api/User
        [HttpGet]
        public IActionResult Get()
        {
            var getUser = _cgDocsContext.Users.ToList();
            return Ok(getUser);
        }


        [HttpPost]
        public void Post([FromBody] UserRequest value)
        {
            Users obj = new Users();
            obj.Username = value.Username;
            obj.UserPassword = value.UserPassword;
            obj.CreatedAt = value.CreatedAt;
            _cgDocsContext.Users.Add(obj);
            _cgDocsContext.SaveChanges();
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
