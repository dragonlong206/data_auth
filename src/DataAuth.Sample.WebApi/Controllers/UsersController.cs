using DataAuth.UserRoles;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DataAuth.Sample.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        IUserRoleService _userRoleService;

        public UsersController(IUserRoleService userRoleService)
        {
            _userRoleService = userRoleService;
        }

        [HttpPost("roles")]
        public async Task AddRole([FromBody] UserRoleModel model)
        {
            await _userRoleService.AddUserRole(model);
        }
    }
}
