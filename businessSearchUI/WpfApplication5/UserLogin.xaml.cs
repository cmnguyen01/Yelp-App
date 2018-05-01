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
    /// Interaction logic for UserLogin.xaml
    /// </summary>
    public partial class UserLogin : Window
    {
        CurrentUser currUser;
        public UserLogin()
        {
            InitializeComponent();
            loginError_label.Visibility = Visibility.Hidden;
        }
        private void login_Click_1(object sender, RoutedEventArgs e)
        {
            string username = usernameInput.Text;
            string password = passwordInput.Text;

            using (var conn = new NpgsqlConnection("Host=localhost; Username=postgres; Password=6765; Database=Project"))
            {
                bool isValid = false;
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "Select userid from registration where username='" + username + "'  and userpassword='" + password + "';";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            currUser = CurrentUser.getCurrentUser();
                            currUser.UserID = reader.GetString(0);
                            currUser.UserName = username;
                            isValid = true;
                        }
                    }
                }
                conn.Close();
                if (!isValid)
                {
                    loginError_label.Visibility = Visibility.Visible;
                }
                else
                {
                    registeruser x = new registeruser(true);
                    x.Show();
                    this.Hide();
                }
            }
        }
    }
}
