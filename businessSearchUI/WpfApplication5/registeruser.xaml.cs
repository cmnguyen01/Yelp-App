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
    public class Tip
    {
        public string Reviewer_id { get; set; }
        public string Tip_Text { get; set; }
        public DateTime Date { get; set; }
        public string Likes { get; set; }
    }
    public partial class registeruser : Window
    {
        private bool isProfile;
        User selectedUser;
        CurrentUser currUser;
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
        public registeruser(bool isProfile)
        {
            this.isProfile = isProfile;
            InitializeComponent();
            registrationError_label.Visibility = Visibility.Hidden;
            Addcolumns1();
            addFriend.Visibility = Visibility.Hidden;
            currUser = CurrentUser.getCurrentUser();

            if (isProfile)
            {
                PopulateUserDetails();
                registerbutton.Visibility = Visibility.Hidden;
                newusername_textblock.Visibility = Visibility.Hidden;
                newusername_textbox.Visibility = Visibility.Hidden;
                newpassword_textblock.Visibility = Visibility.Hidden;
                newpassword_textbox.Visibility = Visibility.Hidden;
                addFriend.Visibility = Visibility.Visible;
            }
            else
            {
                addFriend.Visibility = Visibility.Hidden;
                removefriend_button.Visibility = Visibility.Hidden;
                registerbutton.Visibility = Visibility.Visible;
                newusername_textblock.Visibility = Visibility.Visible;
                newusername_textbox.Visibility = Visibility.Visible;
                newpassword_textblock.Visibility = Visibility.Visible;
                newpassword_textbox.Visibility = Visibility.Visible;
            }
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
                    cmd.CommandText = "SELECT DISTINCT user_id FROM user_info WHERE LOWER(user_name) LIKE '%" + x.ToLower() +"%'";
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

            DataGridTextColumn col6 = new DataGridTextColumn();
            col6.Header = "Reviewer";
            col6.Binding = new Binding("Reviewer_id");

            DataGridTextColumn col7 = new DataGridTextColumn();
            col7.Header = "Tip";
            col7.Binding = new Binding("Tip_Text");
            col7.Width = 355;

            DataGridTextColumn col8 = new DataGridTextColumn();
            col8.Header = "#Likes";
            col8.Binding = new Binding("Likes");

            DataGridTextColumn col9 = new DataGridTextColumn();
            col9.Header = "Date";
            col9.Binding = new Binding("Date");

            tips.Columns.Add(col6);
            tips.Columns.Add(col7);
            tips.Columns.Add(col8);
            tips.Columns.Add(col9);

        }
        private string buildConnString()
        {
            return "Host=localhost; Username=postgres; Password=6765; Database = Project";
        }
        private void PopulateUserDetails()
        {
            if (isProfile)
            {
                tipsGridLabel.Content = "My tips: ";
                User current = new User(currUser.UserID);
                nameBox.Text = current.Username;
                starsBox.Text = current.Avg_stars.ToString();
                coolBox.Text = current.Cool_count.ToString();
                funnyBox.Text = current.Funny_count.ToString();
                usefulBox.Text = current.Useful_count.ToString();
                fansBox.Text = current.Num_fans.ToString();
                yelpingBox.Text = current.Yelping_since;
                Friendslist.Items.Clear();
                tips.Items.Clear();
                foreach (String x in new User(currUser.UserID).Friendslist)
                {
                    Friendslist.Items.Add(new User(x));
                    
                }
                using (var conn = new NpgsqlConnection(buildConnString()))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT user_name, tip_text, date, likes FROM tips NATURAL JOIN user_info WHERE user_id= '" + currUser.UserID + "';";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                tips.Items.Add(new Tip() { Reviewer_id = reader.GetString(0), Tip_Text = reader.GetString(1), Date = reader.GetDateTime(2), Likes = reader.GetString(3) });
                            }
                        }
                    }
                    conn.Close();
                }
            }
            else
            {
                nameBox.Text = selectedUser.Username;
                starsBox.Text = selectedUser.Avg_stars.ToString();
                coolBox.Text = selectedUser.Cool_count.ToString();
                funnyBox.Text = selectedUser.Funny_count.ToString();
                usefulBox.Text = selectedUser.Useful_count.ToString();
                fansBox.Text = selectedUser.Num_fans.ToString();
                yelpingBox.Text = selectedUser.Yelping_since;
            }

        }
        private void User_id_selected(object sender, SelectionChangedEventArgs e)
        {
            if (!isProfile)
            {
                addFriend.Visibility = Visibility.Hidden;
                registerbutton.Visibility = Visibility.Visible;
                if (user_id_list.SelectedIndex != -1)
                {
                    tips.Items.Clear();
                    selectedUser = new User((user_id_list.SelectedItem).ToString());
                    Friendslist.Items.Clear();
                    foreach (String x in selectedUser.Friendslist)
                    {
                        Friendslist.Items.Add(new User(x));
                        using (var conn = new NpgsqlConnection(buildConnString()))
                        {
                            conn.Open();
                            using (var cmd = new NpgsqlCommand())
                            {
                                cmd.Connection = conn;
                                cmd.CommandText = "SELECT user_name, tip_text, date, likes FROM tips NATURAL JOIN user_info WHERE user_id= '" + x + "' LIMIT 1;";
                                using (var reader = cmd.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        tips.Items.Add(new Tip() { Reviewer_id = reader.GetString(0), Tip_Text = reader.GetString(1), Date = reader.GetDateTime(2), Likes = reader.GetString(3) });
                                    }
                                }
                            }
                            conn.Close();
                        }
                    }
                    PopulateUserDetails();
                }
            }
            else
            {
                addFriend.Visibility = Visibility.Visible;
                registerbutton.Visibility = Visibility.Hidden;
            }            
        }

        private void registerbutton_Click(object sender, RoutedEventArgs e)
        {
            string userid = selectedUser.User_id;
            string username = newusername_textbox.Text;
            string password = newpassword_textbox.Text;
            bool isValid;
            using (var conn = new NpgsqlConnection("Host=localhost; Username=postgres; Password=6765; Database=Project"))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "select COUNT(*) from registration where userid = '"+ userid + "' OR username = '" + username + "';";
                    using (var reader = cmd.ExecuteReader())
                    {
                        reader.Read();
                        if (reader.GetInt32(0) > 0)
                        {
                            isValid = false;
                            registrationError_label.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            isValid = true;
                            currUser.UserID = userid;
                            currUser.UserName = username;

                        }                        
                    }
                    if (isValid)
                    {
                        cmd.CommandText = "insert into registration (userid, username, userpassword) values ('" + userid + "', '" + username + "', '" + password + "');";
                        cmd.ExecuteNonQuery();
                    }
                }
                conn.Close();
                if (isValid)
                {
                    registeruser x = new registeruser(true);
                    x.Show();
                    this.Hide();
                }
            }
        }

        //private void registerbutton_Click(object sender, RoutedEventArgs e)
        //{
        //    string userid = selectedUser.User_id;
        //    string username = newusername_textbox.Text;
        //    string password = newpassword_textbox.Text;
        //    bool isValid;
        //    using (var conn = new NpgsqlConnection("Host=localhost; Username=postgres; Password=6765; Database=Project"))
        //    {
        //        conn.Open();
        //        using (var cmd = new NpgsqlCommand())
        //        {
        //            cmd.Connection = conn;
        //            cmd.CommandText = "select COUNT(*) from registration where userid = '" + userid + "' OR username = '" + username + "';";
        //            using (var reader = cmd.ExecuteReader())
        //            {
        //                reader.Read();
        //                if (reader.GetInt32(0) > 0)
        //                {
        //                    isValid = false;
        //                    registrationError_label.Visibility = Visibility.Visible;
        //                }
        //                else
        //                {
        //                    isValid = true;
        //                    currUser.UserID = userid;
        //                    currUser.UserName = username;

        //                }
        //            }
        //            if (isValid)
        //            {
        //                cmd.CommandText = "insert into registration (userid, username, userpassword) values ('" + userid + "', '" + username + "', '" + password + "');";
        //                cmd.ExecuteNonQuery();
        //            }
        //        }
        //        conn.Close();
        //        if (isValid)
        //        {
        //            registeruser x = new registeruser(true);
        //            x.Show();
        //            this.Hide();
        //        }
        //    }
        //}

        private void NameTextChanged(object sender, TextChangedEventArgs e)
        {
            Adduser_id(textBox.Text);
        }

        private void Friendslist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isProfile && Friendslist.SelectedIndex > -1)
            {
                removefriend_button.Visibility = Visibility.Visible;
            }
            else
            {
                removefriend_button.Visibility = Visibility.Hidden;
            }
        }

        private void removefriend_button_Click(object sender, RoutedEventArgs e)
        {
            string friend = ((User)Friendslist.SelectedItem).User_id;
            HashSet<String> friends = new HashSet<string>();
            foreach (User x in Friendslist.Items)
            {
                friends.Add(x.User_id);
            }
            friends.Remove(friend);
            string newFriendlist = "{";
            foreach (String y in friends)
            {
                newFriendlist += y + ", ";
            }
            if (newFriendlist.Length > 3)
            {
                newFriendlist = newFriendlist.Substring(0, newFriendlist.Length - 2);
            }
            newFriendlist += "}";
            using (var conn = new NpgsqlConnection("Host=localhost; Username=postgres; Password=6765; Database=Project"))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "update user_info set friends_list = '" + newFriendlist + "' where user_id = '" + currUser.UserID + "';";
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }
            PopulateUserDetails();
        }

        private void addFriend_Click(object sender, RoutedEventArgs e)
        {
            HashSet<String> friends = new HashSet<string>();
            foreach(User friend in Friendslist.Items)
            {
                friends.Add(friend.User_id);
            }
            friends.Add(user_id_list.SelectedItem.ToString());
            string newFriendlist = "{";
            foreach(String friend in friends)
            {
                newFriendlist += friend + ", ";
            }
            newFriendlist = newFriendlist.Substring(0, newFriendlist.Length - 2);
            newFriendlist += "}";
            using (var conn = new NpgsqlConnection("Host=localhost; Username=postgres; Password=6765; Database=Project"))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "update user_info set friends_list = '" + newFriendlist + "' where user_id = '" + currUser.UserID + "';";
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }
            PopulateUserDetails();
        }
    }
}
