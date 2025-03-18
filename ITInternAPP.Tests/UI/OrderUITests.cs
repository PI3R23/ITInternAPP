using ITInternAPP.Models.Repositories;
using ITInternAPP.Services;
using ITInternAPP.UI;
using Moq;

namespace ITInternAPP.Tests.UI;

public class OrderUITests
{
    [Fact]
    public void ShowMenu_ShouldDisplayError_WhenOptionInMenuIsInvalid()
    {
        var input = new StringReader("invalid_option\n6\n"); //must be 6 to end while loop
        Console.SetIn(input);

        var output = new StringWriter();
        Console.SetOut(output);

        var mockRepo = new Mock<IOrderRepository>();
        var orderService = new OrderService(mockRepo.Object);
        var ui = new OrderUI(orderService);

        ui.ShowMenu();

        var consoleOutput = output.ToString();
        Assert.Contains("Nieprawidłowa opcja. Sprobuj ponownie", consoleOutput);
    }
    
    [Fact]
    public void CreateOrderUI_ShouldDisplayError_WhenProductNameIsEmpty()
    {
        var input = new StringReader("\n");
        Console.SetIn(input);

        var output = new StringWriter();
        Console.SetOut(output);

        var mockRepo = new Mock<IOrderRepository>();
        var orderService = new OrderService(mockRepo.Object);
        var ui = new OrderUI(orderService);

        ui.CreateOrderUI();
        
        var consoleOutput = output.ToString();
        Assert.Contains("Blad: niepoprawna nazwa", consoleOutput);
    }
    
    [Fact]
    public void CreateOrderUI_ShouldDisplayError_WhenProductsAmountIsInvalid()
    {
        var mockRepo = new Mock<IOrderRepository>();
        var orderService = new OrderService(mockRepo.Object);
        var ui = new OrderUI(orderService);

        var output = new StringWriter();
        Console.SetOut(output);
        
        var input = new StringReader("ExampleName\nnot_a_number_for_sure\n");
        Console.SetIn(input);

        ui.CreateOrderUI();

        var consoleOutput = output.ToString();
        Assert.Contains("Blad: niepoprawna kwota", consoleOutput);
    }
    
    [Fact]
    public void CreateOrderUI_ShouldDisplayError_WhenClientTypeIsInvalid()
    {
        var mockRepo = new Mock<IOrderRepository>();
        var orderService = new OrderService(mockRepo.Object);
        var ui = new OrderUI(orderService);

        var output = new StringWriter();
        Console.SetOut(output);

        var input = new StringReader("ExampleName\n100,90\n3\n");
        Console.SetIn(input);
        
        ui.CreateOrderUI();
        
        var consoleOutput = output.ToString();
        Assert.Contains("Blad: niepoprawny typ klienta", consoleOutput);
    }
    
    [Fact]
    public void CreateOrderUI_ShouldDisplayError_WhenShippingAddressIsInvalid()
    {
        var mockRepo = new Mock<IOrderRepository>();
        var orderService = new OrderService(mockRepo.Object);
        var ui = new OrderUI(orderService);

        var output = new StringWriter();
        Console.SetOut(output);

        var input = new StringReader("ExampleName\n100,90\n1\n\n");
        Console.SetIn(input);
        
        ui.CreateOrderUI();
        
        var consoleOutput = output.ToString();
        Assert.Contains("Blad: niepoprawny adres dostawy", consoleOutput);
    }
    
    [Fact]
    public void CreateOrderUI_ShouldDisplayError_WhenPaymentTypeIsInvalid()
    {
        var mockRepo = new Mock<IOrderRepository>();
        var orderService = new OrderService(mockRepo.Object);
        var ui = new OrderUI(orderService);

        var output = new StringWriter();
        Console.SetOut(output);

        var input = new StringReader("ExampleName\n100,90\n1\nExStreet\n3");
        Console.SetIn(input);
        
        ui.CreateOrderUI();
        
        var consoleOutput = output.ToString();
        Assert.Contains("Blad: niepoprawna metoda platnosci", consoleOutput);
    }
    
    [Fact]
    public void MoveToWarehouseUI_ShouldDisplayError_WhenIdIsInvalid()
    {
        var input = new StringReader("100_percent_invalid_id");
        Console.SetIn(input);

        var output = new StringWriter();
        Console.SetOut(output);

        var mockRepo = new Mock<IOrderRepository>();
        var orderService = new OrderService(mockRepo.Object);
        var ui = new OrderUI(orderService);

        ui.MoveToWarehouseUI();

        var consoleOutput = output.ToString();
        Assert.Contains("Blad: niepoprawny numer ID", consoleOutput);
    }
    
    [Fact]
    public void MoveToShippingUI_ShouldDisplayError_WhenIdIsInvalid()
    {
        var input = new StringReader("100_percent_invalid_id");
        Console.SetIn(input);

        var output = new StringWriter();
        Console.SetOut(output);

        var mockRepo = new Mock<IOrderRepository>();
        var orderService = new OrderService(mockRepo.Object);
        var ui = new OrderUI(orderService);

        ui.MoveToShippingUI();

        var consoleOutput = output.ToString();
        Assert.Contains("Blad: niepoprawny numer ID", consoleOutput);
    }
    
    [Fact]
    public void RemoveOrderUI_ShouldDisplayError_WhenIdIsInvalid()
    {
        var input = new StringReader("100_percent_invalid_id");
        Console.SetIn(input);

        var output = new StringWriter();
        Console.SetOut(output);

        var mockRepo = new Mock<IOrderRepository>();
        var orderService = new OrderService(mockRepo.Object);
        var ui = new OrderUI(orderService);

        ui.RemoveOrderUI();

        var consoleOutput = output.ToString();
        Assert.Contains("Blad: niepoprawny numer ID", consoleOutput);
    }
}