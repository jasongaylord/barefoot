using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Windows;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Threading;
using Barefoot.Core;
using Barefoot.Models;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Controls.Maps;
using Microsoft.Phone.Tasks;

namespace Barefoot
{
    public partial class TrackActivityPage : PhoneApplicationPage
    {
        #region Private Variables
        private DispatcherTimer _dispatcherTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(.1) };
        private GeoCoordinateWatcher _geoCoordinateWatcher;
        private ActivityDetails _activityDetails;
        private Coordinate _lastKnownCoordinate;
        private DateTime _previousTick;
        private double _distance;
        #endregion

        #region Constructor
        public TrackActivityPage()
        {
            InitializeComponent();

            _activityDetails = new ActivityDetails();

            _geoCoordinateWatcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High) { MovementThreshold = 0 };
            _geoCoordinateWatcher.PositionChanged += this._geoCoordinateWatcher_PositionChanged;
            _geoCoordinateWatcher.StatusChanged += this._geoCoordinateWatcher_StatusChanged;
            _lastKnownCoordinate = null;
        }
        #endregion

        #region Page Events
        protected override void OnNavigatedTo(NavigationEventArgs args)
        {
            IDictionary<string, string> parameters = this.NavigationContext.QueryString;
            if (parameters.ContainsKey("activity"))
            {
                var activity = parameters["activity"];
                TitleBlock.Title = "barefoot ~ " + activity;
            }
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            ////TODO: Save this information in the application history. Only keep the last 100 entries.
            ////TODO: Also, check to see if the back button was pressed so that info is saved.

            //base.OnNavigatedFrom(e);
            //var body = "";
            //var body2 = "";
            //var totalDistance = "";

            //body += "Total Time: " + _activityDetails.TotalTime + "\n";

            //foreach (var coord in _activityDetails.Coordinates)
            //{
            //    body2 += coord.TimeStamp.ToLocalTime() + "\nLatitude: " + coord.Latitude.ToString("0.00000") + "\nLongitude: " +
            //            coord.Longitude.ToString("0.00000") + "\nSpeed: " + coord.Speed +
            //            "\nAltitude: " + coord.Altitude + "\nTotalDistance: " + coord.TotalDistance + "\n\n";
            //    totalDistance = "Total Distance: " + coord.TotalDistance + "\n\n";
            //}          

            ////Send email
            //var task = new EmailComposeTask();
            //task.To = "jason@jasongaylord.com";
            //task.Body = body + totalDistance + body2;
            //task.Show();
        }
        #endregion

        #region Button Event
        private void startStopButton_Click(object sender, RoutedEventArgs e)
        {
            if (startStopButton.Content.ToString().ToLower() == "start")
            {
                _lastKnownCoordinate = null;
                startStopButton.Content = "Pause";
                _activityDetails.TimeStarted = DateTime.UtcNow;
                _previousTick = DateTime.UtcNow;
                _dispatcherTimer.Tick += _dispatcherTimer_Tick;

                _geoCoordinateWatcher.Start();
                _dispatcherTimer.Start();
            }
            else
            {
                _dispatcherTimer.Stop();
                _geoCoordinateWatcher.Stop();
                _lastKnownCoordinate = null;
                startStopButton.Content = "Start";
                _activityDetails.TimeCompleted = DateTime.UtcNow;


                emailButton.Visibility = Visibility.Visible;
                // Write Historical Entry

            }
        }

        private void emailButton_Click(object sender, RoutedEventArgs e)
        {
            var distance = _activityDetails.Coordinates[_activityDetails.Coordinates.Count - 1].TotalDistance;

            var body = "";
            var body2 = "";
            var totalDistance = "";

            body += "Total Time: " + _activityDetails.TotalTime + "\n";

            //foreach (var coord in _activityDetails.Coordinates)
            //{
            //    body2 += coord.TimeStamp.ToLocalTime() + "\nLatitude: " + coord.Latitude.ToString("0.00000") + "\nLongitude: " +
            //            coord.Longitude.ToString("0.00000") + "\nSpeed: " + coord.Speed +
            //            "\nAltitude: " + coord.Altitude + "\nTotalDistance: " + coord.TotalDistance + "\n\n";
            //}

            totalDistance = "Total Distance: " + distance + "\n\n";
            _activityDetails.TotalDistance = distance;

            //Send email
            var task = new EmailComposeTask();
            task.To = "jason@jasongaylord.com";
            task.Body = body + totalDistance + body2;
            task.Show();
        }
        #endregion

        #region Timer Event
        private void _dispatcherTimer_Tick(object sender, EventArgs e)
        {
            var delta = DateTime.UtcNow - _previousTick;
            _activityDetails.TotalTime += delta;
            stopWatchDisplay.Time = delta;
            if (_distance > 0)
            {
                var secondsPerMile = new TimeSpan(0, 0, 0, System.Convert.ToInt32(delta.TotalSeconds / _distance));
                //speedTextBox.Text = secondsPerMile.ToString("'mm':'ss'");
                speedTextBox.Text = secondsPerMile.ToString();
                //speedTextBox.Text = delta.ToString();
                //stopWatchText.Text = _activityDetails.TotalTime.ToString();
            }
        }
        #endregion
        
        #region GeoCordinateWatcher Events
        private void _geoCoordinateWatcher_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
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

        private void _geoCoordinateWatcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            var epl = e.Position.Location;

            var coord = new Coordinate();
            coord.Altitude = epl.Altitude;
            coord.Course = epl.Course;
            coord.HorizontalAccuracy = epl.HorizontalAccuracy;
            coord.Latitude = epl.Latitude;
            coord.Longitude = epl.Longitude;
            coord.Speed = epl.Speed;
            coord.TimeStamp = e.Position.Timestamp.LocalDateTime;
            coord.VerticalAccuracy = epl.VerticalAccuracy;
            // Calculate distance
            if (_lastKnownCoordinate == null)
            {
                coord.DistanceFromLastCall = 0;
                coord.TotalDistance = 0;
            } else {
                coord.DistanceFromLastCall = Core.Distance.Calculate(_lastKnownCoordinate, coord);
                coord.TotalDistance = _lastKnownCoordinate.TotalDistance + coord.DistanceFromLastCall;
            }

            // Add coordinate and set last known
            _distance = coord.TotalDistance;
            _lastKnownCoordinate = coord;
            _activityDetails.Coordinates.Add(coord);

            // Update UI
            distanceTextBox.Text = coord.TotalDistance.ToString();
        }
        #endregion

        #region Map - Drawing
        private void TitleBlock_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (((PivotItem)TitleBlock.SelectedItem).Header.ToString().ToLower() == "map")
            {
                // Determine if there are locations to show.
                if (_activityDetails.Coordinates.Count > 0)
                {
                    // Get the locations
                    var _locations = new LocationCollection();
                    foreach (var coordinate in _activityDetails.Coordinates)
                    {
                        _locations.Add(new GeoCoordinate(coordinate.Latitude, coordinate.Longitude));
                    }

                    Color routeColor = Colors.Blue;
                    SolidColorBrush routeBrush = new SolidColorBrush(routeColor);
                    MapPolyline routeLine = new MapPolyline();
                    routeLine.Locations = _locations;
                    routeLine.Stroke = routeBrush;
                    routeLine.Opacity = 0.65;
                    routeLine.StrokeThickness = 5.0;

                    // Change zoom level and set the current point to be the last point
                    courseMap.Center = _locations[_locations.Count - 1];
                    courseMap.ZoomLevel = 10;

                    // Add a map layer in which to draw the route.
                    MapLayer myRouteLayer = new MapLayer();
                    courseMap.Children.Add(myRouteLayer);

                    // Add the route line to the new layer.
                    myRouteLayer.Children.Add(routeLine);
                }
                else
                {
                    //GeoCoordinate location = new GeoCoordinate();
                    //var immediate = new ImmediateLocation(x => location = x);
                    //immediate.GetLocation();

                    //// Change zoom level and set the current point to be the last point
                    //courseMap.Center = location;
                    courseMap.ZoomLevel = 10;
                }
            }
        }
        #endregion
    }
}