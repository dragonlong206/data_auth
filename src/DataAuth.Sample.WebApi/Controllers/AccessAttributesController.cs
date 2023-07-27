using DataAuth.AccessAttributes;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DataAuth.Sample.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccessAttributesController : ControllerBase
    {
        IAccessAttributeService _accessAttributeService;

        public AccessAttributesController(IAccessAttributeService accessAttributeService)
        {
            _accessAttributeService = accessAttributeService;
        }

        // GET: api/<AccessAttributesController>
        [HttpGet]
        public async Task<IEnumerable<AccessAttributeModel>> Get()
        {
            return await _accessAttributeService.GetAccessAttributes();
        }

        // GET api/<AccessAttributesController>/5
        [HttpGet("{id}")]
        public async Task<AccessAttributeModel?> Get(int id)
        {
            return await _accessAttributeService.GetAccessAttributeById(id);
        }

        // POST api/<AccessAttributesController>
        [HttpPost]
        public async Task Post([FromBody] AccessAttributeModel model)
        {
            await _accessAttributeService.AddAccessAttribute(model);
        }

        // PUT api/<AccessAttributesController>/5
        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] AccessAttributeModel model)
        {
            await _accessAttributeService.UpdateAccessAttribute(id, model);
        }

        // DELETE api/<AccessAttributesController>/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _accessAttributeService.DeleteAccessAttribute(id);
        }
    }
}
