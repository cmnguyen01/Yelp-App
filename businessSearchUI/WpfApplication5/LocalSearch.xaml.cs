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
            public string Zipcode { get; set; }
            public string Address { get; set; }
            public int Reviewcount { get; set; }
            public int Totalcheckins { get; set; }
            public string business_id { get; set; }
            public Dictionary<string, int> CheckInDetails { get; set; }
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
            addstates();
            addColumns2Grid();
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
                    cmd.CommandText = "SELECT distinct zipcode FROM businesstable WHERE state = '" + statelist.SelectedItem.ToString() + "AND" + cityList.SelectedItem.ToString() + "'ORDER BY zipcode '"+ "'; ";
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
            if (statelist.SelectedIndex > -1)
            {
                using (var conn = new NpgsqlConnection(buildConnString()))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT name, state, city, zipcode, address, reviewcount, numcheckins, business_id FROM businesstable WHERE state= '" + statelist.SelectedItem.ToString() + "';";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                businessGrid.Items.Add(new Business() { Name = reader.GetString(0), State = reader.GetString(1), City = reader.GetString(2), Zipcode = reader.GetString(3), Address = reader.GetString(4), Reviewcount=reader.GetInt32(5), Totalcheckins=reader.GetInt32(6),business_id=reader.GetString(7)  });
                            }
                        }
                        cmd.CommandText = "SELECT distinct city FROM businesstable WHERE state = '" + statelist.SelectedItem.ToString() + "'; ";
                        cityList.Items.Clear();
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
            tagsListBox.Items.Clear();
            RefreshTagsBox();
        }

        private void CityListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cityList.SelectedIndex > -1)

            {
                Zipcode_List.Items.Clear();
                businessGrid.Items.Clear();
                using (var conn = new NpgsqlConnection(buildConnString()))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT name, state, city, zipcode, address, reviewcount, numcheckins, business_id FROM businesstable WHERE state= '" + statelist.SelectedItem.ToString() + "' AND city= '" + cityList.SelectedItem.ToString() + "';";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                businessGrid.Items.Add(new Business() { Name = reader.GetString(0), State = reader.GetString(1), City = reader.GetString(2), Zipcode = reader.GetString(3), Address = reader.GetString(4), Reviewcount = reader.GetInt32(5), Totalcheckins = reader.GetInt32(6), business_id = reader.GetString(7) });
                            }
                        }

                        cmd.CommandText = "SELECT distinct zipcode FROM businesstable WHERE state = '" + statelist.SelectedItem.ToString() + "' AND city= '" + cityList.SelectedItem.ToString() + "';";
                        Zipcode_List.Items.Clear();
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
            tagsListBox.Items.Clear();
            RefreshTagsBox();
        }

        private void Zipcode_List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (Zipcode_List.SelectedIndex > -1)

            {
                businessGrid.Items.Clear();
                using (var conn = new NpgsqlConnection(buildConnString()))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;

                        if (cityList.SelectedIndex > -1)
                        {
                            cmd.CommandText = "SELECT name, state, city, zipcode, address, reviewcount, numcheckins, business_id FROM businesstable WHERE state= '" + statelist.SelectedItem.ToString() + "' AND city= '" + cityList.SelectedItem.ToString() + "';";
                        }
                        else if(statelist.SelectedIndex> -1)
                        {
                            cmd.CommandText = "SELECT name, state, city, zipcode, address, reviewcount, numcheckins, business_id FROM businesstable WHERE state= '" + statelist.SelectedItem.ToString() + "';";
                        }
                        else
                        {
                            cmd.CommandText = "SELECT name, state, city, zipcode, address, reviewcount, numcheckins, business_id FROM businesstable WHERE state= '" + statelist.SelectedItem.ToString() + "' AND city= '" + cityList.SelectedItem.ToString() + "' AND zipcode= '" + Zipcode_List.SelectedItem.ToString() + "';";
                        }
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                businessGrid.Items.Add(new Business() { Name = reader.GetString(0), State = reader.GetString(1), City = reader.GetString(2), Zipcode = reader.GetString(3), Address = reader.GetString(4), Reviewcount = reader.GetInt32(5), Totalcheckins = reader.GetInt32(6), business_id = reader.GetString(7) });
                            }
                        }
                        tagsListBox.Items.Clear();
                        cmd.CommandText = "select distinct unnest(categories) from businesstable WHERE state = '" + statelist.SelectedItem.ToString() + "' AND city= '" + cityList.SelectedItem.ToString() + "' AND zipcode= '" + Zipcode_List.SelectedItem.ToString() + "';";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                tagsListBox.Items.Add(reader.GetString(0));
                            }
                        }
                        conn.Close();

                    }
                }
            }
            tagsListBox.Items.Clear();
            RefreshTagsBox();
        }
        private void RefreshTagsSearch()
        {
            using (var conn = new NpgsqlConnection(buildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;

                    if (Zipcode_List.SelectedIndex > -1)
                    {
                        cmd.CommandText = "SELECT name, state, city, zipcode, address, reviewcount, numcheckins, business_id FROM businesstable WHERE state= '" + statelist.SelectedItem.ToString() + "' AND city= '" + cityList.SelectedItem.ToString() + "' AND zipcode= '" + Zipcode_List.SelectedItem.ToString() + "'";
                    }
                    else if (cityList.SelectedIndex > -1)
                    {
                        cmd.CommandText = "SELECT name, state, city, zipcode, address, reviewcount, numcheckins, business_id FROM businesstable WHERE state= '" + statelist.SelectedItem.ToString() + "' AND city= '" + cityList.SelectedItem.ToString() + "'";
                    }
                    else
                    {
                        cmd.CommandText = "SELECT name, state, city, zipcode, address, reviewcount, numcheckins, business_id FROM businesstable WHERE state= '" + statelist.SelectedItem.ToString() + "'";
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
                    businessGrid.Items.Clear();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //businessGrid.Items.Add(new Business() { Name = reader.GetString(0), State = reader.GetString(1), City = reader.GetString(2), Zipcode = reader.GetString(3) });
                            businessGrid.Items.Add(new Business() { Name = reader.GetString(0), State = reader.GetString(1), City = reader.GetString(2), Zipcode = reader.GetString(3), Address = reader.GetString(4), Reviewcount = reader.GetInt32(5), Totalcheckins = reader.GetInt32(6), business_id = reader.GetString(7) });
                        }
                    }
                }
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

                    if (Zipcode_List.SelectedIndex > -1)
                    {
                        cmd.CommandText = "SELECT name, state, city, zipcode, address, reviewcount, numcheckins, business_id FROM businesstable WHERE state= '" + statelist.SelectedItem.ToString() + "' AND city= '" + cityList.SelectedItem.ToString() + "' AND zipcode= '" + Zipcode_List.SelectedItem.ToString() + "'";
                    }
                    else if (cityList.SelectedIndex > -1)
                    {
                        cmd.CommandText = "SELECT name, state, city, zipcode, address, reviewcount, numcheckins, business_id FROM businesstable WHERE state= '" + statelist.SelectedItem.ToString() + "' AND city= '" + cityList.SelectedItem.ToString() + "'";
                    }
                    else
                    {
                        cmd.CommandText = "SELECT name, state, city, zipcode, address, reviewcount, numcheckins, business_id FROM businesstable WHERE state= '" + statelist.SelectedItem.ToString() + "'";
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
            RefreshTagsSearch();
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
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT user_name, tip_text, date, likes FROM tips NATURAL JOIN user_info WHERE business_id= '" + current.business_id + "';";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                BusinessTips.Items.Add(new Tip() { Reviewer_id = reader.GetString(0), Tip_Text = reader.GetString(1), Date = reader.GetDateTime(2), Likes = reader.GetString(3) });
                            }
                        }
                    }
                    conn.Close();
                }
            }
        }

        private void busDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            if (businessGrid.SelectedIndex >= 0)
            {
                BusinessDetails detailsWindow = new BusinessDetails((Business)businessGrid.SelectedItem);
                detailsWindow.Show();
            }
        }
    }
}