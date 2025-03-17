using Xunit;
using Moq;
using ITInternAPP.Models;
using ITInternAPP.Models.Enums;
using ITInternAPP.Models.Repositories;
using ITInternAPP.Services;

public class OrderServiceTests
{
    [Fact]
    public void CreateOrder_ShouldReturnOrder_WithCorrectData()
    {
        var mockRepo = new Mock<IOrderRepository>();
        var service = new OrderService(mockRepo.Object);

        var productName = "ProduktTestowy";
        var amount = 1999m;
        var customerType = CustomerType.Firma;
        var shippingAddress = "Testowa 123";
        var paymentMethod = PaymentMethod.Karta;

        var order = service.CreateOrder(productName, amount, customerType, shippingAddress, paymentMethod);

        Assert.NotNull(order);
        Assert.Equal(productName, order.ProductName);
        Assert.Equal(amount, order.Amount);
        Assert.Equal(customerType, order.CustomerType);
        Assert.Equal(shippingAddress, order.ShippingAddress);
        Assert.Equal(paymentMethod, order.PaymentMethod);
        Assert.Equal(OrderStatus.Nowe, order.Status);

        mockRepo.Verify(r => r.AddOrder(It.IsAny<Order>()), Times.Once);
    }
}