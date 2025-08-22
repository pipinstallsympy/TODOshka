using System.Windows;
using MySql.Data.MySqlClient;
using static SQLLogic.SQLLogic;

namespace TODOshka;

public partial class InsertWindow : Window
{
    private int panelNumber;
    private MySqlConnection connection;
    private string userId;
    
    public InsertWindow()
    {
        InitializeComponent();
    }

    public InsertWindow(MySqlConnection connection, string UserId, int data)
    {
        InitializeComponent();
        this.connection = connection;
        userId = UserId;
        panelNumber = data;
    }

    private void DataSendButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            connection.Open();
            SQLInsert(connection, PanelToTable(panelNumber), userId, TaskBox.Text, DescriptionBox.Text);
            connection.Close();
            Close();
            
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
            Close();
        }
    }
}