﻿using System;
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
    public partial class Guest_login : Window
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
        }
        public Guest_login()
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
    }
}