﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        public IMediator Mediator { get; }

        public OrderController(IMediator mediator)
        {
            Mediator = mediator;
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

            return Ok(new { order, items });
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

        // POST api/<OrderController>
        [HttpPost]
        public async Task<ActionResult<Order>> Post([FromBody] DateTime date)
        {
            var order = await Mediator.Send(new Mediator.Commands.Order.CreateCommand(new Order { Date = date }));

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