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
using Npgsql;

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
            saveChangesButtion.Visibility = Visibility.Hidden;
        }
        public BusinessDetails(bool isProfile, LocalSearch.Business business)
        {
            InitializeComponent();
            currentBusiness = business;
            ShowChart();
            businessNameLabel.Content = currentBusiness.Name;
            addressLabel.Content = currentBusiness.Address;
            InitMap();
            InitGrid();
            AddTips();
            HoursComboBoxes();
            saveChangesButtion.Visibility = Visibility.Visible;
        }
        public void HoursComboBoxes()
        {
            List<ComboBox> hoursBoxList = new List<ComboBox>();

            mondayComboLabel.Visibility = Visibility.Visible;
            mondayStartHoursBox.Visibility = Visibility.Visible;
            mondayToLabel.Visibility = Visibility.Visible;
            mondayEndHoursBox.Visibility = Visibility.Visible;
            hoursBoxList.Add(mondayStartHoursBox);
            hoursBoxList.Add(mondayEndHoursBox);

            tuesdayComboLabel.Visibility = Visibility.Visible;
            tuesdayStartHoursBox.Visibility = Visibility.Visible;
            tuesdayToLabel.Visibility = Visibility.Visible;
            tuesdayEndHoursBox.Visibility = Visibility.Visible;
            hoursBoxList.Add(tuesdayStartHoursBox);
            hoursBoxList.Add(tuesdayEndHoursBox);

            wednesdayComboLabel.Visibility = Visibility.Visible;
            wednesdayStartHoursBox.Visibility = Visibility.Visible;
            wednesdayToLabel.Visibility = Visibility.Visible;
            wednesdayEndHoursBox.Visibility = Visibility.Visible;
            hoursBoxList.Add(wednesdayStartHoursBox);
            hoursBoxList.Add(wednesdayEndHoursBox);

            thursdayComboLabel.Visibility = Visibility.Visible;
            thursdayStartHoursBox.Visibility = Visibility.Visible;
            thursdayToLabel.Visibility = Visibility.Visible;
            thursdayEndHoursBox.Visibility = Visibility.Visible;
            hoursBoxList.Add(thursdayStartHoursBox);
            hoursBoxList.Add(thursdayEndHoursBox);

            fridayComboLabel.Visibility = Visibility.Visible;
            fridayStartHoursBox.Visibility = Visibility.Visible;
            fridayToLabel.Visibility = Visibility.Visible;
            fridayEndHoursBox.Visibility = Visibility.Visible;
            hoursBoxList.Add(fridayStartHoursBox);
            hoursBoxList.Add(fridayEndHoursBox);

            saturdayComboLabel.Visibility = Visibility.Visible;
            saturdayStartHoursBox.Visibility = Visibility.Visible;
            saturdayToLabel.Visibility = Visibility.Visible;
            saturdayEndHoursBox.Visibility = Visibility.Visible;
            hoursBoxList.Add(saturdayStartHoursBox);
            hoursBoxList.Add(saturdayEndHoursBox);

            sundayComboLabel.Visibility = Visibility.Visible;
            sundayStartHoursBox.Visibility = Visibility.Visible;
            sundayToLabel.Visibility = Visibility.Visible;
            sundayEndHoursBox.Visibility = Visibility.Visible;
            hoursBoxList.Add(sundayStartHoursBox);
            hoursBoxList.Add(sundayEndHoursBox);
            foreach(ComboBox x in hoursBoxList)
            {
                for(int y = 0; y < 24; y++)
                {
                    x.Items.Add(string.Format("{0:D2}", y) + ":00");
                }
            }
            if (currentBusiness.Hours.ContainsKey("Monday"))
            {
                mondayStartHoursBox.SelectedIndex = Int32.Parse(currentBusiness.Hours["Monday"].Trim(' ').Substring(0, 2));
                mondayEndHoursBox.SelectedIndex = Int32.Parse(currentBusiness.Hours["Monday"].Trim(' ').Substring(7, 2));
            }
            if (currentBusiness.Hours.ContainsKey("Tuesday"))
            {
                tuesdayStartHoursBox.SelectedIndex = Int32.Parse(currentBusiness.Hours["Tuesday"].Trim(' ').Substring(0, 2));
                tuesdayEndHoursBox.SelectedIndex = Int32.Parse(currentBusiness.Hours["Tuesday"].Trim(' ').Substring(7, 2));
            }

            if (currentBusiness.Hours.ContainsKey("Wednesday"))
            {
                wednesdayStartHoursBox.SelectedIndex = Int32.Parse(currentBusiness.Hours["Wednesday"].Trim(' ').Substring(0, 2));
                wednesdayEndHoursBox.SelectedIndex = Int32.Parse(currentBusiness.Hours["Wednesday"].Trim(' ').Substring(7, 2));
            }
            if (currentBusiness.Hours.ContainsKey("Thursday"))
            {
                thursdayStartHoursBox.SelectedIndex = Int32.Parse(currentBusiness.Hours["Thursday"].Trim(' ').Substring(0, 2));
                thursdayEndHoursBox.SelectedIndex = Int32.Parse(currentBusiness.Hours["Thursday"].Trim(' ').Substring(7, 2));
            }
            if (currentBusiness.Hours.ContainsKey("Friday"))
            {
                fridayStartHoursBox.SelectedIndex = Int32.Parse(currentBusiness.Hours["Friday"].Trim(' ').Substring(0, 2));
                fridayEndHoursBox.SelectedIndex = Int32.Parse(currentBusiness.Hours["Friday"].Trim(' ').Substring(7, 2));
            }
            if (currentBusiness.Hours.ContainsKey("Saturday"))
            {
                saturdayStartHoursBox.SelectedIndex = Int32.Parse(currentBusiness.Hours["Saturday"].Trim(' ').Substring(0, 2));
                saturdayEndHoursBox.SelectedIndex = Int32.Parse(currentBusiness.Hours["Saturday"].Trim(' ').Substring(7, 2));
            }
            if (currentBusiness.Hours.ContainsKey("Sunday"))
            {
                sundayStartHoursBox.SelectedIndex = Int32.Parse(currentBusiness.Hours["Sunday"].Trim(' ').Substring(0, 2));
                sundayEndHoursBox.SelectedIndex = Int32.Parse(currentBusiness.Hours["Sunday"].Trim(' ').Substring(7, 2));
            }
            foreach (string x in currentBusiness.Tags)
            {
                tagsTextBox.AppendText(x + ", ");
            }
            tagsTextBox.Text = tagsTextBox.Text.Substring(0, tagsTextBox.Text.Length - 2);
        }
        public void InitLabels()
        {
            //Populate business hours for any days they are open.
            if (currentBusiness.IsOpen) //if closed, they have no hours.
            {
                if (currentBusiness.Hours.ContainsKey("Monday"))
                {
                    mondayHoursLabel.Content = "Monday: " + currentBusiness.Hours["Monday"].Trim(' ').Substring(0, 5) + " - " + currentBusiness.Hours["Monday"].Trim(' ').Substring(6);
                    mondayHoursLabel.Visibility = Visibility.Visible;
                }
                if (currentBusiness.Hours.ContainsKey("Wednesday"))
                {
                    wednesdayHoursLabel.Content = "Wednesday: " + currentBusiness.Hours["Wednesday"].Trim(' ').Substring(0, 5) + " - " + currentBusiness.Hours["Wednesday"].Trim(' ').Substring(6);
                    wednesdayHoursLabel.Visibility = Visibility.Visible;
                }
                if (currentBusiness.Hours.ContainsKey("Thursday"))
                {
                    thursdayHoursLabel.Content = "Thursday: " + currentBusiness.Hours["Thursday"].Trim(' ').Substring(0, 5) + " - " + currentBusiness.Hours["Thursday"].Trim(' ').Substring(6);
                    thursdayHoursLabel.Visibility = Visibility.Visible;
                }
                if (currentBusiness.Hours.ContainsKey("Friday"))
                {
                    fridayHoursLabel.Content = "Friday: " + currentBusiness.Hours["Friday"].Trim(' ').Substring(0, 5) + " - " + currentBusiness.Hours["Friday"].Trim(' ').Substring(6);
                    fridayHoursLabel.Visibility = Visibility.Visible;
                }
                if (currentBusiness.Hours.ContainsKey("Saturday"))
                {
                    saturdayHoursLabel.Content = "Saturday: " + currentBusiness.Hours["Saturday"].Trim(' ').Substring(0, 5) + " - " + currentBusiness.Hours["Saturday"].Trim(' ').Substring(6);
                    saturdayHoursLabel.Visibility = Visibility.Visible;
                }
                if (currentBusiness.Hours.ContainsKey("Sunday"))
                {
                    sundayHoursLabel.Content = "Sunday: " + currentBusiness.Hours["Sunday"].Trim(' ').Substring(0, 5) + " - " + currentBusiness.Hours["Sunday"].Trim(' ').Substring(6);
                    sundayHoursLabel.Visibility = Visibility.Visible;
                }
                if (currentBusiness.Hours.ContainsKey("Tuesday"))
                {
                    tuesdayHoursLabel.Content = "Tuesday: " + currentBusiness.Hours["Tuesday"].Trim(' ').Substring(0, 5) + " - " + currentBusiness.Hours["Tuesday"].Trim(' ').Substring(6);
                    tuesdayHoursLabel.Visibility = Visibility.Visible;
                }
            }
            else//display closed label if closed.
            {
                closedLabel.Visibility = Visibility.Visible;
            }
            avgStarslabel.Content = "Average Stars: " + currentBusiness.Stars;
            //Fill tags box with all tags for business.
            foreach(string x in currentBusiness.Tags)
            {
                tagsTextBox.AppendText(x + ", ");
            }
            tagsTextBox.Text = tagsTextBox.Text.Substring(0, tagsTextBox.Text.Length - 2);
            tagsTextBox.Focusable = false;
            tagsTextBox.IsReadOnly = true;
        }
        //Initializes the map with a pin for the business location based on GPS coords.
        public void InitMap()
        {
            Pushpin pin = new Pushpin();
            pin.Location = new Microsoft.Maps.MapControl.WPF.Location(currentBusiness.Latitude, currentBusiness.Longitude);
            // Adds the pushpin to the map.
            businessMap.Children.Add(pin);
            businessMap.Center = pin.Location;
            businessMap.ZoomLevel = 16;
        }

        //Setup for columns for tips datagrid 
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

        //populates tips datagrid with all of the business' tips.
        private void AddTips()
        {
            tipsGrid.Items.Clear();
            foreach(LocalSearch.Tip x in currentBusiness.Tips)
            {
                tipsGrid.Items.Add(x);
            }
        }
        //displays a chart of the business checkins per weekday.
        private void ShowChart()
        {
            checkInsChart.DataContext = currentBusiness.CheckInDetails;

            //Following code hides the (unnecesary and redundant) legend of the chart.
            Style HideLegendStyle = new Style(typeof(Legend));
            HideLegendStyle.Setters.Add(new Setter(WidthProperty, 0.0));
            HideLegendStyle.Setters.Add(new Setter(HeightProperty, 0.0));
            HideLegendStyle.Setters.Add(new Setter(VisibilityProperty, Visibility.Collapsed));
            checkInsChart.LegendStyle = HideLegendStyle;
        }

        private void tempProfileButton_Click(object sender, RoutedEventArgs e)
        {
            BusinessDetails x = new BusinessDetails(true, currentBusiness);
            x.Show();
        }

        private void saveChangesButtion_Click(object sender, RoutedEventArgs e)
        {
            using (var conn = new NpgsqlConnection(buildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    StringBuilder temp = new StringBuilder();
                    temp.Append("{");
                    if(mondayStartHoursBox.SelectedIndex != -1 && mondayEndHoursBox.SelectedIndex > mondayStartHoursBox.SelectedIndex)
                    {
                        temp.Append("\"Monday, "+ string.Format("{0:D2}", mondayStartHoursBox.SelectedIndex) + ":00, " + string.Format("{0:D2}", mondayEndHoursBox.SelectedIndex) + ":00\", ");
                    }
                    if (tuesdayStartHoursBox.SelectedIndex != -1 && tuesdayEndHoursBox.SelectedIndex > tuesdayStartHoursBox.SelectedIndex)
                    {
                        temp.Append("\"Tuesday, " + string.Format("{0:D2}", tuesdayStartHoursBox.SelectedIndex) + ":00, " + string.Format("{0:D2}", tuesdayEndHoursBox.SelectedIndex) + ":00\", ");
                    }
                    if (wednesdayStartHoursBox.SelectedIndex != -1 && wednesdayEndHoursBox.SelectedIndex > wednesdayStartHoursBox.SelectedIndex)
                    {
                        temp.Append("\"Wednesday, " + string.Format("{0:D2}", wednesdayStartHoursBox.SelectedIndex) + ":00, " + string.Format("{0:D2}", wednesdayEndHoursBox.SelectedIndex) + ":00\", ");
                    }
                    if (thursdayStartHoursBox.SelectedIndex != -1 && thursdayEndHoursBox.SelectedIndex > thursdayStartHoursBox.SelectedIndex)
                    {
                        temp.Append("\"Thursday, " + string.Format("{0:D2}", thursdayStartHoursBox.SelectedIndex) + ":00, " + string.Format("{0:D2}", thursdayEndHoursBox.SelectedIndex) + ":00\", ");
                    }
                    if (fridayStartHoursBox.SelectedIndex != -1 && fridayEndHoursBox.SelectedIndex > fridayStartHoursBox.SelectedIndex)
                    {
                        temp.Append("\"Friday, " + string.Format("{0:D2}", fridayStartHoursBox.SelectedIndex) + ":00, " + string.Format("{0:D2}", fridayEndHoursBox.SelectedIndex) + ":00\", ");
                    }
                    if (saturdayStartHoursBox.SelectedIndex != -1 && saturdayEndHoursBox.SelectedIndex > saturdayStartHoursBox.SelectedIndex)
                    {
                        temp.Append("\"Saturday, " + string.Format("{0:D2}", saturdayStartHoursBox.SelectedIndex) + ":00, " + string.Format("{0:D2}", saturdayEndHoursBox.SelectedIndex) + ":00\", ");
                    }
                    if (sundayStartHoursBox.SelectedIndex != -1 && sundayEndHoursBox.SelectedIndex > sundayStartHoursBox.SelectedIndex)
                    {
                        temp.Append("\"Sunday, " + string.Format("{0:D2}", sundayStartHoursBox.SelectedIndex) + ":00, " + string.Format("{0:D2}", sundayEndHoursBox.SelectedIndex) + ":00\", ");
                    }
                    string temp2 = temp.ToString().Substring(0, temp.Length - 2);
                    temp.Clear();
                    temp.Append(temp2);
                    temp.Append("}");
                    string hours = temp.ToString();
                    temp.Clear();
                    temp.Append("{");
                    string[] temp3 = tagsTextBox.Text.ToString().Split(',');
                    foreach(string x in temp3)
                    {
                        temp.Append("\"" + x + "\", ");
                    }
                    temp2 = temp.ToString().Substring(0, temp.Length - 2);
                    temp.Clear();
                    temp.Append(temp2);
                    temp.Append("}");
                    string tags = temp.ToString();
                    cmd.CommandText = "UPDATE businesstable SET hours = '" + hours + "', categories = '" + tags + "' WHERE business_id = '" + currentBusiness.business_id + "';";
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }
        }
        private string buildConnString()
        {
            return "Host=localhost; Username=postgres; Password=6765; Database = Project";
        }
    }
}
