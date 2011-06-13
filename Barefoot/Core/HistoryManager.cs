using System;
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
using Barefoot.Models;

namespace Barefoot.Core
{
    public class HistoryManager
    {
        public void WriteEntry(ActivityDetails activityDetails)
        {
            // Create the userHistory file if it doesn't exist
            IsolatedStorageFile userStore = IsolatedStorageFile.GetUserStoreForApplication();
            if (!userStore.FileExists("userHistory.xml"))
            {
                userStore.CreateFile("userHistory.xml");
            }

            // Read the XML into a document and keep only the last 100 entries
        }
    }
}
