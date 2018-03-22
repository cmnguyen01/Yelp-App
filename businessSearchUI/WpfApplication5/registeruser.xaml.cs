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
        public class User
        {
            public string User_id { get; set; }
            public string Username { get; set; }
            public int Review_count { get; set; }
            public string Type { get; set; }
            public int Num_fans { get; set; }
            public double Avg_stars { get; set; }
            public List<String> Friendslist { get; set; }
            public int Cool_count { get; set; }
            public int Funny_count { get; set; }
            public int Useful_count { get; set; }
            public string Yelping_since { get; set; }

            public User(String userID)
            {
                Friendslist = new List<string>();
                using (var conn = new NpgsqlConnection("Host=localhost; Username=postgres; Password=6765; Database=Project"))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT * FROM user_info WHERE user_id = '" + userID + "'";
                        using (var reader = cmd.ExecuteReader())
                        {                          
                            reader.Read();
                            object[] row = new object[11];
                            reader.GetValues(row);
                            User_id = (String)row[0];
                            Username = (String)row[1];
                            Review_count = (int)row[2];
                            Type = (String)row[3];
                            Num_fans = (int)row[4];
                            Avg_stars = (double)row[5];
                            foreach(String x in (String[])row[6])
                            {
                                Friendslist.Add(x);
                            }
                            Cool_count = (int)row[7];
                            Funny_count = (int)row[8];
                            Useful_count = (int)row[9];
                            Yelping_since = (String)row[10];
                        }
                    }
                }
            }
         }
        public registeruser()
        {
            InitializeComponent();
            Addcolumns1();
        }
        private string Buildcomm()
        {
            return "Host=localhost; Username=postgres; Password=6765; Database=Project";
        }
        public void Adduser_id(String x)
        {
            using (var conn = new NpgsqlConnection(Buildcomm()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT DISTINCT user_id FROM user_info WHERE user_name LIKE '" + x +"%'";
                    user_id_list.Items.Clear();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            user_id_list.Items.Add(reader.GetString(0));
                        }
                    }
                }
                conn.Close();
            }

        }
        

        public void Addcolumns1()
        {
            DataGridTextColumn col1= new DataGridTextColumn();
            col1.Header = "User ID";
            col1.Binding = new Binding("User_id");
            col1.Width = 180;
            Friendslist.Columns.Add(col1);
            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Width = 100;
            col2.Header = "Name";
            col2.Binding = new Binding("Username");
            Friendslist.Columns.Add(col2);
            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Width = 80;

            col3.Header = "Avg Stars";
            col3.Binding = new Binding("Avg_stars");
            Friendslist.Columns.Add(col3);
            DataGridTextColumn col4 = new DataGridTextColumn();
            col4.Width = 80;

            col4.Header = "Yelping Since";
            col4.Binding = new Binding("Yelping_since");
            Friendslist.Columns.Add(col4);

        }

        private void User_id_selected(object sender, SelectionChangedEventArgs e)
        {
            if (user_id_list.SelectedIndex != -1)
            {
                User selectedUser = new User((user_id_list.SelectedItem).ToString());
                Friendslist.Items.Clear();
                foreach (String x in selectedUser.Friendslist)
                {
                    Friendslist.Items.Add(new User(x));
                }
                nameBox.Text = selectedUser.Username;
                starsBox.Text = selectedUser.Avg_stars.ToString();
                coolBox.Text = selectedUser.Cool_count.ToString();
                funnyBox.Text = selectedUser.Funny_count.ToString();
                usefulBox.Text = selectedUser.Useful_count.ToString();
                fansBox.Text = selectedUser.Num_fans.ToString();
                yelpingBox.Text = selectedUser.Yelping_since;
            }
        }

        private void Registereduser(object sender, RoutedEventArgs e)
        {

        }

        private void NameTextChanged(object sender, TextChangedEventArgs e)
        {
            Adduser_id(textBox.Text);
        }
    }
}
