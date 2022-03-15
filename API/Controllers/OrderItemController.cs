using API.Dto;
using Mediator.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
        public IMediator Mediator { get; }

        public OrderItemController(IMediator mediator)
        {
            Mediator = mediator;
        } 

        // GET: api/<OrderItemController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<OrderItemController>/5
        [HttpGet("{id}")]
        public async Task<GetOrderItemsByOrderIdDto> Get(int id)
        {
            (Order order, List<Product> items) = await Mediator.Send(new GetOrderItemsByOrderIdQuery(id));
            
            var result = new GetOrderItemsByOrderIdDto
            {
                Order = order,
                Items = items
            };

            return result;
        }

        // POST api/<OrderItemController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<OrderItemController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<OrderItemController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
