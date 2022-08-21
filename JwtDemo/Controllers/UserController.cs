using JwtDemo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly JwtHelper _jwt;

        public UserController(ILogger<UserController> logger, JwtHelper jwt)
        {
            _logger = logger;
            _jwt = jwt;
        }

        [HttpGet("Hello")]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("User")]
        public ActionResult CreateUser(UserViewModel model)
        {
            model.NickName = "台積郭雪芙";
            return Ok(model);
        }


        [Authorize]
        [HttpGet("User")]
        public ActionResult GetUsers()
        {
            var dummyUsers = new List<UserViewModel>()
            {
                new UserViewModel() { NickName = "三重劉德華", LastLogin = DateTime.Now },
                new UserViewModel() { NickName = "松山金城武", LastLogin = DateTime.Now },
                new UserViewModel() { NickName = "信義梁朝偉", LastLogin = DateTime.Now },
            };
            return Ok(dummyUsers);
        }
    }
}