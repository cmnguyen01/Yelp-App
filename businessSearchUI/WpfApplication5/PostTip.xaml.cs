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
    /// Interaction logic for PostTip.xaml
    /// </summary>
    public partial class PostTip : Window
    {
        string userName;
        string businessID;
        string businessName;
        string userID;
        string date;
        public PostTip(LocalSearch.Business arg)
        {
            userName = CurrentUser.getCurrentUser().UserName;
            userID = CurrentUser.getCurrentUser().UserID;
            businessID = arg.business_id;
            businessName = arg.Name;
            InitializeComponent();
            userLabel.Content = "User: " + userName;
            businessLabel.Content = "Business: " + arg.Name;
            businessID = arg.business_id;
            date = DateTime.Now.ToString();
            FormatDate();
            dateLabel.Content = "Date: " + date;
        }
        private void FormatDate()
        {
            string[] split = date.Split('/');
            date = split[2] + "-" + split[0] + "-" + split[1];
        }
        private string buildConnString()
        {
            return "Host=localhost; Username=postgres; Password=6765; Database = Project";
        }
        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void submitButton_Click(object sender, RoutedEventArgs e)
        {
            using (var conn = new NpgsqlConnection(buildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "INSERT INTO tips (user_id, business_id, tip_text, date) VALUES (" + userID + ", " + businessID + ", " + tipText.Text + "," + date + ");" ;
                    if (cmd.ExecuteNonQuery() == 0)
                    {
                        //failure
                    }
                    else
                    {
                        //Tip is posted.
                        conn.Close();
                        this.Hide();
                    }
                }
                conn.Close();
            }
        }
    }
}
