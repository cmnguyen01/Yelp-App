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
using Npgsql;

namespace WpfApplication5
{
    /// <summary>
    /// Interaction logic for BusinessLogin.xaml
    /// </summary>
    public partial class BusinessLogin : Window
    {
        public BusinessLogin()
        {
            InitializeComponent();
            invalid.Visibility = Visibility.Hidden;
           
        }
        private string Buildcomm()
        {
            return "Host=localhost; Username=postgres; Password=6765; Database=Project";
        }
        private void button_Click(object sender, RoutedEventArgs e)
        {
            bool valid = false;
            
            using (var conn = new NpgsqlConnection(Buildcomm()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(Buildcomm()))
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "select business_id from businessuser where business_name='" + UserName.Text + "' AND password='" + Passwordtext.Text + "';";
                    using (var reader = cmd.ExecuteReader())
                    {
                        
                        while(reader.Read())
                        {
                            valid = true;
                            CurrentBusiness.getCurrentUser().UserID = reader.GetString(0);
                        }
                        if(!valid)
                        {
                            invalid.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            BusinessDetails press = new BusinessDetails(valid);
                            press.Show();
                            this.Hide();
                        }

                    }


                }
            }


    }
    }

}
