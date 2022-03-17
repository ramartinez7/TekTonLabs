using MediatR;
using Microsoft.AspNetCore.Mvc;
using Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        public IMediator Mediator { get; }

        public ProductController(IMediator mediator)
        {
            Mediator = mediator;
        }        

        // GET: api/<ProductController>
        [HttpGet]
        public async Task<IEnumerable<Product>> Get()
        {
            return await Mediator.Send(new Mediator.Queries.Product.GetQuery());
        }

        // GET api/<ProductController>/5
        [HttpGet("{id}")]
        [ResourceExists(typeof(Product), typeof(int), "id")]
        public async Task<ActionResult<Product>> Get(int id)
        {
            var product = await Mediator.Send(new Mediator.Queries.Product.GetByIdQuery(id));

            if (product is null)
            {
                return NotFound();  
            }

            return Ok(product);
        }

        public record ProductDto(string Name, double Price);

        // POST api/<ProductController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ProductDto value)
        {
            var product = await Mediator.Send(new Mediator.Commands.Product.CreateCommand(new Product { Name = value.Name, Price = value.Price }));

            return CreatedAtAction(nameof(Get), product.Id, product);
        }

        // PUT api/<ProductController>/5
        [HttpPut("{id}")]
        [ResourceExists(typeof(Product), typeof(int), "id")]
        public async Task<ActionResult> Put(int id, [FromBody] Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            await Mediator.Send(new Mediator.Commands.Product.UpdateCommand(product));

            return NoContent();
        }
    }
}
