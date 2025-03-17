using System;
using System.Threading;
using ITInternAPP.Models;
using ITInternAPP.Models.Enums;
using ITInternAPP.Models.Repositories;

namespace ITInternAPP.Services;

public class OrderService
{
    private readonly IOrderRepository _orderRepository;

    public OrderService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public Order CreateOrder(string productName, decimal amount, CustomerType customerType, string shippingAddress,
        PaymentMethod paymentMethod)
    {
        var order = new Order
        {
            ProductName = productName,
            Amount = amount,
            CustomerType = customerType,
            ShippingAddress = shippingAddress,
            PaymentMethod = paymentMethod,
            Status = OrderStatus.Nowe
        };
        _orderRepository.AddOrder(order);
        return order;
    }
    public void MoveToWarehouse(int orderId)
    {
        var order = _orderRepository.GetOrderById(orderId);
        if (order == null)
        {
            Console.WriteLine("zamowienie nie istnieje.");
            return;
        }

        if (order.Status == OrderStatus.WMagazynie)
        {
            Console.WriteLine("zamownienie juz jest w magazynie");
            return;
        }

        if (order.Status == OrderStatus.Zamkniete || order.Status == OrderStatus.ZwroconoDoKlienta 
                                                  || order.Status == OrderStatus.Blad)
        {
            Console.WriteLine("zamownienie juz jest zamkniete");
            return;
        }

        if (order.Amount >= 2500 && order.PaymentMethod == PaymentMethod.GotowkaPrzyOdbiorze)
        {
            order.Status = OrderStatus.ZwroconoDoKlienta;
            Console.WriteLine("zamowienie zostalo zwrocone do klienta.");
        }
        else if (string.IsNullOrEmpty(order.ShippingAddress))
        {
            order.Status = OrderStatus.Blad;
            Console.WriteLine("blad w zamowieniu: brak adresu wysylki");
        }
        else
        {
            order.Status = OrderStatus.WMagazynie;
            Console.WriteLine("zamowienie przeslane do magazynu");
        }
        _orderRepository.UpdateOrderStatus(orderId, order.Status);
    }

    public void MoveToShipping(int orderId)
    {
        var order = _orderRepository.GetOrderById(orderId);
        if (order == null)
        {
            Console.WriteLine("zamowienie nie istnieje.");
            return;
        }

        if (order.Status != OrderStatus.WMagazynie)
        {
            Console.WriteLine("zamowienie powinno byc w magazynie zamin zostanie przekazane do wysylki");
            return;
        }
        if (order.Status == OrderStatus.WWysylce)
        {
            Console.WriteLine("zamownienie juz jest w Wysylce");
            return;
        }

        order.Status = OrderStatus.WWysylce;
        _orderRepository.UpdateOrderStatus(orderId, order.Status);
        Console.WriteLine("zamowienie przeslane do wysylki...");
        
        Thread.Sleep(new Random().Next(1000, 5001));
        order.Status = OrderStatus.Zamkniete;
        _orderRepository.UpdateOrderStatus(orderId, order.Status);
        Console.WriteLine("zamowienie zostalo wyslane oraz zamkniete");
    }

    public void ShowOrders()
    {
        var orders = _orderRepository.GetAllOrders();
        if (orders.Count==0)
        {
            Console.WriteLine("brak zamowien");
            return;
        }

        Console.WriteLine("\n lista zamowien:");
        foreach (var order in orders)
        {
            Console.WriteLine($"ID: {order.Id}, Produkt: {order.ProductName}, Status: {order.Status}, " +
                              $"Kwota: {order.Amount}, Klient: {order.CustomerType}, Płatność: {order.PaymentMethod}");
        }
    }

    public void DeleteOrder(int orderId)
    {
        var order = _orderRepository.GetOrderById(orderId);
        if (order == null )
        {
            Console.WriteLine("zamowienie nie istnieje.");
            return;
        }
        _orderRepository.DeleteOrder(orderId);
        Console.WriteLine($"Zamówienie o ID {orderId} zostało usunięte.");
    }
}