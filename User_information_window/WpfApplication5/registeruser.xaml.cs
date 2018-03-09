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
    /// Interaction logic for registeruser.xaml
    /// </summary>
    /// 
    public partial class registeruser : Window
    {
        public class user
        {
        public string user_id { get; set; }
        public string username { get; set; }
        public int review_count { get; set; }
        public string type { get; set; }
        public int num_fans { get; set; }
        public double avg_stars { get; set; }
        public int [] friendslist { get; set; }
        public int cool_count { get; set; }
        public int funny_count { get; set; }
        public int useful_count { get; set; }
        public string yelping_since { get; set; }
         }
        public registeruser()
        {
            InitializeComponent();
            adduser_id();
        }
        private string buildcomm()
        {
            return " Host=localhost; Username=postgres; Password=1994; Database=Project";
        }
        public void adduser_id()
        {
            using (var conn = new NpgsqlConnection(buildcomm()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT DISTINCT user_id FROM user_info ORDER BY user_id";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            //Friendslist.Items.Add(reader.GetString(0));
                            user_id_list.Items.Add(reader.GetString(0));
                        }
                    }
                }
                conn.Close();
            }

        }
        

        //private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{

        //}

        //private void button_Click(object sender, RoutedEventArgs e)
        //{

        //}

        //private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        //{

        //}

        //private void friendslist_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        //{

        //}

        //private void tips_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{

        //}
        public void addcolumns1()
        {
            DataGridTextColumn col= new DataGridTextColumn();
        }

        private void user_id_selected(object sender, SelectionChangedEventArgs e)
        {

        }

        private void user_textchanged(object sender, TextChangedEventArgs e)
        {

        }

        private void registereduser(object sender, RoutedEventArgs e)
        {

        }
    }
}
