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
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<AccessAttributeTablesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<AccessAttributeTablesController>
        [HttpPost]
        public async Task Post([FromBody] AccessAttributeTableModel model)
        {
            await _accessAttributeTableService.AddAccessAttributeTable(model);
        }

        // PUT api/<AccessAttributeTablesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value) { }

        // DELETE api/<AccessAttributeTablesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id) { }
    }
}
