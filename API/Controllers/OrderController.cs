using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        public IMediator Mediator { get; }
        public IMemoryCache Cache { get; }

        public OrderController(IMediator mediator, IMemoryCache cache)
        {
            Mediator = mediator;
            Cache = cache;
        }

        // GET: api/order
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var orders = await Mediator.Send(new Mediator.Queries.Order.GetQuery());
            return Ok(orders.Select(o => new { o.Id, o.Date }).ToList());
        }

        // GET api/order/5
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var order = await Mediator.Send(new Mediator.Queries.Order.GetByIdQuery(id));

            if (order is null)
            {
                return NotFound();
            }

            var items = order.Items?.Select(i => i.Product).ToList();

            object additionalData = new();

            if (Cache.TryGetValue("CompanyName", out string CompanyName) && Cache.TryGetValue("CompanyAddress", out string CompanyAddress))
            {
                additionalData = new { CompanyName, CompanyAddress };
            }

            return Ok(new { order, additionalData, items });
        }

        // GET api/<OrderController>/5/product
        [HttpGet("{id}/product")]
        public async Task<ActionResult> GetProductsById(int id)
        {
            var order = await Mediator.Send(new Mediator.Queries.Order.GetByIdQuery(id));

            if (order is null)
            {
                return NotFound();
            }

            var items = order.Items?.Select(i => i.Product).ToList();

            return Ok(items);
        }

        public record OrderDto(DateTime Date);

        // POST api/<OrderController>
        [HttpPost]
        public async Task<ActionResult<Order>> Post([FromBody] OrderDto value)
        {
            var order = await Mediator.Send(new Mediator.Commands.Order.CreateCommand(new Order { Date = value.Date }));

            return CreatedAtAction(nameof(Get), new { id = order.Id }, order);
        }

        // PUT api/<OrderController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Order>> Put(int id, [FromBody] Order order)
        {
            if (id != order.Id)
            {
                return BadRequest();
            }

            await Mediator.Send(new Mediator.Commands.Order.UpdateCommand(order));

            return NoContent();
        }
    }
}
