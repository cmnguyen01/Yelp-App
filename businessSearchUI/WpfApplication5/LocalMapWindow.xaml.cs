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

namespace WpfApplication5
{
    /// <summary>
    /// Interaction logic for LocalMapWindow.xaml
    /// </summary>
    public partial class LocalMapWindow : Window
    {
        LocationRect bounds;
        public LocalMapWindow(List<LocalSearch.Business> arg)
        {
            IList<Location> pins = new List<Location>();
            InitializeComponent();
            foreach(LocalSearch.Business x in arg)
            {
                
                Pushpin pin = new Pushpin();
                pin.Location = new Location(x.Latitude, x.Longitude);
                localMap.Children.Add(pin);
                pins.Add(pin.Location);
            }
            bounds = new LocationRect(pins);

        }

        private void localMap_Initialized(object sender, EventArgs e)
        {
        }

        private void localMap_Loaded(object sender, RoutedEventArgs e)
        {
            localMap.SetView(bounds);
        }
    }
}
