using API;
using API.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Tests.MockData;
using Xunit;

namespace Tests
{
    public class ProductControllerTests : IClassFixture<ProductData>
    {
        public API.Controllers.ProductController Sut;
        public Mock<IMediator> MockMediator;
        public CancellationToken Token;

        public ProductControllerTests(ProductData data)
        {
            MockMediator = new Mock<IMediator>();
            Token = new();
            Sut = new ProductController(MockMediator.Object);
            Data = data;
        }

        public ProductData Data { get; }

        [Fact]
        public async Task Get_ShouldReturnList()
        {
            //arrange
            var query = new Mediator.Queries.Product.GetQuery();
            MockMediator
                .Setup(m => m.Send(query, Token))
                .ReturnsAsync(Data.ProductList)
                .Verifiable();

            //act
            var actual = await Sut.Get(); 

            //assert
            Assert.NotNull(actual);
            Assert.IsType<List<Product>>(actual);
            Assert.Same(Data.ProductList, actual);
            MockMediator.Verify();
        }

        [Fact]
        public async Task Get_ShouldReturnException()
        {
            //arrange
            var query = new Mediator.Queries.Product.GetQuery();
            MockMediator
                .Setup(m => m.Send(query, Token))
                .Throws<Exception>();

            //act
            var ex = await Record.ExceptionAsync(() => Sut.Get());

            //assert
            Assert.NotNull(ex);
            Assert.IsType<Exception>(ex);
        }

        [Fact]
        public async Task GetById_ShouldReturnOneProduct()
        {
            //arrange
            var query = new Mediator.Queries.Product.GetByIdQuery(Data.ID);
            
            MockMediator
                .Setup(m => m.Send(query, Token))
                .ReturnsAsync(Data.Product)
                .Verifiable();

            //act
            var actual = (await Sut.GetById(Data.ID)).Result as OkObjectResult;

            //assert
            Assert.NotNull(actual);
            Assert.Same(Data.Product, actual.Value);
            MockMediator.Verify();
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound()
        {
            //arrange
            var query = new Mediator.Queries.Product.GetByIdQuery(Data.NOT_EXISTS_ID);

            MockMediator
                .Setup(m => m.Send(query, Token))
                .ReturnsAsync((Product)null)
                .Verifiable();

            //act
            var actual = (await Sut.GetById(Data.NOT_EXISTS_ID)).Result as NotFoundResult;

            //assert
            Assert.NotNull(actual);
            MockMediator.Verify();
        }

        [Fact]
        public void GetById_ShouldBeDecoratedWithResourceExistsAttribute()
        {
            var type = Sut.GetType();
            var methodInfo = type.GetMethod("GetById");
            var attributes = methodInfo.GetCustomAttributes(typeof(ResourceExistsAttribute), true);
            Assert.True(attributes.Any());
        }

        [Fact]
        public async Task Post_ShouldSuccess()
        {
            //arrange
            MockMediator
                .Setup(m => m.Send(It.IsAny<Mediator.Commands.Product.CreateCommand>(), Token))
                .ReturnsAsync(Data.Product)
                .Verifiable();

            //act
            var actual = (await Sut.Post(Data.ProductDto)) as CreatedAtActionResult;

            //assert
            Assert.NotNull(actual);
            Assert.Same(Data.Product, actual.Value);
            MockMediator.Verify();
        }

        [Fact]
        public async Task Put_ShouldSuccess()
        {
            //arrange
            MockMediator
                .Setup(m => m.Send(It.IsAny<Mediator.Commands.Product.UpdateCommand>(), Token))
                .ReturnsAsync(Data.Product)
                .Verifiable();

            //act
            var actual = (await Sut.Put(Data.Product.Id, Data.Product)) as NoContentResult;

            //assert
            Assert.NotNull(actual);
            MockMediator.Verify();
        }

        [Fact]
        public void Put_ShouldBeDecoratedWithResourceExistsAttribute()
        {
            var type = Sut.GetType();
            var methodInfo = type.GetMethod("Put");
            var attributes = methodInfo.GetCustomAttributes(typeof(ResourceExistsAttribute), true);
            Assert.True(attributes.Any());
        }
    }
}