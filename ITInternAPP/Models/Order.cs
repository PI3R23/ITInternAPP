using ITInternAPP.Models.Enums;

namespace ITInternAPP.Models;

public class Order
{
    public int Id { get; set; }
    public string ProductName { get; set; }
    public decimal Amount { get; set; }
    public CustomerType CustomerType { get; set; }
    public string ShippingAddress { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public OrderStatus Status { get; set; }
}