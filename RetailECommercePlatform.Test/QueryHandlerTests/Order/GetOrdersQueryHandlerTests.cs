using System.Linq.Expressions;
using Moq;
using NUnit.Framework;
using RetailECommercePlatform.Data.Contract;
using RetailECommercePlatform.Data.Enum;
using RetailECommercePlatform.Data.RequestModels.Query.Order;
using RetailECommercePlatform.Repository.Entities;
using RetailECommercePlatform.Repository.Repositories.Interfaces;
using RetailECommercePlatform.Service.QueryHandlers.Order;
using RetailECommercePlatform.Service.Services.Token;

namespace RetailECommercePlatform.Tests.QueryHandlers.Order;

[TestFixture]
public class GetOrdersQueryHandlerTests
{
    private Mock<IOrderRepository> _orderRepositoryMock;
    private Mock<ICustomerRepository> _customerRepositoryMock;
    private Mock<ITokenService> _tokenServiceMock;
    private GetOrdersQueryHandler _handler;

    [SetUp]
    public void Setup()
    {
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _customerRepositoryMock = new Mock<ICustomerRepository>();
        _tokenServiceMock = new Mock<ITokenService>();

        _handler = new GetOrdersQueryHandler(
            _orderRepositoryMock.Object,
            _customerRepositoryMock.Object,
            _tokenServiceMock.Object
        );
    }

    [Test]
    public async Task Handle_ReturnsOrderList_WhenOrdersExist()
    {
        // Arrange
        var request = new GetOrdersQuery();
        var currentUser = new CurrentUserDto() { Id = "12234123423", Name = "Name", Surname = "Surname" };
        var orders = new List<Repository.Entities.Order>
        {
            new Repository.Entities.Order
            {
                Id = "37825732482389",
                CustomerId = "12234123423",
                IsActive = true,
                State = (int)OrderState.OrderReceived,
                OrderItems = new List<Cart>
                {
                    new Cart() { Product = new Product { Code = "P1", Name = "Product 1", Price = (decimal)99.99 } }
                },
                TotalPrice = (decimal)99.99
            }
        };
        var customers = new List<Customer>
        {
            new Customer
            {
                Id = "12234123423", Name = "Name", Surname = "Surname", Address = "123 Main St",
                IsActive = true
            }
        };

        _tokenServiceMock.Setup(ts => ts.Me()).Returns(currentUser);
        _orderRepositoryMock
            .Setup(repo => repo.FilterAsync(It.IsAny<Expression<Func<Repository.Entities.Order, bool>>>()))
            .ReturnsAsync(orders);
        _customerRepositoryMock.Setup(repo => repo.FilterAsync(It.IsAny<Expression<Func<Customer, bool>>>()))
            .ReturnsAsync(customers);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(1));
        Assert.That(result[0].TotalPrice, Is.EqualTo(99.99));
    }

    [Test]
    public async Task Handle_ReturnsEmptyList_WhenNoOrdersExist()
    {
        // Arrange
        var request = new GetOrdersQuery();
        var currentUser = new CurrentUserDto() { Id = "12234123423", Name = "Name", Surname = "Surname" };
        var orders = new List<Repository.Entities.Order>();
        var customers = new List<Customer>();

        _tokenServiceMock.Setup(ts => ts.Me()).Returns(currentUser);
        _orderRepositoryMock
            .Setup(repo => repo.FilterAsync(It.IsAny<Expression<Func<Repository.Entities.Order, bool>>>()))
            .ReturnsAsync(orders);
        _customerRepositoryMock.Setup(repo => repo.FilterAsync(It.IsAny<Expression<Func<Customer, bool>>>()))
            .ReturnsAsync(customers);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(0));
    }

    [Test]
    public void Handle_ThrowsArgumentNullException_WhenOrdersIsNull()
    {
        // Arrange
        var request = new GetOrdersQuery();
        var currentUser = new CurrentUserDto() { Id = "12234123423", Name = "Name", Surname = "Surname" };

        _tokenServiceMock.Setup(ts => ts.Me()).Returns(currentUser);
        _orderRepositoryMock
            .Setup(repo => repo.FilterAsync(It.IsAny<Expression<Func<Repository.Entities.Order, bool>>>()))
            .ReturnsAsync((List<Repository.Entities.Order>)null);

        // Act & Assert
        Assert.ThrowsAsync<ArgumentNullException>(
            async () => await _handler.Handle(request, CancellationToken.None));
    }
}