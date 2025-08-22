using MySql.Data.MySqlClient;

namespace TODOshka;

public class DataBridge
{
    public static MySqlConnection Connection { get; set; }
    public static string UserId { get; set; }
}