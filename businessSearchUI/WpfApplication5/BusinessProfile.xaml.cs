using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApplication5
{
    /// <summary>
    /// Interaction logic for BusinessProfile.xaml
    /// </summary>
    public partial class BusinessProfile : Window
    {
        public BusinessProfile()
        {
            
            InitializeComponent();
            addcombobox();
            InitGrid();
        }

        private void businessSearchButton_Click(object sender, RoutedEventArgs e)
        {
            LocalSearch press = new LocalSearch();
            press.Show();
        }

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            MainWindow returnTo =new MainWindow();
            returnTo.Show();
            this.Hide();
        }
        private void addcombobox()
        {

            for (int x = 0; x < 24; x++)
            {
                MondayST.Items.Add(string.Format("{0:D2}", x) + ":00");
                TuesdayST.Items.Add(string.Format("{0:D2}", x) + ":00");
                WednesdayST.Items.Add(string.Format("{0:D2}", x) + ":00");
                ThursdayST.Items.Add(string.Format("{0:D2}", x) + ":00");
                FridayST.Items.Add(string.Format("{0:D2}", x) + ":00");
                SaturdayST.Items.Add(string.Format("{0:D2}", x) + ":00");
                SundayST.Items.Add(string.Format("{0:D2}", x) + ":00");
                MondayET.Items.Add(string.Format("{0:D2}", x) + ":00");
                TuesdayET.Items.Add(string.Format("{0:D2}", x) + ":00");
                WednesdayET.Items.Add(string.Format("{0:D2}", x) + ":00");
                ThursdayET.Items.Add(string.Format("{0:D2}", x) + ":00");
                FridayET.Items.Add(string.Format("{0:D2}", x) + ":00");
                SaturdayET.Items.Add(string.Format("{0:D2}", x) + ":00");
                SundayET.Items.Add(string.Format("{0:D2}", x) + ":00");

            }


        }

        private void MondayST_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            updateTime();
        }
        private void updateTime()
        {

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

        }
        public void InitGrid()
        {
            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Header = "Reviewer";
            col1.Binding = new Binding("Reviewer_id");
            col1.Width = 100;

            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Header = "Tip";
            col2.Binding = new Binding("Tip_Text");
            col2.Width = 385;

            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Header = "#Likes";
            col3.Binding = new Binding("Likes");

            DataGridTextColumn col4 = new DataGridTextColumn();
            col4.Header = "Date";
            col4.Binding = new Binding("Date");

            tipsGrid.Columns.Add(col1);
            tipsGrid.Columns.Add(col2);
            tipsGrid.Columns.Add(col3);
            tipsGrid.Columns.Add(col4);
        }
    }
}
