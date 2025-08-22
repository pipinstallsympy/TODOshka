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
                message += ($"ID: {reader["id"]} UserID: {reader["user_id"]} Task: {reader["task"]} Description: {reader["description"]}\n");
            }
            return message;
        }
    }
    
    public static string[][] SQLReadAllMatrix(MySqlConnection connection, string table, string userId)
    {
        string cmdCount = $"SELECT COUNT(*) FROM {table} WHERE user_id = @userId";
        int count;
        using (MySqlCommand commandCount = new MySqlCommand(cmdCount, connection))
        {
            commandCount.Parameters.AddWithValue("@table", table);
            commandCount.Parameters.AddWithValue("@userId", userId);
            count = Convert.ToInt32(commandCount.ExecuteScalar());
        }
        
        string cmdText = $"SELECT * FROM {table} WHERE user_id = @userId";
        MySqlCommand command = new MySqlCommand(cmdText, connection);
        command.Parameters.AddWithValue("@table", table);
        command.Parameters.AddWithValue("@userId", userId);
        
        using (MySqlDataReader reader = command.ExecuteReader())
        {
            string[][] message = new string[count][];
            int i = 0;
            while (reader.Read())
            {
                message[i] = new string[3];
                message[i][0] = reader["id"].ToString();
                message[i][1] = reader["task"].ToString();
                message[i][2] = reader["description"].ToString();
                i++;
            }
            return message;
        }
    }

    public static void SQLInsert(MySqlConnection connection, string table, string userId, string newTask, string newDescription)
    {
        MySqlTransaction transaction = connection.BeginTransaction();
        string cmdText = $"""
                          INSERT INTO {table} (user_id, task, description)
                          VALUES (@userId, @task, @description); 
                          """;
        try
        {
            using (MySqlCommand command = new MySqlCommand(cmdText, connection, transaction))
            {
                command.Parameters.AddWithValue("@task", newTask);
                command.Parameters.AddWithValue("@description", newDescription);
                command.Parameters.AddWithValue("@userId", userId);
                command.ExecuteNonQuery();
            }
            transaction.Commit();
        }
        catch (Exception e)
        {
            transaction.Rollback();
            throw new Exception(e.Message);
        }
    }

    public static void SQLDelete(MySqlConnection connection, string table, string UserId, string oldTask, string oldDescription)
    {
        MySqlTransaction transaction = connection.BeginTransaction();

        try
        {
            string cmdText = $"""
                              DELETE FROM {table} WHERE task = @task AND description = @description AND user_id = @userId;
                              """;
            using (MySqlCommand command = new MySqlCommand(cmdText, connection, transaction))
            {
                command.Parameters.AddWithValue("@task", oldTask);
                command.Parameters.AddWithValue("@description", oldDescription);
                command.Parameters.AddWithValue("@userId", UserId);
                command.ExecuteNonQuery();
            }
        
            string cmdMax = $"""
                             SELECT IFNULL(MAX(id), 0) FROM {table};
                             """;
            int idMax;
            using (MySqlCommand commandMax = new MySqlCommand(cmdMax, connection, transaction))
            {
                idMax = Convert.ToInt32(commandMax.ExecuteScalar());
            }
            string cmdInc = $"""
                             ALTER TABLE {table} AUTO_INCREMENT = @max;
                             """;
            using (MySqlCommand commandInc = new MySqlCommand(cmdInc, connection, transaction))
            {
                commandInc.Parameters.AddWithValue("@max", idMax + 1);
                commandInc.ExecuteNonQuery();
            }
            transaction.Commit();
        }
        catch (Exception e)
        {
            transaction.Rollback();
            throw new Exception($"Ошибка: {e.Message}");
        }
    }

    public static string PanelToTable(int panel)
    {
        string table;
        switch (panel)
        {
            case 0:
                table = "tasks_needed";
                break;
            case 1:
                table = "tasks_top_priority";
                break;
            case 2:
                table = "tasks_in_progress";
                break;
            case 3:
                table = "tasks_finished";
                break;
            default:
                throw new ArgumentException("Invalid panel");
        }
        return table;
    }
    
    public static string PanelToTable(string panel)
    {
        string table;
        switch (Convert.ToInt32(panel))
        {
            case 0:
                table = "tasks_needed";
                break;
            case 1:
                table = "tasks_top_priority";
                break;
            case 2:
                table = "tasks_in_progress";
                break;
            case 3:
                table = "tasks_finished";
                break;
            default:
                throw new ArgumentException("Invalid panel");
        }
        return table;
    }
}