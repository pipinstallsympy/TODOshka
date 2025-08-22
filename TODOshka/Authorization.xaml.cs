using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using MySql.Data.MySqlClient;

namespace TODOshka;
public partial class Authorization : Window
{
    private bool isProgrammClose = false;
    public Authorization()
    {
        InitializeComponent();
    }
    private void Authorization_OnClosing(object? sender, CancelEventArgs e)
    {
        if(isProgrammClose) return;
        
        Environment.Exit(0);
    }

    private void ProgrammClose()
    {
        isProgrammClose = true;
        Close();
    }

    private void LoginButton_Click(object sender, RoutedEventArgs e)
    {
        if (LoginBox.Text.Length > 128)
        {
            MessageBox.Show($"kentik, no more than 128 characters in login. You have {LoginBox.Text.Length}");
            return;
        }
        try
        {
            string connectionString = $"server=localhost;port=3306;database=TODOshka;user={LoginBox.Text};password={PasswordBox.Text};";
            
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            connection.Close();
                
            DataBridge.Connection = connection;
            DataBridge.UserId = LoginBox.Text;
            ProgrammClose();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
            Environment.Exit(0);
        }
    }

    private void CreateAccountButton_Click(object sender, RoutedEventArgs e)
    {
        CreateAccount createAccountWindow = new CreateAccount();
        createAccountWindow.ShowDialog();
    }
}