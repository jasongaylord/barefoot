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

namespace Barefoot.Core
{
    public class ActivitySummary
    {
        public string ActivityType { get; set; }
        public double Distance { get; set; }
        public TimeSpan ElapsedTime { get; set; }
        public TimeSpan Pace { get; set; }
        public int Calories { get; set; }
        public double Altitude { get; set; }
    }

    public class ActivitySummaryCollection
    {
        public ActivitySummary[] Collection { get; set; }
    }
}
