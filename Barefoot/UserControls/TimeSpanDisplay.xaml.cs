using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace Barefoot
{
    public partial class TimeSpanDisplay : UserControl
    {
        int digitWidth;
        TimeSpan time;

        public TimeSpanDisplay()
        {
            InitializeComponent();
            // In design mode, show something other than an empty StackPanel
            if (DesignerProperties.IsInDesignTool)
                this.LayoutRoot.Children.Add(new TextBlock { Text = "0:00.0" });
        }

        public int DigitWidth
        {
            get { return this.digitWidth; }
            set
            {
                this.digitWidth = value;
                // Force a display update using the new width:
                this.Time = this.time;
            }
        }

        public TimeSpan Time
        {
            get { return this.time; }
            set
            {
                this.LayoutRoot.Children.Clear();
                // Carve out the appropriate digits and add each individually
                // Support an arbitrary # of minutes digits (with no leading 0)
                string minutesString = value.Minutes.ToString();
                for (int i = 0; i < minutesString.Length; i++)
                    AddDigitString(minutesString[i].ToString());
                this.LayoutRoot.Children.Add(new TextBlock { Text = ":" });
                // Seconds (always two digits, including a leading zero if necessary)
                AddDigitString((value.Seconds / 10).ToString());
                AddDigitString((value.Seconds % 10).ToString());
                // Add the decimal separator (a period for en-US)
                this.LayoutRoot.Children.Add(new TextBlock
                {
                    Text =
                        CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator
                });
                // The Remainder (always a single digit)
                AddDigitString((value.Milliseconds / 100).ToString());
                this.time = value;
            }
        }

        void AddDigitString(string digitString)
        {
            Border border = new Border { Width = this.DigitWidth };
            border.Child = new TextBlock
                           {
                               Text = digitString,
                               HorizontalAlignment = HorizontalAlignment.Center
                           };
            this.LayoutRoot.Children.Add(border);
        }
    }
}