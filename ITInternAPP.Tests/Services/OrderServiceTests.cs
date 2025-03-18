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

        var productName = "TestProduct";
        var amount = 1999;
        var customerType = CustomerType.Firma;
        var shippingAddress = "ExampleStreet 122";
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
    
    [Fact]
    public void MoveToWarehouse_ShouldChangeStatusToWarehouse_WhenOrderExists()
    {
        var mockRepo = new Mock<IOrderRepository>();
        var order = new Order 
        { 
            Id = 1, 
            Status = OrderStatus.Nowe,
            ShippingAddress = "TestStreet 123",
            Amount = 1000,
            PaymentMethod = PaymentMethod.Karta
        };
        mockRepo.Setup(r => r.GetOrderById(1)).Returns(order);

        var service = new OrderService(mockRepo.Object);

        service.MoveToWarehouse(1);

        Assert.Equal(OrderStatus.WMagazynie, order.Status);
        mockRepo.Verify(r => r.UpdateOrderStatus(1, OrderStatus.WMagazynie), Times.Once);
    }
  
    [Fact]
    public void MoveToWarehouse_ShouldSetStatusToError_WhenShippingAddressIsEmpty()
    {
        var mockRepo = new Mock<IOrderRepository>();
        var order = new Order
        {
            Id = 2,
            Status = OrderStatus.Nowe,
            ShippingAddress = "",
            Amount = 1000,
            PaymentMethod = PaymentMethod.Karta
        };
        mockRepo.Setup(r => r.GetOrderById(2)).Returns(order);

        var service = new OrderService(mockRepo.Object);

        service.MoveToWarehouse(2);

        Assert.Equal(OrderStatus.Blad, order.Status);
        mockRepo.Verify(r => r.UpdateOrderStatus(2, OrderStatus.Blad), Times.Once);
    }

    [Fact]
    public void MoveToWarehouse_ShouldSetStatusToReturned_WhenAmountIsHighAndCashOnDelivery()
    {
        var mockRepo = new Mock<IOrderRepository>();
        var order = new Order
        {
            Id = 3,
            Status = OrderStatus.Nowe,
            ShippingAddress = "TestStreet 123",
            Amount = 3000,
            PaymentMethod = PaymentMethod.GotowkaPrzyOdbiorze
        };
        mockRepo.Setup(r => r.GetOrderById(3)).Returns(order);

        var service = new OrderService(mockRepo.Object);

        service.MoveToWarehouse(3);

        Assert.Equal(OrderStatus.ZwroconoDoKlienta, order.Status);
        mockRepo.Verify(r => r.UpdateOrderStatus(3, OrderStatus.ZwroconoDoKlienta), Times.Once);
    }
    
    [Fact]
    public void MoveToWarehouse_ShouldNotChangeStatus_WhenAlreadyInWarehouse()
    {
        var mockRepo = new Mock<IOrderRepository>();
        var order = new Order
        {
            Id = 4,
            Status = OrderStatus.WMagazynie,
            ShippingAddress = "TestStreet 123",
            Amount = 3000,
            PaymentMethod = PaymentMethod.GotowkaPrzyOdbiorze
        };
        mockRepo.Setup(r => r.GetOrderById(4)).Returns(order);

        var service = new OrderService(mockRepo.Object);
        

        var consoleOutput = new StringWriter();
        Console.SetOut(consoleOutput);
        
        service.MoveToWarehouse(4);

        Assert.Contains("zamownienie juz jest w magazynie", consoleOutput.ToString());
    }

    [Fact]
    public void MoveToWarehouse_ShouldNotChangeStatus_WhenOrderIsClosed()
    {
        var mockRepo = new Mock<IOrderRepository>();
        var order = new Order
        {
            Id = 5,
            Status = OrderStatus.Zamkniete,
            ShippingAddress = "TestStreet 123",
            Amount = 1000,
            PaymentMethod = PaymentMethod.Karta
        };
        mockRepo.Setup(r => r.GetOrderById(5)).Returns(order);

        var service = new OrderService(mockRepo.Object);

        service.MoveToWarehouse(5);

        Assert.Equal(OrderStatus.Zamkniete, order.Status);
        mockRepo.Verify(r => r.UpdateOrderStatus(It.IsAny<int>(), It.IsAny<OrderStatus>()), Times.Never);
    }
    
    [Fact]
    public void MoveToShipping_ShouldChangeStatusToShippingAndClosed_WhenOrderInWarehouse()
    {
        var mockRepo = new Mock<IOrderRepository>();
        var order = new Order { Id = 1, Status = OrderStatus.WMagazynie };
        mockRepo.Setup(r => r.GetOrderById(1)).Returns(order);

        var service = new OrderService(mockRepo.Object);

        service.MoveToShipping(1);

        mockRepo.Verify(r => r.UpdateOrderStatus(1, OrderStatus.WWysylce), Times.Once);
        Thread.Sleep(5001);
        mockRepo.Verify(r => r.UpdateOrderStatus(1, OrderStatus.Zamkniete), Times.Once);
    }

    [Fact]
    public void MoveToShipping_ShouldNotProceed_WhenOrderNotInWarehouse()
    {
        var mockRepo = new Mock<IOrderRepository>();
        var order = new Order { Id = 1, Status = OrderStatus.Nowe };
        mockRepo.Setup(r => r.GetOrderById(1)).Returns(order);

        var service = new OrderService(mockRepo.Object);

        service.MoveToShipping(1);

        mockRepo.Verify(r => r.UpdateOrderStatus(It.IsAny<int>(), It.IsAny<OrderStatus>()), Times.Never);
    }

    [Fact]
    public void MoveToShipping_ShouldNotProceed_WhenOrderDoesNotExist()
    {
        var mockRepo = new Mock<IOrderRepository>();
        mockRepo.Setup(r => r.GetOrderById(1)).Returns((Order)null);

        var service = new OrderService(mockRepo.Object);

        service.MoveToShipping(1);

        mockRepo.Verify(r => r.UpdateOrderStatus(It.IsAny<int>(), It.IsAny<OrderStatus>()), Times.Never);
    }
    
    [Fact]
    public void DeleteOrder_ShouldCallDelete_WhenOrderExists()
    {
        var mockRepo = new Mock<IOrderRepository>();
        var order = new Order { Id = 1 };
        mockRepo.Setup(r => r.GetOrderById(1)).Returns(order);

        var service = new OrderService(mockRepo.Object);

        service.DeleteOrder(1);

        mockRepo.Verify(r => r.DeleteOrder(1), Times.Once);
    }

    [Fact]
    public void DeleteOrder_ShouldNotCallDelete_WhenOrderDoesNotExist()
    {
        var mockRepo = new Mock<IOrderRepository>();
        mockRepo.Setup(r => r.GetOrderById(1)).Returns((Order)null);

        var service = new OrderService(mockRepo.Object);

        service.DeleteOrder(1);

        mockRepo.Verify(r => r.DeleteOrder(It.IsAny<int>()), Times.Never);
    }

}