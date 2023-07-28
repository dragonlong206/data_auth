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

        [HttpPost("{id}/roles")]
        public async Task AddRole(
            [FromRoute] string id,
            [FromBody] IEnumerable<int> roleIds,
            CancellationToken cancellationToken
        )
        {
            await _userRoleService.AddUserRoles(id, roleIds, cancellationToken);
        }
    }
}
