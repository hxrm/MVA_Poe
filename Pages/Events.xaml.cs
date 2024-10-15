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
using System.Timers;
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
        // List to store Events, sorted by DateTime
        private SortedDictionary<DateTime, List<Event>> events = new SortedDictionary<DateTime, List<Event>>();

        // ObservableCollection to store EventCard items, used for data binding in the UI
        public ObservableCollection<EventCard> EventCardItems { get; set; }

        // Flag to indicate if a date search is being performed
        private bool searchDate = false;

      

        // Flag to indicate if a filter search is being performed
        bool filterSearch;

        // Constructor for the Events class
        public Events()
        {
            // Initialize the components defined in the XAML file
            InitializeComponent();

            // Set the language based on the user's preference
            SetLanguage(DBHelper.lang);

            // Initialize the EventCardItems collection
            EventCardItems = new ObservableCollection<EventCard>();

            // Retrieve data from the database
            GetData();

            // Create a new RecommendationEngine instance
            RecommendationEngine rObj = new RecommendationEngine();

            // If recommendations are enabled, display the recommendation control
            if (RecommendationEngine.recommendationEnabled)
            {
                // Create a new RecControl with the RecommendationEngine instance
                RecControl rc = new RecControl(rObj);

                // Set the content of the recommendation control
                recControl.Content = rc;

                // Hide the EventHolder control
                EventHolder.Visibility = Visibility.Collapsed;
            }
        }
        //----------------------------------------------------------------------------//
        // Method to handle a timed event
        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            // Check if the trackSearch list in DBHelper is not null and contains any items
            if (DBHelper.trackSearch != null && DBHelper.trackSearch.Any())
            {
                // Finalize the session and analyze the data
                DBHelper.FinalizeSessionAndAnalyzeData();
            }
        }
        //----------------------------------------------------------------------------//

        // Event handler for when the page is unloaded
        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            // Finalize the session and analyze the data
            DBHelper.FinalizeSessionAndAnalyzeData();
        }
        //----------------------------------------------------------------------------//

        // Method to close the recommendation control
        private void CloseRecomendation()
        {
            // Clear the content of the recommendation control
            recControl.Content = null;

            // Make the EventHolder control visible
            EventHolder.Visibility = Visibility.Visible;
        }
        //----------------------------------------------------------------------------//

        // Method to set the language of the application
        private void SetLanguage(string cultureCode)
        {
            // Set the current UI culture to the specified culture code
            CultureInfo.CurrentUICulture = new CultureInfo(cultureCode);

            // Create a new ResourceDictionary to hold the language resources
            ResourceDictionary dict = new ResourceDictionary();

            // Switch statement to load the appropriate resource file based on the culture code
            switch (cultureCode)
            {
                case "af":
                    // Load the Afrikaans resource file
                    dict.Source = new Uri("Resources/Strings.af.xaml", UriKind.Relative);
                    break;
                case "isx":
                    // Load the Icelandic resource file
                    dict.Source = new Uri("Resources/Strings.isx.xaml", UriKind.Relative);
                    break;
                default:
                    // Load the English resource file by default
                    dict.Source = new Uri("Resources/Strings.en.xaml", UriKind.Relative);
                    break;
            }

            // Add the loaded resource dictionary to the merged dictionaries of the current page
            this.Resources.MergedDictionaries.Add(dict);
        }

        //----------------------------------------------------------------------------//

        // Method: GetData
        // Retrieves data from the database
        private void GetData()
        {
            // Use a database context to access the database
            using (var db = new AppDbContext())
            {
                // Retrieve all events from the database and convert them to a list
                var eventList = db.Events.ToList();

                // Iterate through each event in the list
                foreach (Event ev in eventList)
                {
                    // Check if the event date already exists in the dictionary
                    if (events.ContainsKey(ev.EventDate))
                    {
                        // If the date exists, add the event to the existing list
                        events[ev.EventDate].Add(ev);
                    }
                    else
                    {
                        // If the date does not exist, create a new list with the event and add it to the dictionary
                        events.Add(ev.EventDate, new List<Event> { ev });
                    }
                }
            }
            // Populate the ComboBox with event categories
            PopulateComboBox();
        }
        //----------------------------------------------------------------------------//

        // Method: PopulateComboBox
        // Populates the ComboBox with event categories
        private void PopulateComboBox()
        {
            // Get the descriptions of the EventCategory enum values
            var categories = Enum.GetValues(typeof(EventCategory))
                                .Cast<EventCategory>()
                                .Select(e => e.GetString())
                                .ToList();

            // Set the categories as the item source for the ComboBox
            EventCB.ItemsSource = categories;

            // Populate the event list with the retrieved events
            PopulateEventList();
        }

        //----------------------------------------------------------------------------//

        // Method: PopulateEventList
        // Populates the event list with EventCard items
        private void PopulateEventList()
        {
            // Clear the existing items in the ObservableCollection and ListView
            EventCardItems.Clear();
            EventViewList.Items.Clear();

            // Iterate through each entry in the events dictionary
            foreach (KeyValuePair<DateTime, List<Event>> entry in events)
            {
                // Iterate through each event in the list of events for the current date
                foreach (Event ev in entry.Value)
                {
                    // Create a new EventCard for the event
                    var eventCard = new EventCard(ev);

                    // Add the EventCard to the ObservableCollection and the ListView
                    EventCardItems.Add(eventCard);
                    EventViewList.Items.Add(eventCard);
                }
            }
        }

        //----------------------------------------------------------------------------//
        // Method to handle the search button click event
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            

            // Close the recommendation control
            CloseRecomendation();

            // Get the search text from the TextBox
            string searchText = txtSearch.Text;

            // Get the selected category from the ComboBox
            var selectedCategory = EventCB.SelectedItem as string;

            // Initialize the category variable
            EventCategory? category = null;

            // Initialize flags for text, category, and date filters
            bool text = false;
            bool cat = false;
            bool date = false;

            // If a category is selected, get the corresponding EventCategory enum value
            if (!string.IsNullOrEmpty(selectedCategory))
            {
                category = Enum.GetValues(typeof(EventCategory))
                               .Cast<EventCategory>()
                               .FirstOrDefault(cate => cate.GetString() == selectedCategory);
            }

            // Get the selected start date
            DateTime? start = startDate.SelectedDate;

            // Get the selected end date
            DateTime? end = endDate.SelectedDate;

            // Flatten the dictionary to a list of events
            IEnumerable<Event> filteredEvents = events.SelectMany(entry => entry.Value);

            // Filter events by text if search text is provided
            if (!string.IsNullOrEmpty(searchText))
            {
                text = true;
                filteredEvents = FilterByText(filteredEvents, searchText);
            }

            // Filter events by category if a category is selected
            if (category.HasValue)
            {
                cat = true;
                filteredEvents = FilterByCategory(filteredEvents, category.Value);
            }

            // Filter events by date range if both start and end dates are selected
            if (start.HasValue && end.HasValue)
            {
                date = true;
                filteredEvents = FilterByDate(filteredEvents, start, end);
            }

            // If no events are found, display all events
            if (!filteredEvents.Any())
            {
                //filteredEvents = events.SelectMany(entry => entry.Value);
                // Show the default message
                DefaultMessage.Visibility = Visibility.Visible;

                // Hide the uploading files list                
                EventViewList.Visibility = Visibility.Collapsed;


            }

            // Clear the existing items in the ListView
            EventViewList.Items.Clear();

            // Clear the existing items in the ObservableCollection
            EventCardItems.Clear();

            // Add the filtered events to the ListView and ObservableCollection
            Save(text, cat, date, filteredEvents);
            foreach (var ev in filteredEvents)
            {
                var eventCard = new EventCard(ev);
                EventCardItems.Add(eventCard);
                EventViewList.Items.Add(eventCard);
            }
        }

        //----------------------------------------------------------------------------//
        // Method to save search records based on the applied filters
        private void Save(bool text, bool cat, bool date, IEnumerable<Event> filteredEvents)
        {
            // Get the selected start date
            DateTime? start = startDate.SelectedDate;

            // Get the selected end date
            DateTime? end = endDate.SelectedDate;

            // If text or category filter is applied and date filter is not applied
            if (text || cat && !date)
            {
                // Iterate through each filtered event
                foreach (var ev in filteredEvents)
                {
                    // Create a new SearchRecord with user ID, event category, and event date
                    SearchRecord r = new SearchRecord(DBHelper.userID, ev.EventCat, ev.EventDate);

                    // Add the SearchRecord to the trackSearch list
                    DBHelper.trackSearch.Add(r);
                }
            }
            // If date filter is applied and both start and end dates are selected
            else if (date && start.HasValue && end.HasValue)
            {
                // Iterate through each filtered event
                foreach (var ev in filteredEvents)
                {
                    // Create a new SearchRecord with user ID, start date, and end date
                    SearchRecord r = new SearchRecord(DBHelper.userID, start.Value, end.Value);

                    // Add the SearchRecord to the trackSearch list
                    DBHelper.trackSearch.Add(r);
                }
            }
        }

      
        //----------------------------------------------------------------------------//
        // Event handler for when the start date calendar is closed
        private void startDate_CalendarClosed(object sender, RoutedEventArgs e)
        {
            // If the end date is selected and the start date is greater than the end date
            if (endDate.SelectedDate != null && startDate.SelectedDate > endDate.SelectedDate)
            {
                // Show a message box indicating the error
                MessageBox.Show("Start date cannot be greater than end date");

                // Clear the selected start date
                startDate.SelectedDate = null;
                return;
            }

           
        }

        //----------------------------------------------------------------------------//
        // Event handler for when the end date calendar is closed
        private void endDate_CalendarClosed(object sender, RoutedEventArgs e)
        {
            // If the start date is selected and the start date is greater than the end date
            if (startDate.SelectedDate != null && startDate.SelectedDate > endDate.SelectedDate)
            {
                // Show a message box indicating the error
                MessageBox.Show("End date cannot be less than start date");

                // Clear the selected end date
                endDate.SelectedDate = null;
                return;
            }
            // If the start date is not selected
            else if (startDate.SelectedDate == null)
            {
                // Show a message box indicating the error
                MessageBox.Show("Please select a start date first");

                // Clear the selected end date
                endDate.SelectedDate = null;
                return;
            }

        }


        //----------------------------------------------------------------------------//
        // Event handler for the reset button click event
        private void resetBtn_Click(object sender, RoutedEventArgs e)
        {
           //set bool to false
            searchDate = false;

            // Clear the selected item in the ComboBox
            EventCB.SelectedItem = null;

            // Clear the selected start and end dates
            startDate.SelectedDate = null;
            endDate.SelectedDate = null;

            // Clear the search text
            txtSearch.Text = "";
            // Repopulate the event list with all events

            if (EventViewList.Visibility == Visibility.Collapsed)
            {
                EventViewList.Visibility = Visibility.Visible;
                DefaultMessage.Visibility = Visibility.Collapsed;
            }

            PopulateEventList();
        }
        //----------------------------------------------------------------------------//

        // Event handler for the filter button click event
        private void filterBtn_Click(object sender, RoutedEventArgs e)
        {
            // Set the filter search flag to true
            filterSearch = true;

            // Toggle visibility for the EventCB ComboBox
            EventCB.Visibility = EventCB.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;

            // Toggle visibility for the dateGrid
            dateGrid.Visibility = dateGrid.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }
        

        //----------------------------------------------------------------------------//
        // Event handler for the text changed event in the search TextBox
        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Repopulate the event list with all events
            PopulateEventList();
        }
       

       
        //----------------------------------------------------------------------------//
        // Method to filter events by text
        private IEnumerable<Event> FilterByText(IEnumerable<Event> events, string searchText)
        {
            // Return events that contain the search text in their name or description
            return events.Where(ev => ev.EventName.Contains(searchText) || ev.EventDesc.Contains(searchText));
        }
       
        //----------------------------------------------------------------------------//
        // Method to filter events by category
        private IEnumerable<Event> FilterByCategory(IEnumerable<Event> events, EventCategory? category)
        {
            // Return events that match the specified category
            return events.Where(ev => ev.EventCat == category);
        }
        
        //----------------------------------------------------------------------------//
        // Method to filter events by date range
        private IEnumerable<Event> FilterByDate(IEnumerable<Event> events, DateTime? startDate, DateTime? endDate)
        {
            // Return events that fall within the specified date range
            return events.Where(ev => ev.EventDate >= startDate && ev.EventDate <= endDate);
        }

        //----------------------------------------------------------------------------//

    }

}
//__---____---____---____---____---____---____---__.ooo END OF FILE ooo.__---____---____---____---____---____---____---__\\
