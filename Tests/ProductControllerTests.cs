using API.Controllers;
using MediatR;
using Models;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class ProductControllerTests
    {
        public API.Controllers.ProductController Sut;
        public Mock<IMediator> MockMediator;
        public CancellationToken Token;

        public ProductControllerTests()
        {
            MockMediator = new Mock<IMediator>();
            Token = new();
            Sut = new ProductController(MockMediator.Object);
        }

        [Fact]
        public async Task Get_ShouldReturnList()
        {
            //arrange
            var query = new Mediator.Queries.Product.GetQuery();
            var expected = new List<Product>() { };
            MockMediator.Setup(m => m.Send(query, Token)).ReturnsAsync(expected);

            //act
            var actual = await Sut.Get(); 

            //assert
            Assert.NotNull(actual);
            Assert.IsType<List<Product>>(actual);
            Assert.Same(expected, actual);
        }
    }
}