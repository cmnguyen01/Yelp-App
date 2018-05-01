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

                        while (reader.Read())
                        {
                            valid = true;
                            CurrentBusiness.getCurrentUser().UserID = reader.GetString(0);
                        }
                    }
                        if(!valid)
                        {
                            invalid.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            cmd.CommandText = $"SELECT name, address, reviewcount, numcheckins, business_id, " +
                            $"latitude, longitude, hours, openstatus, hours, categories FROM businesstable WHERE business_id='"+CurrentBusiness.getCurrentUser().UserID+"';";
                        using (var reader = cmd.ExecuteReader())
                        {
                            LocalSearch.Business current = new LocalSearch.Business()
                            {
                                Name = reader.GetString(0),
                                State = reader.GetString(1),
                                City = reader.GetString(2),
                                Zipcode = reader.GetString(3),
                                Address = reader.GetString(4),
                                Reviewcount = reader.GetInt32(5),
                                Totalcheckins = reader.GetInt32(6),
                                business_id = reader.GetString(7),
                                Latitude = reader.GetDouble(8),
                                Longitude = reader.GetDouble(9),
                                IsOpen = reader.GetBoolean(11)
                                
                                
                                
                            };
                            ParseHours((string[])reader.GetValue(12),current);
                            current.Tags = new List<string>();
                            foreach( string x in (string[])reader.GetValue(13))
                            {
                                
                                current.Tags.Add(x);
                            }
                            BusinessDetails press = new BusinessDetails(true, current);
                            press.Show();
                            this.Hide();
                        }

                    }

                    conn.Close();
                }
            }


    }
        private void ParseHours(string[] hours, LocalSearch.Business current)
        {
            current.Hours = new Dictionary<string, string>();
            foreach (string x in hours)
            {
                if (x.StartsWith("Monday"))
                {
                    current.Hours.Add("Monday", x.Substring(8));
                }
                if (x.StartsWith("Tuesday"))
                {
                    current.Hours.Add("Tuesday", x.Substring(8));
                }
                if (x.StartsWith("Wednesday"))
                {
                    current.Hours.Add("Wednesday", x.Substring(10));
                }
                if (x.StartsWith("Thursday"))
                {
                    current.Hours.Add("Thursday", x.Substring(9));
                }
                if (x.StartsWith("Friday"))
                {
                    current.Hours.Add("Friday", x.Substring(7));
                }
                if (x.StartsWith("Saturday"))
                {
                    current.Hours.Add("Saturday", x.Substring(9));
                }
                if (x.StartsWith("Sunday"))
                {
                    current.Hours.Add("Sunday", x.Substring(7));
                }
            }
        }
    }

}
