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
    public static class DecimalHelper
    {
        public static decimal TryParse(object value)
        {
            return value.ToString().ToLower() != "nan" ? (decimal) Convert.ToDecimal(value) : 0;
        }
    }
}
