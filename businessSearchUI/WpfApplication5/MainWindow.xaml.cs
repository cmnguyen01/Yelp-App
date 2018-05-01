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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApplication5
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)// opens register user
        {
            registeruser press = new registeruser(false);
            press.Show();
            this.Close();
        }

        private void button4_Click(object sender, RoutedEventArgs e)// this open business search window
        {
            LocalSearch press = new LocalSearch();
            press.Show();
            this.Close();
        }

        private void button3_Click(object sender, RoutedEventArgs e)//opens register bussiness
        {
            RegisterBussiness press = new RegisterBussiness();
            press.Show();
            this.Close();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            UserLogin x = new UserLogin();
            x.Show();
            //this.Hide();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            BusinessLogin x = new BusinessLogin();
            x.Show();
            //this.Hide();
        }
    }
}
