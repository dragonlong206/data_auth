using DataAuth.Domains.Roles;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DataAuth.Sample.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        // GET: api/<RolesController>
        [HttpGet]
        public async Task<IEnumerable<RoleModel>> Get(CancellationToken cancellationToken)
        {
            return await _roleService.GetRoles(cancellationToken);
        }

        // GET api/<RolesController>/5
        [HttpGet("{id}")]
        public async Task<RoleModel?> Get(int id, CancellationToken cancellationToken)
        {
            return await _roleService.GetRoleById(id, cancellationToken);
        }

        // POST api/<RolesController>
        [HttpPost]
        public async Task Post([FromBody] RoleModel model, CancellationToken cancellationToken)
        {
            await _roleService.AddRole(model, cancellationToken);
        }

        // PUT api/<RolesController>/5
        [HttpPut("{id}")]
        public async Task Put(
            int id,
            [FromBody] RoleModel model,
            CancellationToken cancellationToken
        )
        {
            await _roleService.UpdateRole(id, model, cancellationToken);
        }

        // DELETE api/<RolesController>/5
        [HttpDelete("{id}")]
        public async Task Delete(int id, CancellationToken cancellationToken)
        {
            await _roleService.DeleteRole(id, cancellationToken);
        }
    }
}
