using System.Data.SQLite;

namespace ITInternAPP.Data;

public class Database
{
    private const string ConnectionString = "Data Source=orders.db;Version=3;";

    public static void Initialize()
    {
        using (var connection = new SQLiteConnection(ConnectionString))
        {
            connection.Open();
            string createTableQuery = @"
                    CREATE TABLE IF NOT EXISTS Orders (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        ProductName TEXT NOT NULL,
                        Amount DECIMAL(10,2) NOT NULL,
                        CustomerType TEXT NOT NULL,
                        ShippingAddress TEXT,
                        PaymentMethod TEXT NOT NULL,
                        Status TEXT NOT NULL
                    );";
            using (var command = new SQLiteCommand(createTableQuery, connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }

    public static SQLiteConnection GetConnection()
    {
        return new SQLiteConnection(ConnectionString);
    }
}