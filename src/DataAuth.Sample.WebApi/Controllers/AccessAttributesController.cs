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
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<AccessAttributesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<AccessAttributesController>
        [HttpPost]
        public async Task Post([FromBody] AccessAttributeModel model)
        {
            await _accessAttributeService.AddAccessAttribute(model);
        }

        // PUT api/<AccessAttributesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value) { }

        // DELETE api/<AccessAttributesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id) { }
    }
}
