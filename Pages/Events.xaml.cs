using MVA_poe.Classes;
using MVA_poe.Controls;
using MVA_poe.Data;
using MVA_Poe.Classes;
using MVA_Poe.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
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
        public ObservableCollection<EventCard> EventCardItems { get; set; }
        private bool searchDate = false;
        private bool inSearch = false;
        public RecordPattern trackSearch = new RecordPattern();
        // Initialize the AttachListItems collection
    
        public Events()
        {
            InitializeComponent();
            SetLanguage(DBHelper.lang);
            EventCardItems = new ObservableCollection<EventCard>();
            // Retrieve data
            GetData();
            
       
        }
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


        // Method: GetData
        // Retrieves data from the database
        private void GetData()
        {
            // populate the dictionay with the events in database 
            using (var db = new AppDbContext())
            {
                var eventList = db.Events.ToList();
                foreach (Event ev in eventList)
                {
                    if (events.ContainsKey(ev.EventDate))
                    {
                        events[ev.EventDate].Add(ev);
                    }
                    else
                    {
                        events.Add(ev.EventDate, new List<Event> { ev });
                    }
                }
            }
            PopulateComboBox();
        }
        private void PopulateComboBox()
        {

            // Populate the ComboBox with the descriptions of the EventCategory enum values
            var categories = Enum.GetValues(typeof(EventCategory))
                                .Cast<EventCategory>()
                                .Select(e => e.GetString())
                                .ToList();
            // Set the report names as the item source for the ReportList
            EventCB.ItemsSource = categories;

            //ELSES IF USER HAS RECOMMENDED EVENTS USE PRIORITY QUEUE
            PopulateEventList();
        }
        private void PopulateEventList()
        {
            // Clear the existing items
            EventCardItems.Clear();
            EventViewList.Items.Clear();

            // Iterate through each event in the dictionary
            foreach (KeyValuePair<DateTime, List<Event>> entry in events)
            {
                foreach (Event ev in entry.Value)
                {
                    // Create a new EventCard for each event
                    var eventCard = new EventCard(ev);

                    // Add the EventCard to the ObservableCollection and the ListView
                    EventCardItems.Add(eventCard);
                    EventViewList.Items.Add(eventCard);
                }
            }
        }
        private IEnumerable<Event> FilterByText(IEnumerable<Event> events, string searchText)
        {
            return events.Where(ev => ev.EventName.Contains(searchText) || ev.EventDesc.Contains(searchText));
        }

        private IEnumerable<Event> FilterByCategory(IEnumerable<Event> events, EventCategory? category)
        {
            return events.Where(ev => ev.EventCat == category);
        }

        private IEnumerable<Event> FilterByDate(IEnumerable<Event> events, DateTime? startDate, DateTime? endDate)
        {
            return events.Where(ev => ev.EventDate >= startDate && ev.EventDate <= endDate);
        }


        //----------------------------------------------------------------------------//
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            inSearch = true;
            string searchText = txtSearch.Text;
            var selectedCategory = EventCB.SelectedItem as string;
            EventCategory? category = null;

            if (!string.IsNullOrEmpty(selectedCategory))
            {
                category = Enum.GetValues(typeof(EventCategory))
                               .Cast<EventCategory>()
                               .FirstOrDefault(cat => cat.GetString() == selectedCategory);
            }

            DateTime? start = startDate.SelectedDate;
            DateTime? end = endDate.SelectedDate;

            IEnumerable<Event> filteredEvents = events.SelectMany(entry => entry.Value);

            if (!string.IsNullOrEmpty(searchText))
            {
                filteredEvents = FilterByText(filteredEvents, searchText);
            }

            if (category.HasValue)
            {
                filteredEvents = FilterByCategory(filteredEvents, category.Value);
                trackSearch.RecordSearchCatergory(category.Value);
            }

            if (start.HasValue && end.HasValue)
            {
                filteredEvents = FilterByDate(filteredEvents, start, end);
                trackSearch.RecordSearchDateRange(start.Value);
                trackSearch.RecordSearchDateRange(end.Value);
            }
            // Check if no events are found
            if (!filteredEvents.Any())
            {
                // If no events are found, display all events
                filteredEvents = events.SelectMany(entry => entry.Value);
            }

            EventViewList.Items.Clear();
            EventCardItems.Clear();

            foreach (var ev in filteredEvents)
            {
                var eventCard = new EventCard(ev);
                EventCardItems.Add(eventCard);
                EventViewList.Items.Add(eventCard);
            }
        }
        //----------------------------------------------------------------------------//    

       
        private void DateCatSearch(EventCategory selectedCategory)
        {
            EventViewList.Items.Clear();
            EventCardItems.Clear();
            foreach (KeyValuePair<DateTime, List<Event>> entry in events)
            {
                foreach (Event ev in entry.Value)
                {
                    if (ev.EventCat == selectedCategory && ev.EventDate >= startDate.SelectedDate && ev.EventDate <= endDate.SelectedDate)
                    {
                        var eventCard = new EventCard(ev);
                        EventCardItems.Add(eventCard);
                        EventViewList.Items.Add(eventCard);
                        trackSearch.RecordSearchCatergory(ev.EventCat);
                        trackSearch.RecordSearchDateRange(ev.EventDate);
                        RecordDateRaneg();
                    }
                }
            }
            return;
        }

        private void EventCB_DropDownClosed(object sender, EventArgs e)
        {
            if (inSearch)
            {
                btnSearch_Click(sender, new RoutedEventArgs());
            }
        }

        private void startDate_CalendarClosed(object sender, RoutedEventArgs e)
        {
            if (endDate.SelectedDate != null && startDate.SelectedDate > endDate.SelectedDate)
            {
                MessageBox.Show("Start date cannot be greater than end date");
                startDate.SelectedDate = null;
                return;
            }

            if (inSearch)
            {
                btnSearch_Click(sender, e);
            }
        }

        private void endDate_CalendarClosed(object sender, RoutedEventArgs e)
        {
            if (startDate.SelectedDate != null && startDate.SelectedDate > endDate.SelectedDate)
            {
                MessageBox.Show("End date cannot be less than start date");
                endDate.SelectedDate = null;
                return;
            }
            else if (startDate.SelectedDate == null)
            {
                MessageBox.Show("Please select a start date first");
                endDate.SelectedDate = null;
                return;
            }

            if (inSearch)
            {
                btnSearch_Click(sender, e);
            }
        }



        private void resetBtn_Click(object sender, RoutedEventArgs e)
        {
            inSearch = false;
            searchDate = false;
            EventCB.SelectedItem = null;
            startDate.SelectedDate = null;
            endDate.SelectedDate = null;
            txtSearch.Text = "";
            PopulateEventList();

        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
         //   inSearch = true;
            PopulateEventList();
        }
        private void RecordDateRaneg()
        {
            if (startDate.SelectedDate != null && endDate.SelectedDate != null)
            {
               // get the all dates between range 
               DateTime start = startDate.SelectedDate.Value;
                DateTime end = endDate.SelectedDate.Value;
                for (DateTime date = start; date <= end; date = date.AddDays(1))
                {
                   trackSearch.RecordSearchDateRange(date);
                }
            }        

        }

        //----------------------------------------------------------------------------//

    }

}
//__---____---____---____---____---____---____---__.ooo END OF FILE ooo.__---____---____---____---____---____---____---__\\
