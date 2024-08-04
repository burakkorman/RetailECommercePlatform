using System.Linq.Expressions;
using Moq;
using NUnit.Framework;
using RetailECommercePlatform.Data.Contract;
using RetailECommercePlatform.Data.Errors;
using RetailECommercePlatform.Data.RequestModels.Command.Order;
using RetailECommercePlatform.Repository.Entities;
using RetailECommercePlatform.Repository.Repositories.Interfaces;
using RetailECommercePlatform.Service.CommandHandlers.Order;
using RetailECommercePlatform.Service.Services.Token;

namespace RetailECommercePlatform.Tests.CommandHandlers.Order;

[TestFixture]
public class CreateOrderCommandHandlerTests
{
    private Mock<IOrderRepository> _orderRepositoryMock;
    private Mock<ICartRepository> _cartRepositoryMock;
    private Mock<IProductRepository> _productRepositoryMock;
    private Mock<ITokenService> _tokenServiceMock;
    private CreateOrderCommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _cartRepositoryMock = new Mock<ICartRepository>();
        _productRepositoryMock = new Mock<IProductRepository>();
        _tokenServiceMock = new Mock<ITokenService>();

        _handler = new CreateOrderCommandHandler(
            _orderRepositoryMock.Object,
            _cartRepositoryMock.Object,
            _productRepositoryMock.Object,
            _tokenServiceMock.Object
        );
    }

    [Test]
    public async Task Handle_ThrowsException_WhenCartListIsEmpty()
    {
        // Arrange
        var request = new CreateOrderCommand();
        var currentUser = new CurrentUserDto() { Id = "12234123423", Name = "Name", Surname = "Surname" };

        _tokenServiceMock.Setup(ts => ts.Me()).Returns(currentUser);
        _cartRepositoryMock.Setup(repo => repo.FilterAsync(It.IsAny<Expression<Func<Cart, bool>>>()))
            .ReturnsAsync(new List<Cart>());

        // Act & Assert
        Assert.ThrowsAsync<RetailECommerceApiException>(() => _handler.Handle(request, CancellationToken.None));
    }

    [Test]
    public async Task Handle_ThrowsException_WhenProductNotFound()
    {
        // Arrange
        var request = new CreateOrderCommand();
        var currentUser = new CurrentUserDto() { Id = "12234123423", Name = "Name", Surname = "Surname" };
        var cartList = new List<Cart>
        {
            new Cart { ProductId = "864328328745823", Quantity = 1, IsActive = true }
        };

        _tokenServiceMock.Setup(ts => ts.Me()).Returns(currentUser);
        _cartRepositoryMock.Setup(repo => repo.FilterAsync(It.IsAny<Expression<Func<Cart, bool>>>()))
            .ReturnsAsync(cartList);
        _productRepositoryMock.Setup(repo => repo.FilterAsync(It.IsAny<Expression<Func<Product, bool>>>()))
            .ReturnsAsync([]);

        // Act & Assert
        Assert.ThrowsAsync<RetailECommerceApiException>(() => _handler.Handle(request, CancellationToken.None));
    }

    [Test]
    public async Task Handle_ThrowsException_WhenNotEnoughStock()
    {
        // Arrange
        var request = new CreateOrderCommand();
        var currentUser = new CurrentUserDto() { Id = "12234123423", Name = "Name", Surname = "Surname" };
        var cartList = new List<Cart>
        {
            new Cart { ProductId = "864328328745823", Quantity = 10, IsActive = true }
        };
        var products = new List<Product>
        {
            new Product { Id = "864328328745823", Name = "Product 1", StockCount = 5, IsActive = true }
        };

        _tokenServiceMock.Setup(ts => ts.Me()).Returns(currentUser);
        _cartRepositoryMock.Setup(repo => repo.FilterAsync(It.IsAny<Expression<Func<Cart, bool>>>()))
            .ReturnsAsync(cartList);
        _productRepositoryMock.Setup(repo => repo.FilterAsync(It.IsAny<Expression<Func<Product, bool>>>()))
            .ReturnsAsync(products);

        // Act & Assert
        Assert.ThrowsAsync<RetailECommerceApiException>(() => _handler.Handle(request, CancellationToken.None));
    }

    [Test]
    public async Task Handle_SuccessfullyCreatesOrder()
    {
        // Arrange
        var request = new CreateOrderCommand();
        var currentUser = new CurrentUserDto() { Id = "12234123423", Name = "Name", Surname = "Surname" };
        var cartList = new List<Cart>
        {
            new Cart
            {
                ProductId = "864328328745823", Quantity = 1, IsActive = true,
                Product = new Product { Id = "prod1", Name = "Product 1", Price = (decimal)99.99 }
            }
        };
        var products = new List<Product>
        {
            new Product { Id = "864328328745823", Name = "Product 1", StockCount = 5, IsActive = true }
        };

        _tokenServiceMock.Setup(ts => ts.Me()).Returns(currentUser);
        _cartRepositoryMock.Setup(repo => repo.FilterAsync(It.IsAny<Expression<Func<Cart, bool>>>()))
            .ReturnsAsync(cartList);
        _productRepositoryMock.Setup(repo => repo.FilterAsync(It.IsAny<Expression<Func<Product, bool>>>()))
            .ReturnsAsync(products);
        // _orderRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Repository.Entities.Order>()))
        //     .Returns((Task<Repository.Entities.Order>)Task.CompletedTask);
        // _productRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<string>(), It.IsAny<Product>()))
        //     .Returns((Task<Product>)Task.CompletedTask);
        // _cartRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<string>(), It.IsAny<Cart>()))
        //     .Returns((Task<Cart>)Task.CompletedTask);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.That(result, Is.True);
        _orderRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Repository.Entities.Order>()), Times.Once);
        _productRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<string>(), It.IsAny<Product>()), Times.Once);
        _cartRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<string>(), It.IsAny<Cart>()), Times.Once);
    }
}