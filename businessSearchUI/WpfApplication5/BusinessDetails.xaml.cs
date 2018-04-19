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
    /// Interaction logic for BusinessDetails.xaml
    /// </summary>
    public partial class BusinessDetails : Window
    {
        public BusinessDetails(LocalSearch.Business business)
        {
            InitializeComponent();
            showChart(business);
        }
        private string buildConnString()
        {
            return "Host=localhost; Username=postgres; Password=6765; Database = Project";
        }
        private void showChart(LocalSearch.Business business)
        {
            List<KeyValuePair<string, int>> MyValue = new List<KeyValuePair<string, int>>();
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

            checkInsChart.DataContext = MyValue;

        }
    }
}
