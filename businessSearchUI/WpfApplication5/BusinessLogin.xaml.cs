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
                    LocalSearch.Business current = new LocalSearch.Business();
                        if (!valid)
                        {
                            invalid.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            cmd.CommandText = $"SELECT name, address, reviewcount, numcheckins, business_id, " +
                            $"latitude, longitude, hours, openstatus, categories, stars FROM businesstable WHERE business_id='"+CurrentBusiness.getCurrentUser().UserID+"';";
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                current = new LocalSearch.Business()
                                {
                                    Name = reader.GetString(0),
                                    Address = reader.GetString(1),
                                    Reviewcount = reader.GetInt32(2),
                                    Totalcheckins = reader.GetInt32(3),
                                    business_id = reader.GetString(4),
                                    Latitude = reader.GetDouble(5),
                                    Longitude = reader.GetDouble(6),
                                    IsOpen = reader.GetBoolean(8),
                                    Stars = reader.GetDouble(10)
                                };
                                ParseHours((string[])reader.GetValue(7), current);
                                current.Tags = new List<string>();
                                foreach (string x in (string[])reader.GetValue(9))
                                {
                                    current.Tags.Add(x);
                                }
                            }
                        }
                        cmd.CommandText = "SELECT user_name, tip_text, date, likes FROM tips NATURAL JOIN user_info WHERE business_id= '" + current.business_id + "';";
                        using (var reader = cmd.ExecuteReader())
                        {
                            current.Tips = new List<LocalSearch.Tip>();
                            while (reader.Read())
                            {
                                LocalSearch.Tip newTip = new LocalSearch.Tip() { Reviewer_id = reader.GetString(0), Tip_Text = reader.GetString(1), Date = reader.GetDateTime(2), Likes = reader.GetString(3) };
                                current.Tips.Add(newTip);
                            }
                        }
                        cmd.CommandText = "SELECT (SELECT SUM(s) FROM UNNEST(sunday) s) as Sunday, (SELECT SUM(s) FROM UNNEST(monday) s) as Monday, (SELECT SUM(s) FROM UNNEST(tuesday) s) as Tuesday, (SELECT SUM(s) FROM UNNEST(wednesday) s) as Wednesday, (SELECT SUM(s) FROM UNNEST(thursday) s) as Thursday, (SELECT SUM(s) FROM UNNEST(friday) s) as Friday, (SELECT SUM(s) FROM UNNEST(saturday) s) as Saturday FROM check_ins  where business_id = '" + current.business_id.ToString() + "'";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                current.CheckInDetails = new Dictionary<string, int>();
                                current.CheckInDetails.Add("Sunday", reader.GetInt32(0));
                                current.CheckInDetails.Add("Monday", reader.GetInt32(1));
                                current.CheckInDetails.Add("Tuesday", reader.GetInt32(2));
                                current.CheckInDetails.Add("Wednesday", reader.GetInt32(3));
                                current.CheckInDetails.Add("Thursday", reader.GetInt32(4));
                                current.CheckInDetails.Add("Friday", reader.GetInt32(5));
                                current.CheckInDetails.Add("Saturday", reader.GetInt32(6));
                            }
                        }
                    }
                    conn.Close();
                    BusinessDetails press = new BusinessDetails(true, current);
                    press.Show();
                    this.Hide();
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
