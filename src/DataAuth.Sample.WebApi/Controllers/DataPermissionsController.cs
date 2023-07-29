using DataAuth.Domains.DataPermissions;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DataAuth.Sample.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataPermissionsController : ControllerBase
    {
        IDataPermissionService _dataPermissionService;

        public DataPermissionsController(IDataPermissionService dataAuthService)
        {
            _dataPermissionService = dataAuthService;
        }

        // GET: api/<DataPermissionsController>
        [HttpGet]
        public async Task<IEnumerable<DataPermissionModel>> Get(CancellationToken cancellationToken)
        {
            return await _dataPermissionService.GetDataPermissions(
                cancellationToken: cancellationToken
            );
        }

        // GET api/<DataPermissionsController>/5
        [HttpGet("{id}")]
        public async Task<DataPermissionModel?> Get(int id, CancellationToken cancellationToken)
        {
            return await _dataPermissionService.GetDataPermissionById(id, cancellationToken);
        }

        // POST api/<DataPermissionsController>
        [HttpPost]
        public async Task Post(
            [FromBody] DataPermissionModel model,
            CancellationToken cancellationToken
        )
        {
            await _dataPermissionService.AddDataPermission(model, cancellationToken);
        }

        // PUT api/<DataPermissionsController>/5
        [HttpPut("{id}")]
        public async Task Put(
            int id,
            [FromBody] DataPermissionModel model,
            CancellationToken cancellationToken
        )
        {
            await _dataPermissionService.UpdateDataPermission(id, model, cancellationToken);
        }

        // DELETE api/<DataPermissionsController>/5
        [HttpDelete("{id}")]
        public async Task Delete(int id, CancellationToken cancellationToken)
        {
            await _dataPermissionService.DeleteDataPermission(id, cancellationToken);
        }
    }
}
