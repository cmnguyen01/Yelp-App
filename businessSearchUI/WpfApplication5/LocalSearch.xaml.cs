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
    public partial class LocalSearch : Window
    {
        //Business class is used to temporarily store information about
        //each business and populate the appropriate UI structures.
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
                    //If no valid zipcode is found, displays 00000 as zipcode.
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

            //this function returs a bool that represents whether the business is open
            //or not during a given interval on a given day.
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
                    if (weekday == "" || openIndex < 0 || closedIndex < 0 )
                    {
                        return true;
                    }
                    if (Hours.ContainsKey(weekday)){
                        int opens = Int32.Parse(Hours[weekday].Trim(' ').Substring(0, 2));
                        int closes = Int32.Parse(Hours[weekday].Trim(' ').Substring(7, 2));
                        if(closedIndex < openIndex)
                        {
                            if(closes > opens)
                            {
                                return false;
                            }
                            else
                            {
                                return (closes > closedIndex && opens < openIndex);
                            }
                        }
                        else
                        {
                            return (closes > closedIndex && opens < openIndex);
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
            businessesPerZipButton.Visibility = Visibility.Hidden;
            if (CurrentUser.getCurrentUser().UserID == null || CurrentUser.getCurrentUser().UserID == "")
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
            Addstates();
            AddColumns2Grid();
            InitHoursBoxes();
        }

        private void InitHoursBoxes()
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
        public void Addstates()
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
        public void AddColumns2Grid()
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
            businessesPerZipButton.Visibility = Visibility.Hidden;
            Zipcode_List.Items.Clear();
            businessGrid.Items.Clear();
            addcities();
            UpdateGrid();
            tagsListBox.Items.Clear();
            RefreshTagsBox();
        }
        private void UpdateGrid()
        {
            if (statelist.SelectedIndex != -1)
            {
                businessGrid.Items.Clear();
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
                        bool needAnd = false;
                        if (!(Zipcode_List.SelectedIndex < 0))
                        {
                            temp.Append(" zipcode = '" + Zipcode_List.SelectedItem.ToString() + "'");
                            needAnd = true;
                        }
                        if (!(cityList.SelectedIndex < 0))
                        {
                            if (needAnd)
                            {
                                temp.Append(" AND");
                            }
                            temp.Append(" city = '" + cityList.SelectedItem.ToString() + "'");
                            needAnd = true;
                        }
                        if (!(statelist.SelectedIndex < 0))
                        {
                            if (needAnd)
                            {
                                temp.Append(" AND");
                            }
                            temp.Append(" state = '" + statelist.SelectedItem.ToString() + "'");
                        }
                        if (tagsListBox.SelectedItems.Count != 0)
                        {
                            temp.Append(" AND ARRAY[");
                            foreach (string x in tagsListBox.SelectedItems)
                            {
                                temp.Append("'" + x + "', ");
                            }
                            temp.Remove(temp.Length - 2, 2);
                            temp.Append("]::varchar[] <@ categories");
                        }
                        cmd.CommandText = temp.ToString();
                        using (var reader = cmd.ExecuteReader())
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
                                if (current.OpenDuring(weekdayComboBox.SelectedIndex, fromHoursComboBox.SelectedIndex, toHoursComboBox.SelectedIndex))
                                {
                                    businessGrid.Items.Add(current);
                                }
                            }
                        }
                    }
                    conn.Close();
                }
            }
        }
        //Whenever a city is selected, or deselected(by state changed).
        private void CityListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cityList.SelectedIndex > -1)
            {
                //Show button for zipcode chart
                businessesPerZipButton.Visibility = Visibility.Visible;
                //clear and refresh zipcodes.
                Zipcode_List.Items.Clear();
                addZipcode();
                //update main grid.
                UpdateGrid();
                //update tags box.
                tagsListBox.Items.Clear();
                RefreshTagsBox();
            }

        }
        //Whenever zipcode is selected, or deselected by changing city or state.
        private void Zipcode_List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Zipcode_List.SelectedIndex > -1)
            {
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
        private void BusDetailsButton_Click(object sender, RoutedEventArgs e)
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

        //Once all three combo boxes for hours have a selection, update grid to reflect choice.
        private void WeekdayComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //update grid if all selections are valid.
            if (weekdayComboBox.SelectedIndex != -1 && fromHoursComboBox.SelectedIndex != -1 && toHoursComboBox.SelectedIndex > fromHoursComboBox.SelectedIndex)
            {
                UpdateGrid();
            }
        }

        private void FromHoursComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //update grid if all selections are valid.
            if (weekdayComboBox.SelectedIndex != -1 && fromHoursComboBox.SelectedIndex != -1 && toHoursComboBox.SelectedIndex > fromHoursComboBox.SelectedIndex)
            {
                UpdateGrid();
            }
        }

        private void ToHoursComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //update grid if all selections are valid.
            if (weekdayComboBox.SelectedIndex != -1 && fromHoursComboBox.SelectedIndex != -1 && toHoursComboBox.SelectedIndex > fromHoursComboBox.SelectedIndex)
            {
                UpdateGrid();
            }
        }

        //Post a tip for selected business.  Button is hidden if no user is logged in.
        private void PostTipButton_Click(object sender, RoutedEventArgs e)
        {
            if (businessGrid.SelectedIndex > -1)
            {
                //Show post tip window for currently selected business.
                PostTip x = new PostTip((Business)businessGrid.SelectedItem);
                x.Show();
            }
            //update grid to reflect new post.
            UpdateGrid();
        }

        private void CheckInButton_Click(object sender, RoutedEventArgs e)
        {
            //Make sure a business is selected to check in to.
            if (businessGrid.SelectedIndex > -1)
            {
                using (var conn = new NpgsqlConnection(buildConnString()))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        //get current date/time
                        DateTime now = DateTime.Now;
                        int day = (int)now.DayOfWeek;
                        //get hour of day from current time.
                        double hour = now.TimeOfDay.TotalHours;
                        //Slot stores which of the 4 times of day check in will go in.
                        //(1= morning, 2=afternoon, 3=evening, 4=night)
                        int slot;
                        if (hour < 6)
                        {
                            //if before 6 AM, counts as previous day's night category
                            day--;
                            slot = 4;
                        }
                        else if (hour < 12)
                        {
                            slot = 1;
                        }
                        else if (hour < 17)
                        {
                            slot = 2;
                        }
                        else if (hour < 23)
                        {
                            slot = 3;
                        }
                        else
                        {
                            slot = 4;
                        }
                        //Get day of week in all lower case
                        string weekday = ((DayOfWeek)day).ToString().ToLower();
                        cmd.Connection = conn;
                        //update appropriate record in DB.
                        cmd.CommandText = "UPDATE check_ins SET " + weekday + "[" + slot + "] = " + weekday + "[" + slot + "] + 1 WHERE business_id = '" + ((Business)businessGrid.SelectedItem).business_id + "';";
                        cmd.ExecuteNonQuery();
                    }
                    conn.Close();
                }
            }
            //After updating DB, update Grid to reflect changes.
            UpdateGrid();
        }

        //Displays a new window with a chart showing how many businesses are in each zipcode of the selected city
        private void BusinessesPerZipButton_Click(object sender, RoutedEventArgs e)
        {
            //This dictionary stores the zipcodes as a key string, with value being num of businesses.
            Dictionary<string, int> zipsDictionary = new Dictionary<string, int>();
            //Populates dictionary from zipcode list box.
            foreach(string x in Zipcode_List.Items)
            {
                using (var conn = new NpgsqlConnection(buildConnString()))
                {
                    conn.Open();
                    //queries database for each zipcode, and counts num of results.
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        //count businesses in each zip.
                        cmd.CommandText = "SELECT COUNT(business_id) FROM businesstable WHERE zipcode = '" + x + "';";
                        var num = cmd.ExecuteReader();
                        num.Read();
                        //store zip and count in dictionary.
                        zipsDictionary.Add(x, num.GetInt32(0));
                    }
                    conn.Close();
                }
            }
            //Displays the new window with chart.  Dictionary is argument to build chart
            //so new window doesn't need to do any queries.
            BussinesessPerZipWindow y = new BussinesessPerZipWindow(zipsDictionary);
            y.Show();
        }

        //clear all selections from Hours Open filter comboboxes.
        private void ClearHoursButton_Click(object sender, RoutedEventArgs e)
        {
            weekdayComboBox.SelectedIndex = -1;
            fromHoursComboBox.SelectedIndex = -1;
            toHoursComboBox.SelectedIndex = -1;
        }
    }
}