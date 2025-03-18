using System;
using ITInternAPP.Models.Enums;
using ITInternAPP.Services;

namespace ITInternAPP.UI;

public class OrderUI
{
    private readonly OrderService _orderService;

    public OrderUI(OrderService orderService)
    {
        _orderService = orderService;
    }

    public void ShowMenu()
    {
        while (true)
        {
            Console.WriteLine("\n MENU:");
            Console.WriteLine("> 1 - utworz nowe zamowienie");
            Console.WriteLine("> 2 - przekaz zamowienie do magazynu");
            Console.WriteLine("> 3 - przekaz zamoiwenie do wysylki");
            Console.WriteLine("> 4 - wyswietl wszystkie zamowienia");
            Console.WriteLine("> 5 - usun wybrane zamowienie");
            Console.WriteLine("> 6 - wyjscie");
            Console.WriteLine(" > wybierz opcje: <");
            switch (Console.ReadLine())
            {
                case "1":
                    CreateOrderUI();
                    break;
                case "2":
                    MoveToWarehouseUI();
                    break;
                case "3":
                    MoveToShippingUI();
                    break;
                case "4":
                    _orderService.ShowOrders();
                    break;
                case "5":
                    RemoveOrderUI();
                    break;
                case "6":
                    Console.WriteLine("Zamykanie aplikacji...");
                    return;
                default:
                    Console.WriteLine("Nieprawidłowa opcja. Sprobuj ponownie.");
                    break;
            }
        }
    }

    internal void CreateOrderUI()
    {
        Console.Write("\n Podaj nazwe produktu: ");
        string productName = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(productName))
        {
            Console.WriteLine(" Blad: niepoprawna nazwa.");
            return;
        }

        Console.Write("Podaj kwote zamowienia: ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal amount))
        {
            Console.WriteLine(" Blad: niepoprawna kwota.");
            return;
        }

        Console.Write("Klient (1 = Firma, 2 = Osoba fizyczna): ");
        if (!int.TryParse(Console.ReadLine(), out int clientTypeInput) || (clientTypeInput < 1 || clientTypeInput > 2))
        {
            Console.WriteLine("Blad: niepoprawny typ klienta.");
            return;
        }
        var customerType = (CustomerType)(clientTypeInput - 1);

        Console.Write("Podaj adres dostawy: ");
        string shippingAddress = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(shippingAddress))
        {
            Console.WriteLine(" Blad: niepoprawny adres dostawy.");
            return;
        }

        Console.Write("Metoda platnosci (1 = Karta, 2 = Gotowka przy odbiorze): ");
        if (!int.TryParse(Console.ReadLine(), out int paymentMethodInput) || (paymentMethodInput < 1 || paymentMethodInput > 2))
        {
            Console.WriteLine(" Blad: niepoprawna metoda platnosci.");
            return;
        }
        var paymentMethod = (PaymentMethod)(paymentMethodInput - 1);

        _orderService.CreateOrder(productName, amount, customerType, shippingAddress, paymentMethod);
        Console.WriteLine("Zamowienie zostalo utworzone.");
    }
    internal void MoveToWarehouseUI()
    {
        Console.Write("\n Podaj ID zamowienia do magazynu: ");
        if (!int.TryParse(Console.ReadLine(), out int orderId))
        {
            Console.WriteLine(" Blad: niepoprawny numer ID.");
            return;
        }

        _orderService.MoveToWarehouse(orderId);
    }

    internal void MoveToShippingUI()
    {
        Console.Write("\nPodaj ID zamowienia do wysylki: ");
        if (!int.TryParse(Console.ReadLine(), out int orderId))
        {
            Console.WriteLine("Blad: niepoprawny numer ID.");
            return;
        }

        _orderService.MoveToShipping(orderId);
    }
    
    internal void RemoveOrderUI()
    {
        Console.Write("\n Podaj ID zamowienia do usuniecia: ");
        if (!int.TryParse(Console.ReadLine(), out int orderId))
        {
            Console.WriteLine("Blad: niepoprawny numer ID.");
            return;
        }
        _orderService.DeleteOrder(orderId);
    }
}