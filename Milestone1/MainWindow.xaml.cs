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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Npgsql;

namespace Milestone1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public class Business
        {
            public string Name { get; set; }
            public string State { get; set; }
            public string City { get; set; }
        }


        public MainWindow()
        {
            InitializeComponent();
            addStates();
            addColumns2Grid();
        }
        private string buildConnString()
        {
            return "Host=localhost; Username=postgres; Password=6765; Database = milestone1db";
        }

        public void addStates()
        {
            using (var conn = new NpgsqlConnection(buildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT distinct state FROM business ORDER BY STATE";
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
        public void addCities()
        {
            cityList.Items.Clear();
            using (var conn = new NpgsqlConnection(buildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT distinct city FROM business WHERE state = '" + statelist.SelectedItem.ToString() + "'; ";
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
        public void addColumns2Grid()
        {
            DataGridTextColumn col1 = new DataGridTextColumn();        
            col1.Header = "Business Name";
            col1.Binding = new Binding("Name");
            col1.Width = 255;

            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Header = "State";
            col2.Binding = new Binding("State");

            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Header = "City";
            col3.Binding = new Binding("City");

            businessGrid.Columns.Add(col1);
            businessGrid.Columns.Add(col2);
            businessGrid.Columns.Add(col3);

        }

        private void statelist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            businessGrid.Items.Clear();
            if (statelist.SelectedIndex > -1)
            {
                using (var conn = new NpgsqlConnection(buildConnString()))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT name, state, city FROM business WHERE state= '" + statelist.SelectedItem.ToString() + "';";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                businessGrid.Items.Add(new Business() { Name = reader.GetString(0), State = reader.GetString(1), City = reader.GetString(2) });
                            }
                        }
                        cmd.CommandText = "SELECT distinct city FROM business WHERE state = '" + statelist.SelectedItem.ToString() + "'; ";
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
        }

        private void cityList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (cityList.SelectedIndex > -1)
            {
                businessGrid.Items.Clear();
                using (var conn = new NpgsqlConnection(buildConnString()))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT name, state, city FROM business WHERE state= '" + statelist.SelectedItem.ToString() + "' AND city= '"+cityList.SelectedItem.ToString()+"';";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                businessGrid.Items.Add(new Business() { Name = reader.GetString(0), State = reader.GetString(1), City = reader.GetString(2) });
                            }
                        }

                    }
                    conn.Close();

                }
            }
        }
    }

}
