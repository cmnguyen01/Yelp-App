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

namespace WpfApplication5
{
    /// <summary>
    /// Interaction logic for BussinesessPerZipWindow.xaml
    /// </summary>
    public partial class BussinesessPerZipWindow : Window
    {
        public BussinesessPerZipWindow(Dictionary<string, int> arg)
        {
            InitializeComponent();
            zipsChart.DataContext = arg;
            Style HideLegendStyle = new Style(typeof(Legend));
            HideLegendStyle.Setters.Add(new Setter(WidthProperty, 0.0));
            HideLegendStyle.Setters.Add(new Setter(HeightProperty, 0.0));
            HideLegendStyle.Setters.Add(new Setter(VisibilityProperty, Visibility.Collapsed));
            zipsChart.LegendStyle = HideLegendStyle;
        }
    }
}
