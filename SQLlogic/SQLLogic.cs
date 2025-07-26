namespace SQLLogic;
using MySql.Data.MySqlClient;

public static class SQLLogic
{
    public static string SQLReadAll(MySqlConnection connection, string table)
    {
        MySqlCommand command = new MySqlCommand($"SELECT * FROM {table}", connection);
        using (MySqlDataReader reader = command.ExecuteReader())
        {
            string message = "";
            while (reader.Read())
            { 
                message += ($"ID: {reader["id"]} Task: {reader["task"]} Description: {reader["description"]}\n");
            }
            return message;
        }
    }

    public static void SQLInsert(MySqlConnection connection, string table, string newTask, string newDescription)
    {
        string cmdText = $"""
                          INSERT INTO {table} (task, description)
                          VALUES (@task, @description); 
                          """;
        using (MySqlCommand command = new MySqlCommand(cmdText, connection))
        {
            command.Parameters.AddWithValue("@task", newTask);
            command.Parameters.AddWithValue("@description", newDescription);
            command.ExecuteNonQuery();
        }
    }

    public static void SQLDelete(MySqlConnection connection, string table, string oldTask, string oldDescription)
    {
        string cmdText = $"DELETE FROM {table} WHERE task = @task AND description = @description";
        using (MySqlCommand command = new MySqlCommand(cmdText, connection))
        {
            command.Parameters.AddWithValue("@task", oldTask);
            command.Parameters.AddWithValue("@description", oldDescription);
            command.ExecuteNonQuery();
        }
    }
}