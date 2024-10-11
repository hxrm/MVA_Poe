using MVA_poe.Classes;
using MVA_Poe.Classes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MVA_poe.Pages
{
    /// <summary>
    /// Interaction logic for Events.xaml
    /// </summary>
    public partial class Events : Page
    {

        // List to store Events
        private SortedDictionary<DateTime, List<Event>> events = new  SortedDictionary<DateTime, List<Event>>();
        public Events()
        {
            InitializeComponent();
            SetLanguage(DBHelper.lang);
            // Retrieve data
            GetData();
        }

        // Method: GetData
        // Retrieves data from the database
        private void GetData()
        {

            // List to store report names
            List<String> eventNames = new List<String>();

            // Create a new instance of AppDbContext
            using (var context = new AppDbContext())
            {
                // Retrieve the events from the database
                var eventList = context.Events
                    .OrderBy(e => e.EventDate)
                    .ToList();

                // Add each event to the SortedDictionary
                foreach (var e in eventList)
                {
                    if (!events.ContainsKey(e.EventDate))
                    {
                        events[e.EventDate] = new List<Event>();
                    }
                    events[e.EventDate].Add(e);

                    // Add each event name to the eventNames list
                    eventNames.Add(e.EventName);
                }
            }
            // Set the report names as the item source for the ReportList
            EventList.ItemsSource = eventNames;
            //ELSES IF USER HAS RECOMMENDED EVENTS USE PRIORITY QUEUE
        }

        //----------------------------------------------------------------------------//
        private void SetLanguage(string cultureCode)
        {
            CultureInfo.CurrentUICulture = new CultureInfo(cultureCode);
            ResourceDictionary dict = new ResourceDictionary();
            switch (cultureCode)
            {
                case "af":
                    dict.Source = new Uri("Resources/Strings.af.xaml", UriKind.Relative);
                    break;
                case "isx":
                    dict.Source = new Uri("Resources/Strings.isx.xaml", UriKind.Relative);
                    break;
                default:
                    dict.Source = new Uri("Resources/Strings.en.xaml", UriKind.Relative);
                    break;
            }
            this.Resources.MergedDictionaries.Add(dict);
        }
        //----------------------------------------------------------------------------//

    }

}
//__---____---____---____---____---____---____---__.ooo END OF FILE ooo.__---____---____---____---____---____---____---__\\
