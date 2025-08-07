using System.Windows;
using MySql.Data.MySqlClient;
using static SQLLogic.SQLLogic;

namespace TODOshka;

public partial class InsertWindow : Window
{
    private int panelNumber;
    private string connectionString;
    
    public InsertWindow()
    {
        InitializeComponent();
    }

    public InsertWindow(int data, string connection)
    {
        InitializeComponent();
        panelNumber = data;
        connectionString = connection;
    }

    private void DataSendButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                SQLInsert(connection, PanelToTable(panelNumber), TaskBox.Text, DescriptionBox.Text);
                connection.Close();
                Close();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
            Close();
        }
    }
}