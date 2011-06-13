using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;

namespace Barefoot
{
    public partial class NewActivityPage : PhoneApplicationPage
    {
        GeoCoordinateWatcher watcher;
        private List<GeoPosition<GeoCoordinate>> _positions;
        private GeoCoordinate _lastPosition;
        
        public NewActivityPage()
        {
            InitializeComponent();

            this.PageTitle.Text = "Running"; // qsTitle;

            _positions = new List<GeoPosition<GeoCoordinate>>();
            watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High) { MovementThreshold = 0 };

            watcher.PositionChanged += this.watcher_PositionChanged;
            watcher.StatusChanged += this.watcher_StatusChanged;
            watcher.Start();
        }

        private void watcher_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            switch (e.Status)
            {
                case GeoPositionStatus.Disabled:
                // location is unsupported on this device
                break;
                case GeoPositionStatus.NoData:
                // data unavailable
                break;
            }
        }

        private void watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            var epl = e.Position.Location;
            _positions.Add(e.Position);

            // Access the position information thusly:
            epl.Latitude.ToString("0.00000");
            epl.Longitude.ToString("0.00000");
            epl.Altitude.ToString();
            epl.HorizontalAccuracy.ToString();
            epl.VerticalAccuracy.ToString();
            epl.Course.ToString();
            epl.Speed.ToString();
            e.Position.Timestamp.LocalDateTime.ToString();

            results.Text += e.Position.Timestamp.LocalDateTime.ToString() + "\nLatitude: " + epl.Latitude.ToString("0.00000") + "\nLongitude: " + epl.Longitude.ToString("0.00000") + "\nSpeed: " + epl.Speed + "\n\n";
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            var body = "";

            foreach (var position in _positions)
            {
                body += position.Timestamp.LocalDateTime + "\nLatitude: " + position.Location.Latitude.ToString("0.00000") + "\nLongitude: " +
                        position.Location.Longitude.ToString("0.00000") + "\nSpeed: " + position.Location.Speed +
                        "\nAltitude: " + position.Location.Altitude + "\nHorizontalAccuracy: " +
                        position.Location.HorizontalAccuracy + "\nVerticalAccuracy: " + position.Location.VerticalAccuracy +
                        "Course: " + position.Location.Course + "\n\n";

            }
            
            //Send email
            var task = new EmailComposeTask();
            task.To = "jason@jasongaylord.com";
            task.Body = body;
            task.Show();
        }

    }
}