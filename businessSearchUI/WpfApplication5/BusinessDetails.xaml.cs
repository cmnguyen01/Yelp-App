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
using Microsoft.Maps.MapControl.WPF;
using Npgsql;
using WpfApplication5;


namespace WpfApplication5
{
    /// <summary>
    /// Interaction logic for BusinessDetails.xaml
    /// </summary>
    public partial class BusinessDetails : Window
    {
        public BusinessDetails(LocalSearch.Business business)
        {
            InitializeComponent();
            showChart(business);
            businessNameLabel.Content = business.Name;
            addressLabel.Content = business.Address;
            InitMap(business);
            InitGrid();
            AddTips(business);
        }
        public void InitMap(LocalSearch.Business business)
        {
            Pushpin pin = new Pushpin();
            pin.Location = new Microsoft.Maps.MapControl.WPF.Location(business.Latitude, business.Longitude);
            // Adds the pushpin to the map.
            businessMap.Children.Add(pin);
            businessMap.Center = pin.Location;
            businessMap.ZoomLevel = 16;
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
        private void AddTips(LocalSearch.Business business)
        {
            tipsGrid.Items.Clear();
            using (var conn = new NpgsqlConnection(buildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {                       
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT user_name, tip_text, date, likes FROM tips NATURAL JOIN user_info WHERE business_id= '" + business.business_id + "';";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tipsGrid.Items.Add(new Tip() { Reviewer_id = reader.GetString(0), Tip_Text = reader.GetString(1), Date = reader.GetDateTime(2), Likes = reader.GetString(3) });
                        }
                    }
                }
                conn.Close();
            }
        }
        private string buildConnString()
        {
            return "Host=localhost; Username=postgres; Password=6765; Database = Project";
        }
        private void showChart(LocalSearch.Business business)
        {
            using (var conn = new NpgsqlConnection(buildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT (SELECT SUM(s) FROM UNNEST(sunday) s) as Sunday, (SELECT SUM(s) FROM UNNEST(monday) s) as Monday, (SELECT SUM(s) FROM UNNEST(tuesday) s) as Tuesday, (SELECT SUM(s) FROM UNNEST(wednesday) s) as Wednesday, (SELECT SUM(s) FROM UNNEST(thursday) s) as Thursday, (SELECT SUM(s) FROM UNNEST(friday) s) as Friday, (SELECT SUM(s) FROM UNNEST(saturday) s) as Saturday FROM check_ins  where business_id = '" + business.business_id.ToString() + "'";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            business.CheckInDetails = new Dictionary<string, int>();
                            business.CheckInDetails.Add("Sunday", reader.GetInt32(0));
                            business.CheckInDetails.Add("Monday", reader.GetInt32(1));
                            business.CheckInDetails.Add("Tuesday", reader.GetInt32(2));
                            business.CheckInDetails.Add("Wednesday", reader.GetInt32(3));
                            business.CheckInDetails.Add("Thursday", reader.GetInt32(4));
                            business.CheckInDetails.Add("Friday", reader.GetInt32(5));
                            business.CheckInDetails.Add("Saturday", reader.GetInt32(6));
                        }
                    }
                }
                conn.Close();

            }

            checkInsChart.DataContext = business.CheckInDetails;

        }
    }
}
