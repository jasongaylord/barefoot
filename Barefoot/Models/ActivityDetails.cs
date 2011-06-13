using System;
using System.Collections.Generic;
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
    public class ActivityDetails
    {
        public List<Coordinate> Coordinates { get; set; }
        public TimeSpan TotalTime { get; set; }
        public DateTime TimeStarted { get; set; }
        public DateTime TimeCompleted { get; set; }
        public double TotalDistance { get; set; }
        public int ActivityType { get; set; }

        public ActivityDetails()
        {
            Coordinates = new List<Coordinate>();       
            TotalTime = new TimeSpan(0);
        }
    }
}
