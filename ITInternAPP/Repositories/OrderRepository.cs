using System;
using System.Collections.Generic;
using System.Data.SQLite;
using ITInternAPP.Data;
using ITInternAPP.Models.Enums;

namespace ITInternAPP.Models.Repositories;

public class OrderRepository:IOrderRepository
{
    public OrderRepository()
    {
        Database.Initialize();
    }
    public void AddOrder(Order order)
    {
        using (var connection = Database.GetConnection())
        {
            connection.Open();
            string query = @"
            INSERT INTO Orders (ProductName, Amount, ShippingAddress, PaymentMethod, Status, CustomerType) 
            VALUES (@ProductName, @Amount, @ShippingAddress, @PaymentMethod, @Status, @CustomerType);
        ";

            using (var command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ProductName", order.ProductName);
                command.Parameters.AddWithValue("@Amount", order.Amount);
                command.Parameters.AddWithValue("@ShippingAddress", order.ShippingAddress ?? "");
                command.Parameters.AddWithValue("@PaymentMethod", order.PaymentMethod.ToString());
                command.Parameters.AddWithValue("@Status", order.Status.ToString());
                command.Parameters.AddWithValue("@CustomerType", order.CustomerType.ToString());

                command.ExecuteNonQuery();
            }
        }
    }

    public List<Order> GetAllOrders()
    {
        List<Order> orders = new List<Order>();
        using (var connection = Database.GetConnection())
        {
            connection.Open();
            string query = "SELECT * FROM Orders";
            using (var command = new SQLiteCommand(query, connection))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    orders.Add(new Order
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        ProductName = reader["ProductName"].ToString(),
                        Amount = Convert.ToDecimal(reader["Amount"]),
                        ShippingAddress = reader["ShippingAddress"].ToString(),
                        PaymentMethod = (PaymentMethod)Enum.Parse(typeof(PaymentMethod), reader["PaymentMethod"].ToString()),
                        Status = (OrderStatus)Enum.Parse(typeof(OrderStatus), reader["Status"].ToString()),
                        CustomerType = (CustomerType)Enum.Parse(typeof(CustomerType), reader["CustomerType"].ToString())
                    });
                }
            }
        }
        return orders;
    }

    public Order GetOrderById(int orderId)
    {
        using (var connection = Database.GetConnection())
        {
            connection.Open();
            string query = "SELECT * FROM Orders WHERE Id = @orderId";
            using (var command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@orderId", orderId);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return new Order
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            ProductName = reader["ProductName"].ToString(),
                            Amount = Convert.ToDecimal(reader["Amount"]),
                            ShippingAddress = reader["ShippingAddress"].ToString(),
                            PaymentMethod = (PaymentMethod)Enum.Parse(typeof(PaymentMethod), reader["PaymentMethod"].ToString()),
                            Status = (OrderStatus)Enum.Parse(typeof(OrderStatus), reader["Status"].ToString()),
                            CustomerType = (CustomerType)Enum.Parse(typeof(CustomerType), reader["CustomerType"].ToString())
                        };
                    }
                }
            }
        }
        return null;
    }

    public void UpdateOrderStatus(int orderId, OrderStatus newStatus)
    {
        using (var connection = Database.GetConnection())
        {
            connection.Open();
            string query = "UPDATE Orders SET Status = @newStatus WHERE Id = @orderId";
            using (var command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@orderId", orderId);
                command.Parameters.AddWithValue("@newStatus", newStatus.ToString());
                command.ExecuteNonQuery();
            }
        }
    }
    public void DeleteOrder(int orderId)
    {
        using (var connection = Database.GetConnection())
        {
            connection.Open();
            string query = "DELETE FROM Orders WHERE Id = @orderId";
            using (var command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@orderId", orderId);
                command.ExecuteNonQuery();
            }
        }
    }
}