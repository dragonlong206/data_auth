using DataAuth.Core;
using DataAuth.Sample.WebApi.Entities;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DataAuth.Sample.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        ApplicationDbContext _dbContext;

        public OrdersController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/<OrdersController>
        [HttpGet]
        public async Task<IEnumerable<Order>> Get()
        {
#pragma warning disable CS8629 // Nullable value type may be null.
            var orders = await _dbContext.Orders.WithDataAuthAsync<Order, int>(
                "2",
                "STORE",
                x => x.StoreId.Value
            );
#pragma warning restore CS8629 // Nullable value type may be null.
            return orders;
        }

        // GET api/<OrdersController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<OrdersController>
        [HttpPost]
        public void Post([FromBody] string value) { }

        // PUT api/<OrdersController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value) { }

        // DELETE api/<OrdersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id) { }
    }
}
