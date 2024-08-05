using System.Linq.Expressions;
using Moq;
using NUnit.Framework;
using RetailECommercePlatform.Data.Enum;
using RetailECommercePlatform.Data.RequestModels.Query.Order;
using RetailECommercePlatform.Repository.Entities;
using RetailECommercePlatform.Repository.Repositories.Interfaces;
using RetailECommercePlatform.Service.QueryHandlers.Order;

namespace RetailECommercePlatform.Tests.QueryHandlers.Order;

[TestFixture]
    public class GetOrderByIdQueryHandlerTests
    {
        private Mock<IOrderRepository> _mockOrderRepository;
        private Mock<ICustomerRepository> _mockCustomerRepository;
        private GetOrderByIdQueryHandler _handler;

        [SetUp]
        public void Setup()
        {
            _mockOrderRepository = new Mock<IOrderRepository>();
            _mockCustomerRepository = new Mock<ICustomerRepository>();
            _handler = new GetOrderByIdQueryHandler(_mockOrderRepository.Object, _mockCustomerRepository.Object);
        }

        [Test]
        public async Task Handle_ValidRequest_ReturnsCorrectResponse()
        {
            // Arrange
            var orderId = "23432532421";
            var customerId = "32521412412";
            var order = new Repository.Entities.Order
            {
                Id = orderId,
                CustomerId = customerId,
                IsActive = true,
                TotalPrice = 100,
                State = (int)OrderState.OrderReceived,
                OrderItems = new List<Cart>
                {
                    new Cart()
                    {
                        Product = new Repository.Entities.Product
                        {
                            Code = "P1",
                            Name = "Product 1",
                            Price = 50
                        }
                    },
                    new Cart()
                    {
                        Product = new Repository.Entities.Product
                        {
                            Code = "P2",
                            Name = "Product 2",
                            Price = 50
                        }
                    }
                }
            };
            var customer = new Customer
            {
                Id = customerId,
                Name = "John",
                Surname = "Doe",
                Address = "123 Test St",
                IsActive = true
            };

            _mockOrderRepository.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Repository.Entities.Order, bool>>>()))
                .ReturnsAsync(order);
            _mockCustomerRepository.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Customer, bool>>>()))
                .ReturnsAsync(customer);

            var query = new GetOrderByIdQuery { Id = orderId };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(orderId, Is.EqualTo(result.Id));
            Assert.That(result.TotalPrice, Is.EqualTo(100));
            Assert.That(result.Products.Count, Is.EqualTo(2));
        }

        [Test]
        public void Handle_OrderNotFound_ThrowsArgumentNullException()
        {
            // Arrange
            _mockOrderRepository.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Repository.Entities.Order, bool>>>()))
                .ReturnsAsync((Repository.Entities.Order)null);

            var query = new GetOrderByIdQuery { Id = "23432532421" };

            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(() => _handler.Handle(query, CancellationToken.None));
        }

        [Test]
        public void Handle_CustomerNotFound_ThrowsArgumentNullException()
        {
            // Arrange
            var order = new Repository.Entities.Order { Id = "23432532421", CustomerId = "32521412412", IsActive = true };
            _mockOrderRepository.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Repository.Entities.Order, bool>>>()))
                .ReturnsAsync(order);
            _mockCustomerRepository.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Customer, bool>>>()))
                .ReturnsAsync((Customer)null);

            var query = new GetOrderByIdQuery { Id = "23432532421" };

            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(() => _handler.Handle(query, CancellationToken.None));
        }
    }