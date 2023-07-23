using DataAuth.Core;
using DataAuth.DataPermissions;
using DataAuth.Entities;
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
        public async Task<IEnumerable<DataPermissionModel>> Get()
        {
            return await _dataPermissionService.GetDataPermissions();
        }

        // GET api/<DataPermissionsController>/5
        [HttpGet("{id}")]
        public async Task<DataPermissionModel?> Get(int id)
        {
            return await _dataPermissionService.GetDataPermissionById(id);
        }

        // POST api/<DataPermissionsController>
        [HttpPost]
        public async Task Post([FromBody] DataPermissionModel model)
        {
            var entity = new DataPermission(
                model.GrantType,
                model.SubjectId,
                model.AccessAttributeTableId,
                model.AccessLevel,
                model.GrantedDataValue
            );
            await _dataPermissionService.AddDataPermission(entity);
        }

        // PUT api/<DataPermissionsController>/5
        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] DataPermissionModel model)
        {
            var entity = new DataPermission(
                model.GrantType,
                model.SubjectId,
                model.AccessAttributeTableId,
                model.AccessLevel,
                model.GrantedDataValue
            )
            {
                Id = id
            };
            await _dataPermissionService.UpdateDataPermission(entity);
        }

        // DELETE api/<DataPermissionsController>/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _dataPermissionService.DeleteDataPermission(id);
        }
    }
}
