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
                return BadRequest("�b�� or �K�X���~");
            }
        }

        // TODO: �ϥ����굥�覡���ҥ[�K�᪺�K�X�O�_���T
        private bool LoginOk(LoginViewModel model)
        {
            return true;
        }

        // TODO: �� DB ��ƨM�w�ϥΪ� "����"
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