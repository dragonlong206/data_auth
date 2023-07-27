using DataAuth.AccessAttributeTables;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DataAuth.Sample.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccessAttributeTablesController : ControllerBase
    {
        IAccessAttributeTableService _accessAttributeTableService;

        public AccessAttributeTablesController(
            IAccessAttributeTableService accessAttributeTableService
        )
        {
            _accessAttributeTableService = accessAttributeTableService;
        }

        // GET: api/<AccessAttributeTablesController>
        [HttpGet]
        public async Task<IEnumerable<AccessAttributeTableModel>> Get()
        {
            return await _accessAttributeTableService.GetAccessAttributeTables();
        }

        // GET api/<AccessAttributeTablesController>/5
        [HttpGet("{id}")]
        public async Task<AccessAttributeTableModel?> Get(int id)
        {
            return await _accessAttributeTableService.GetAccessAttributeTableById(id);
        }

        // POST api/<AccessAttributeTablesController>
        [HttpPost]
        public async Task Post([FromBody] AccessAttributeTableModel model)
        {
            await _accessAttributeTableService.AddAccessAttributeTable(model);
        }

        // PUT api/<AccessAttributeTablesController>/5
        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] AccessAttributeTableModel model)
        {
            await _accessAttributeTableService.UpdateAccessAttributeTable(id, model);
        }

        // DELETE api/<AccessAttributeTablesController>/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _accessAttributeTableService.DeleteAccessAttributeTable(id);
        }
    }
}
