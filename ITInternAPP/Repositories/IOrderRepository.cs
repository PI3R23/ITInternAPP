using System.Collections.Generic;
using ITInternAPP.Models.Enums;

namespace ITInternAPP.Models.Repositories;

public interface IOrderRepository
{
    void AddOrder(Order order);
    List<Order> GetAllOrders();
    Order GetOrderById(int orderId);
    void UpdateOrderStatus(int orderId, OrderStatus newStatus);
    void DeleteOrder(int orderId);
}