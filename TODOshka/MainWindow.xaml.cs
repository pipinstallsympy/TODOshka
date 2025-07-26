using System.Windows;
using MySql.Data.MySqlClient;
using static SQLLogic.SQLLogic;

namespace TODOshka;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    public void ButtonClickRead(object sender, RoutedEventArgs e)
    {
        string connectionString = "server=localhost;port=3306;database=TODOshka;user=user1;password=sussybaka;";
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                MessageBox.Show(SQLReadAll(connection, "tasks_needed"));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }

    public void ButtonClickAdd(object sender, RoutedEventArgs e)
    {
        string connectionString = "server=localhost;port=3306;database=TODOshka;user=user1;password=sussybaka;";
        
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                SQLInsert(connection, "tasks_needed", TaskAdd.Text, DescriptionAdd.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }

    public void ButtonClickDelete(object sender, RoutedEventArgs e)
    {
        string connectionString = "server=localhost;port=3306;database=TODOshka;user=user1;password=sussybaka;";

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                SQLDelete(connection, "tasks_needed", TaskDelete.Text, DescriptionDelete.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}