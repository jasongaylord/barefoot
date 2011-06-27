using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Xml.Serialization;

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
        public Dictionary<DateTime, ActivitySummary> Collection { get; set; }
        //public ActivitySummary[] Collection { get; set; }
    }

    public class ActivitySummaryFile
    {
        private const string Filename = "UserHistory.xml";

        public ActivitySummaryCollection ReadFile()
        {
            var userStore = IsolatedStorageFile.GetUserStoreForApplication();

            if (userStore.FileExists(Filename))
            {
                var file = userStore.OpenFile(Filename, FileMode.Open);

                ActivitySummaryCollection activitySummaryCollection;

                var xmlSerializer = new XmlSerializer(typeof (ActivitySummaryCollection));
                activitySummaryCollection = (ActivitySummaryCollection)xmlSerializer.Deserialize(file);

                return activitySummaryCollection;
            }

            return new ActivitySummaryCollection();
        }

        public bool WriteFile(ActivitySummaryCollection activitySummaryCollection)
        {
            var rtn = true;

            try
            {
                var userStore = IsolatedStorageFile.GetUserStoreForApplication();
                var writer = (!userStore.FileExists(Filename))
                                 ? userStore.CreateFile(Filename)
                                 : userStore.OpenFile(Filename, FileMode.Truncate);
            
                var xmlSerializer = new XmlSerializer(typeof(ActivitySummaryCollection));
                xmlSerializer.Serialize(writer, activitySummaryCollection);

                writer.Close();
            }
            catch (Exception)
            {
                rtn = false;
                MessageBox.Show("Unfortunately, we're having trouble saving your history to memory. Try again later.",
                                "Error", MessageBoxButton.OK);
            }

            return rtn;
        }

        public bool AddRecentActivity(ActivitySummary activitySummary)
        {
            var userHistory = ReadFile();

            if (userHistory.Collection.Count > 49)
            {
                // TODO: Remove the last item and then insert this item
                //userHistory.Collection.
            }
        }
    }
}
