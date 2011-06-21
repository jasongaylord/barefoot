using System;
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
using Barefoot.Models;
using gpx;

namespace Barefoot.Core
{
    public class Gpx
    {
        public void Save(ActivityDetails details)
        {
            var metadata = new metadataType();
            metadata.author = new personType();
            metadata.author.name = "barefoot for Windows Phone 7";
            metadata.link = new[] {new linkType() {href = "http://barefootapp.com", text="barefoot for Windows Phone 7"}};
            metadata.time = DateTime.UtcNow;

            var arrayLen = details.Coordinates.Count;
            var wpt = new wptType[arrayLen];
            var x = 0;

            foreach (var coord in details.Coordinates)
            {
                var trkpt = new wptType();
                trkpt.lat = Convert.ToDecimal(coord.Latitude);
                trkpt.lon = Convert.ToDecimal(coord.Longitude);
                trkpt.ele = Convert.ToDecimal(coord.Altitude);
                trkpt.hdop = Convert.ToDecimal(coord.HorizontalAccuracy);
                trkpt.vdop = Convert.ToDecimal(coord.VerticalAccuracy);
                trkpt.time = coord.TimeStamp;

                wpt[x] = trkpt;
                x++;
            }

            var trksegtype = new trksegType[1];
            trksegtype[0] = new trksegType();
            trksegtype[0].trkpt = new wptType[arrayLen];
            trksegtype[0].trkpt = wpt;

            // TODO: Convert the int activitytype to text
            var trk = new trkType();
            trk.name = details.ActivityType.ToString();
            trk.trkseg = trksegtype;

            var gps = new gpxType();
            gps.metadata = metadata;
            gps.trk = new trkType[1];
            gps.trk[0] = trk;

            IsolatedStorageFile userStore = IsolatedStorageFile.GetUserStoreForApplication();
            var writer = (!userStore.FileExists("test.xml"))
                             ? userStore.CreateFile("test.xml")
                             : userStore.OpenFile("test.xml", FileMode.Truncate);

            //var writer = new FileStream("test1.gpx", FileMode.Create);
            
            var xmlSerializer = new XmlSerializer(typeof(gpx.gpxType));
            xmlSerializer.Serialize(writer, gps);

            writer.Close();
        }
    }
}