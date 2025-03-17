
using System;
using ITInternAPP.Models.Repositories;
using ITInternAPP.Services;
using ITInternAPP.UI;

class Program
{
    static void Main()
    {
        Console.WriteLine("Uruchamianie systemu zarzadzania zamowieniami...");
        
        IOrderRepository orderRepository = new OrderRepository();
        OrderService orderService = new OrderService(orderRepository);
        OrderUI ui = new OrderUI(orderService);

        ui.ShowMenu();
    }
}

