using System.Diagnostics;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Security.RightsManagement;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MySql.Data.MySqlClient;
using static SQLLogic.SQLLogic;

namespace TODOshka;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    static string connectionString = "server=localhost;port=3306;database=TODOshka;user=user1;password=sussybaka;";
    public MainWindow()
    {
        InitializeComponent();
        AddDataToStackPanel(0);
        AddDataToStackPanel(1);
        AddDataToStackPanel(2);
        AddDataToStackPanel(3);
    }

    private void AddDataToStackPanel(int panel)
    {
        try
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string table = PanelToTable(panel);
                string[][] newData = SQLReadAllMatrix(connection, table);
                int count = newData.Length;
                
                for (int i = 0; i < count; i++)
                {
                    CreateCuteBox(newData[i], panel);
                }
                connection.Close();
            }
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
        }
    }

    private void CreateCuteBox(string[] data, int panel)
    {
        Border box = new Border
        {
            Height = 100,
            BorderBrush = Brushes.Black,
            BorderThickness = new Thickness(5),
            CornerRadius = new CornerRadius(5),
            Margin = new Thickness(10),
            Background = Brushes.Aquamarine
        };
        Grid.SetRow(box, 1);
        Grid.SetColumn(box, panel);

        StackPanel mainStack = new StackPanel
        {
            Orientation = Orientation.Vertical
        };

        Grid taskStack = new Grid
        {
            Background = Brushes.Blue,   
        };
        taskStack.ColumnDefinitions.Add(new ColumnDefinition
        {
            Width = new GridLength(1, GridUnitType.Star)
        });
        taskStack.ColumnDefinitions.Add(new ColumnDefinition{Width = GridLength.Auto});
        
        TextBlock task = new TextBlock
        {
            Text = data[1],
            FontSize = 18,
            FontWeight = FontWeights.Bold
        };

        Button deleteButton = new Button
        {
            Height = 20,
            Width = 20,
            HorizontalAlignment = HorizontalAlignment.Right,
            Content = new Image
            {
                Source = new BitmapImage(new Uri("pack://application:,,,/TODOshka;component/Resources/bin.png")),
                Stretch = Stretch.Uniform
            }
        };
        deleteButton.Click += DeleteButton_Click;
        string[] tagData = new string[4];
        data.CopyTo(tagData, 0);
        tagData[3] = panel.ToString();
        deleteButton.Tag = tagData;
        
        
        TextBlock description = new TextBlock
        {
            Text = data[2]
        };

        Grid descriptionGrid = new Grid
        {
            VerticalAlignment = VerticalAlignment.Stretch
        };
        
        taskStack.Children.Add(task);
        taskStack.Children.Add(deleteButton);
        mainStack.Children.Add(taskStack);
        
        descriptionGrid.Children.Add(description);
        mainStack.Children.Add(descriptionGrid);
        
        box.Child = mainStack;

        switch (panel)
        {
            case 0:
                PanelTasksNeeded0.Children.Add(box);
                break;
            case 1:
                PanelTasksNeeded1.Children.Add(box);
                break;
            case 2:
                PanelTasksNeeded2.Children.Add(box);
                break;
            case 3:
                PanelTasksNeeded3.Children.Add(box);
                break;
            default:
                throw new ArgumentException("Invalid panel in create box");
        }
    }

    private void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        Button button = sender as Button;
        string[] data = button.Tag as string[];
        try
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                SQLDelete(connection, PanelToTable(data[3]), data[1], data[2]);
                RefreshPanel(int.Parse(data[3]));
                connection.Close();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
        Button button = sender as Button;
        string data = button.Tag as string;
        Random random = new Random();
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            SQLInsert(connection, PanelToTable(data), random.Next().ToString(), random.Next().ToString());
            RefreshPanel(int.Parse(data));
            connection.Close();
        }
    }


    private void RefreshPanel(int panel)
    {
        switch (panel)
        {
            case 0:
                PanelTasksNeeded0.Children.Clear();
                break;
            case 1:
                PanelTasksNeeded1.Children.Clear();
                break;
            case 2:
                PanelTasksNeeded2.Children.Clear();
                break;
            case 3:
                PanelTasksNeeded3.Children.Clear();
                break;
            default:
                throw new ArgumentException("Invalid panel in refresh panel");
        }
        AddDataToStackPanel(panel);
    }
}