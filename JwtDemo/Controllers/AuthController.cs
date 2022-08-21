using JwtDemo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly JwtHelper _jwt;

        public AuthController(ILogger<AuthController> logger, JwtHelper jwt)
        {
            _logger = logger;
            _jwt = jwt;
        }

        [HttpPost("Login")]
        public ActionResult Login(LoginViewModel loginModel)
        {
            if (LoginOk(loginModel))
            {
                var userRoles = this.GetUserRoles(loginModel);
                string token = _jwt.GenerateToken(loginModel.UserName, userRoles, 2);
                return Ok(new { token });
            }
            else
            {
                return BadRequest("帳號 or 密碼錯誤");
            }
        }

        // TODO: 使用雜湊等方式驗證加密後的密碼是否正確
        private bool LoginOk(LoginViewModel model)
        {
            return true;
        }

        // TODO: 撈 DB 資料決定使用者 "角色"
        private List<string> GetUserRoles(LoginViewModel model)
        {
            var roles = new List<string>();
            if (model.UserName.ToLower() == "admin")
            {
                roles.Add("Admin");
            }
            return roles;
        }
    }
}