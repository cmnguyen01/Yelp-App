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
    /// Interaction logic for Guest_login.xaml
    /// </summary>
    public partial class LocalSearch : Window
    {
        public class Business
        {
            public string Name { get; set; }
            public string State { get; set; }
            public string City { get; set; }
            private string zipcode;
            public string Zipcode
            {
                get
                {
                    if (Int32.TryParse(zipcode, out int x))
                    {
                        return zipcode;
                    }
                    else
                    {
                        return "00000";
                    }
                }
                set
                {
                    zipcode = value;
                }
            }
            public string Address { get; set; }
            public int Reviewcount { get; set; }
            public int Totalcheckins { get; set; }
            public string business_id { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public Dictionary<string, int> CheckInDetails { get; set; }
            public bool IsOpen { get; set; }
            public Dictionary<string, string> Hours { get; set; }
            public List<string> Tags { get; set; }
            public double Stars { get; set; }
            public List<Tip> Tips { get; set; }

            public bool OpenDuring(int weekdayIndex, int openIndex, int closedIndex)
            {
                string weekday;
                switch (weekdayIndex)
                {
                    case 0:
                        weekday = "Monday";
                        break;
                    case 1:
                        weekday = "Tuesday";
                        break;
                    case 2:
                        weekday = "Wednesday";
                        break;
                    case 3:
                        weekday = "Thursday";
                        break;
                    case 4:
                        weekday = "Friday";
                        break;
                    case 5:
                        weekday = "Saturday";
                        break;
                    case 6:
                        weekday = "Sunday";
                        break;
                    default:
                        weekday = "";
                        break;
                }
                if (IsOpen)
                {
                    if (weekday == "" || openIndex < 0 || closedIndex < 0 || (closedIndex < openIndex))
                    {
                        return true;
                    }
                    if (Hours.ContainsKey(weekday)){
                        if(Int32.Parse(Hours[weekday].Substring(0,2)) < openIndex && Int32.Parse(Hours[weekday].Substring(8, 2)) > closedIndex){
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }
        public class Tip
        {
            public string Reviewer_id { get; set; }
            public string Tip_Text { get; set; }
            public DateTime Date { get; set; }
            public string Likes { get; set; }
        }
        public LocalSearch()
        {
            InitializeComponent();
            if(CurrentUser.getCurrentUser().UserID == null || CurrentUser.getCurrentUser().UserID == "")
            {
                postTipButton.IsEnabled = false;
                postTipButton.Visibility = Visibility.Hidden;
                checkInButton.IsEnabled = false;
                checkInButton.Visibility = Visibility.Hidden;
            }
            else
            {
                postTipButton.IsEnabled = true;
                postTipButton.Visibility = Visibility.Visible;
                checkInButton.IsEnabled = true;
                checkInButton.Visibility = Visibility.Visible;
            }
            addstates();
            addColumns2Grid();
            initHoursBoxes();
        }

        private void initHoursBoxes()
        {
            weekdayComboBox.Items.Add("Sunday");
            weekdayComboBox.Items.Add("Monday");
            weekdayComboBox.Items.Add("Tuesday");
            weekdayComboBox.Items.Add("Wednesday");
            weekdayComboBox.Items.Add("Thursday");
            weekdayComboBox.Items.Add("Friday");
            weekdayComboBox.Items.Add("Saturday");
            for(int x = 0; x <24; x++)
            {
                fromHoursComboBox.Items.Add(string.Format("{0:D2}", x) + ":00");
                toHoursComboBox.Items.Add(string.Format("{0:D2}", x) + ":00");
            }
        }

        private string buildConnString()
        {
            return "Host=localhost; Username=postgres; Password=6765; Database = Project";
        }
        public void addstates()
        {
            using (var conn = new NpgsqlConnection(buildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT distinct state FROM businesstable ORDER BY STATE";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            statelist.Items.Add(reader.GetString(0));
                        }
                    }
                }
                conn.Close();
            }
        }
        public void addcities()
        {
            cityList.Items.Clear();
            using (var conn = new NpgsqlConnection(buildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT distinct city FROM businesstable WHERE state = '" + statelist.SelectedItem.ToString() + "'ORDER BY city ";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cityList.Items.Add(reader.GetString(0));
                        }
                    }
                }
                conn.Close();
            }
        }
        public void addZipcode()
        {
            Zipcode_List.Items.Clear();
            //cityList.Items.Clear();
            using (var conn = new NpgsqlConnection(buildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT DISTINCT zipcode FROM businesstable WHERE state = '" + statelist.SelectedItem.ToString() + "' AND city = '" + cityList.SelectedItem.ToString() + "';";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Zipcode_List.Items.Add(reader.GetString(0));
                        }
                    }
                }
                conn.Close();
            }
        }

        public void addColumns2Grid()
        {
            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Header = "Business Name";
            col1.Binding = new Binding("Name");
            col1.Width = 255;

            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Header = "Address";
            col2.Binding = new Binding("Address");

            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Header = "#ofTips";
            col3.Binding = new Binding("Reviewcount");

            DataGridTextColumn col4 = new DataGridTextColumn();
            col4.Header = "TotalCheckins";
            col4.Binding = new Binding("Totalcheckins");

            DataGridTextColumn col5 = new DataGridTextColumn();
            col5.Header = "bussinessID";
            col5.Binding = new Binding("business_id");

            businessGrid.Columns.Add(col1);
            businessGrid.Columns.Add(col2);
            businessGrid.Columns.Add(col3);
            businessGrid.Columns.Add(col4);
            businessGrid.Columns.Add(col5);

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

            BusinessTips.Columns.Add(col6);
            BusinessTips.Columns.Add(col7);
            BusinessTips.Columns.Add(col8);
            BusinessTips.Columns.Add(col9);
        }

        private void statesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Zipcode_List.Items.Clear();
            businessGrid.Items.Clear();
            addcities();
            UpdateGrid();
            tagsListBox.Items.Clear();
            RefreshTagsBox();
        }
        private void UpdateGrid()
        {
            using (var conn = new NpgsqlConnection(buildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    string selectBy = $"SELECT name, state, city, zipcode, address, reviewcount, numcheckins, business_id, " +
                            $"latitude, longitude, hours, openstatus FROM businesstable WHERE";
                    StringBuilder temp = new StringBuilder();
                    temp.Append(selectBy);
                    if (!(Zipcode_List.SelectedIndex < 0))
                    {
                        temp.Append(" zipcode = '" + Zipcode_List.SelectedItem.ToString() + "'");
                    }
                    else if (!(cityList.SelectedIndex < 0))
                    {
                        temp.Append(" city = '" + cityList.SelectedItem.ToString() + "' AND state = '" + statelist.SelectedItem.ToString() + "'");
                    }
                    else if (!(statelist.SelectedIndex < 0))
                    {
                        temp.Append(" state = '" + statelist.SelectedItem.ToString() + "'");
                    }
                    else
                    {
                        temp.Append(" true ");
                    }
                    if (tagsListBox.SelectedItems.Count != 0)
                    {
                        temp.Append(" AND ARRAY[");
                        foreach (string x in tagsListBox.SelectedItems)
                        {
                            temp.Append("'" + x + "', ");
                        }
                        temp.Remove(0, temp.Length - 2);
                        temp.Append("]::varchar[] <@ categories");
                    }
                    cmd.CommandText = temp.ToString();
                    using(var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Business current = new Business()
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
                            ParseHours((string[])reader.GetValue(10), current);
                            if (current.OpenDuring(weekdayComboBox.SelectedIndex , fromHoursComboBox.SelectedIndex,toHoursComboBox.SelectedIndex ))
                            {
                                businessGrid.Items.Add(current);
                            }
                        }
                    }
                }
                conn.Close();
            }
        }
        private void CityListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cityList.SelectedIndex > -1)
            {
                Zipcode_List.Items.Clear();
                businessGrid.Items.Clear();
                addZipcode();
                UpdateGrid();
                tagsListBox.Items.Clear();
                RefreshTagsBox();
            }

        }

        private void Zipcode_List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Zipcode_List.SelectedIndex > -1)
            {
                businessGrid.Items.Clear();
                UpdateGrid();
                tagsListBox.Items.Clear();
                RefreshTagsBox();
            }

        }
        private void RefreshTagsBox()
        {
            using (var conn = new NpgsqlConnection(buildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    string command = $"SELECT name, state, city, zipcode, address, reviewcount, numcheckins, business_id, " +
                            $"latitude, longitude FROM businesstable WHERE ";
                    if (Zipcode_List.SelectedIndex > -1)
                    {
                        cmd.CommandText = command + "zipcode= '" + Zipcode_List.SelectedItem.ToString() + "'";
                    }
                    else if (cityList.SelectedIndex > -1)
                    {
                        cmd.CommandText = command + "state = '" + statelist.SelectedItem.ToString() + "' AND city= '" + cityList.SelectedItem.ToString() + "'";
                    }
                    else
                    {
                        cmd.CommandText = command + "state = '" + statelist.SelectedItem.ToString() + "'";
                    }
                    if (tagsListBox.SelectedItems.Count != 0)
                    {
                        cmd.CommandText = cmd.CommandText + " AND ARRAY[";
                        foreach (string x in tagsListBox.SelectedItems)
                        {
                            cmd.CommandText = cmd.CommandText + "'" + x + "', ";
                        }
                        cmd.CommandText = cmd.CommandText.Substring(0, cmd.CommandText.Length - 2) + "]::varchar[] <@ categories";
                    }
                    cmd.CommandText = "SELECT DISTINCT unnest(categories)" + cmd.CommandText.Substring(33);
                    using (var reader = cmd.ExecuteReader())
                    {
                        List<String> tagsToBeRemoved = new List<string>();

                        foreach (String x in tagsListBox.Items)
                        {
                            if (!tagsListBox.SelectedItems.Contains(x))
                            {
                                tagsToBeRemoved.Add(x);
                            }
                        }
                        foreach (String x in tagsToBeRemoved)
                        {
                            tagsListBox.Items.Remove(x);
                        }
                        while (reader.Read())
                        {
                            if (!tagsListBox.Items.Contains(reader.GetString(0)))
                                tagsListBox.Items.Add(reader.GetString(0));
                        }
                    }
                }
            }
        }
        private void TagsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshTagsBox();
            UpdateGrid();
        }

        private void AddTips(object sender, SelectionChangedEventArgs e)
        {
            BusinessTips.Items.Clear();
            if (businessGrid.SelectedIndex >= 0)
            {
                using (var conn = new NpgsqlConnection(buildConnString()))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        Business current = (Business)businessGrid.SelectedItem;
                        if(current.Tips == null)
                        {
                            current.Tips = new List<Tip>();
                        }
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT user_name, tip_text, date, likes FROM tips NATURAL JOIN user_info WHERE business_id= '" + current.business_id + "';";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Tip newTip = new Tip() { Reviewer_id = reader.GetString(0), Tip_Text = reader.GetString(1), Date = reader.GetDateTime(2), Likes = reader.GetString(3) };
                                BusinessTips.Items.Add(newTip);
                                current.Tips.Add(newTip);
                            }
                        }
                    }
                    conn.Close();
                }
            }
        }
        private void ParseHours(string[] hours, Business current)
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
        private void busDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            Business current = (Business)businessGrid.SelectedItem;
            current.Tags = new List<string>();
            current.Hours = new Dictionary<string, string>();
            using (var conn = new NpgsqlConnection(buildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT hours, stars, openstatus, categories FROM businesstable WHERE business_id = '" + current.business_id + "';";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            current.Stars = reader.GetDouble(1);
                            current.IsOpen = reader.GetBoolean(2);
                            foreach (string x in (string[])reader.GetValue(3))
                            {
                                current.Tags.Add(x);
                            }
                            ParseHours((string[])reader.GetValue(0), current);
                        }
                    }
                }
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    //Sums checkins for each day.
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
                if (businessGrid.SelectedIndex >= 0)
                {
                    BusinessDetails detailsWindow = new BusinessDetails(current);
                    detailsWindow.Show();
                }
            }
        }
        private void MapButton_Click(object sender, RoutedEventArgs e)
        {
            List<Business> list = new List<Business>();

            foreach (Business x in businessGrid.Items)
            {
                list.Add(x);
            }
            LocalMapWindow y = new LocalMapWindow(list);
            y.Show();
        }

        private void WeekdayComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateGrid();
        }

        private void FromHoursComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateGrid();
        }

        private void ToHoursComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateGrid();
        }

        private void postTipButton_Click(object sender, RoutedEventArgs e)
        {
            if (businessGrid.SelectedIndex > -1)
            {
                PostTip x = new PostTip((Business)businessGrid.SelectedItem);
                x.Show();
            }
        }

        private void checkInButton_Click(object sender, RoutedEventArgs e)
        {
            using (var conn = new NpgsqlConnection(buildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText ="" ;
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }
        }
    }
}