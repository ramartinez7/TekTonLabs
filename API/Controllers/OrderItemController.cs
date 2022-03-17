using API.Dto;
using Mediator.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public record OrderItemDto(int OrderId, int ProductId, double ProductQuantity);

        // POST api/<OrderItemController>
        [HttpPost]
        public async Task<OrderItem> Post([FromBody] OrderItemDto value)
        {
            try
            {
                var orderItem = await Mediator.Send(
                        new Mediator.Commands.OrderItem.AddOrderItemCommand(
                            new OrderItem
                            {
                                OrderId = value.OrderId,
                                ProductId = value.ProductId,
                                ProductQuantity = value.ProductQuantity
                            })
                        );

                return orderItem;
            }
            catch (DbUpdateException ex)
            {
                throw new HttpRequestException("Something ocurred at a database level", ex, System.Net.HttpStatusCode.InternalServerError);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // DELETE api/<OrderItemController>/5
        [HttpDelete("{id}")]
        [ResourceExists(typeof(OrderItem), typeof(int), "id")]
        public async Task<bool> Delete(int id)
        {
            var result = await Mediator.Send(new Mediator.Commands.OrderItem.DeleteOrderItemCommand(id));

            return result;
        }
    }
}
