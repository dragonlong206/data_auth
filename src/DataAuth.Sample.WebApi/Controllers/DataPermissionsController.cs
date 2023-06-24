using DataAuth.Core;
using DataAuth.DataPermissions;
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
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<DataPermissionsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<DataPermissionsController>
        [HttpPost]
        public async Task Post([FromBody] DataPermissionModel model)
        {
            await _dataPermissionService.AddDataPermission(model);
        }

        // PUT api/<DataPermissionsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<DataPermissionsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
