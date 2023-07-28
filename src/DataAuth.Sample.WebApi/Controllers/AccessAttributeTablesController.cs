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
        public async Task<IEnumerable<AccessAttributeTableModel>> Get(
            CancellationToken cancellationToken
        )
        {
            return await _accessAttributeTableService.GetAccessAttributeTables(cancellationToken);
        }

        // GET api/<AccessAttributeTablesController>/5
        [HttpGet("{id}")]
        public async Task<AccessAttributeTableModel?> Get(
            int id,
            CancellationToken cancellationToken
        )
        {
            return await _accessAttributeTableService.GetAccessAttributeTableById(
                id,
                cancellationToken
            );
        }

        // POST api/<AccessAttributeTablesController>
        [HttpPost]
        public async Task Post(
            [FromBody] AccessAttributeTableModel model,
            CancellationToken cancellationToken
        )
        {
            await _accessAttributeTableService.AddAccessAttributeTable(model, cancellationToken);
        }

        // PUT api/<AccessAttributeTablesController>/5
        [HttpPut("{id}")]
        public async Task Put(
            int id,
            [FromBody] AccessAttributeTableModel model,
            CancellationToken cancellationToken
        )
        {
            await _accessAttributeTableService.UpdateAccessAttributeTable(
                id,
                model,
                cancellationToken
            );
        }

        // DELETE api/<AccessAttributeTablesController>/5
        [HttpDelete("{id}")]
        public async Task Delete(int id, CancellationToken cancellationToken)
        {
            await _accessAttributeTableService.DeleteAccessAttributeTable(id, cancellationToken);
        }
    }
}
