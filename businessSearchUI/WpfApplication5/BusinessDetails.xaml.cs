using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Maps.MapControl.WPF;

namespace WpfApplication5
{
    /// <summary>
    /// Interaction logic for BusinessDetails.xaml
    /// </summary>
    public partial class BusinessDetails : Window
    {
        LocalSearch.Business currentBusiness;
        //should probably move Business class outside of LocalSearch window for easier access.
        public BusinessDetails(LocalSearch.Business business)
        {
            currentBusiness = business;
            InitializeComponent();
            ShowChart();
            businessNameLabel.Content = currentBusiness.Name;
            addressLabel.Content = currentBusiness.Address;
            InitMap();
            InitGrid();
            AddTips();
            InitLabels();
        }
        public void InitLabels()
        {
            if (currentBusiness.IsOpen)
            {
                if (currentBusiness.Hours.ContainsKey("Monday"))
                {
                    mondayHoursLabel.Content = "Monday: " + currentBusiness.Hours["Monday"].Trim(' ').Substring(0, 5) + " - " + currentBusiness.Hours["Monday"].Trim(' ').Substring(6);
                }
                if (currentBusiness.Hours.ContainsKey("Wednesday"))
                {
                    wednesdayHoursLabel.Content = "Wednesday: " + currentBusiness.Hours["Wednesday"].Trim(' ').Substring(0, 5) + " - " + currentBusiness.Hours["Wednesday"].Trim(' ').Substring(6);
                }
                if (currentBusiness.Hours.ContainsKey("Thursday"))
                {
                    thursdayHoursLabel.Content = "Thursday: " + currentBusiness.Hours["Thursday"].Trim(' ').Substring(0, 5) + " - " + currentBusiness.Hours["Thursday"].Trim(' ').Substring(6);
                }
                if (currentBusiness.Hours.ContainsKey("Friday"))
                {
                    fridayHoursLabel.Content = "Friday: " + currentBusiness.Hours["Friday"].Trim(' ').Substring(0, 5) + " - " + currentBusiness.Hours["Friday"].Trim(' ').Substring(6);
                }
                if (currentBusiness.Hours.ContainsKey("Saturday"))
                {
                    saturdayHoursLabel.Content = "Saturday: " + currentBusiness.Hours["Saturday"].Trim(' ').Substring(0, 5) + " - " + currentBusiness.Hours["Saturday"].Trim(' ').Substring(6);
                }
                if (currentBusiness.Hours.ContainsKey("Sunday"))
                {
                    sundayHoursLabel.Content = "Sunday: " + currentBusiness.Hours["Sunday"].Trim(' ').Substring(0, 5) + " - " + currentBusiness.Hours["Sunday"].Trim(' ').Substring(6);
                }
                if (currentBusiness.Hours.ContainsKey("Tuesday"))
                {
                    tuesdayHoursLabel.Content = "Tuesday: " + currentBusiness.Hours["Tuesday"].Trim(' ').Substring(0, 5) + " - " + currentBusiness.Hours["Tuesday"].Trim(' ').Substring(6);
                }
            }
            else
            {
                closedLabel.Visibility = Visibility.Visible;
            }
            avgStarslabel.Content = "Average Stars: " + currentBusiness.Stars;
            foreach(string x in currentBusiness.Tags)
            {
                tagsTextBox.AppendText(x + ", ");
            }
        }

        public void InitMap()
        {
            Pushpin pin = new Pushpin();
            pin.Location = new Microsoft.Maps.MapControl.WPF.Location(currentBusiness.Latitude, currentBusiness.Longitude);
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
        private void AddTips()
        {
            tipsGrid.Items.Clear();
            foreach(LocalSearch.Tip x in currentBusiness.Tips)
            {
                tipsGrid.Items.Add(x);
            }
        }
        private void ShowChart()
        {
            checkInsChart.DataContext = currentBusiness.CheckInDetails;
            Style HideLegendStyle = new Style(typeof(Legend));
            HideLegendStyle.Setters.Add(new Setter(WidthProperty, 0.0));
            HideLegendStyle.Setters.Add(new Setter(HeightProperty, 0.0));
            HideLegendStyle.Setters.Add(new Setter(VisibilityProperty, Visibility.Collapsed));
            checkInsChart.LegendStyle = HideLegendStyle;
        }
    }
}
