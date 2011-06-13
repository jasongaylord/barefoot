using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Barefoot.Models
{
    public class Coordinate
    {
        public double Latitude { get; set; }
        public double Longitude { get; set;  }
        public double Altitude { get; set; }
        public double Course { get; set;  }
        public double Speed { get; set;  }
        public double HorizontalAccuracy { get; set; }
        public double VerticalAccuracy { get; set; }
        public double TotalDistance { get; set; }
        public double DistanceFromLastCall { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
