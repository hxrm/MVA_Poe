// Import necessary namespaces
using MVA_poe.Classes;
using MVA_poe.Data;
using MVA_Poe.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MVA_poe.Controls
{
    // Define the RecControl class, which is a UserControl
    public partial class RecControl : UserControl
    {
        // Private field to hold the recommendation engine instance
        private RecommendationEngine recommendationEngine;

        // Queue to store event recommendations
        public Queue<Event> events = new Queue<Event>();
        // Queue to store event recommendations
        public Queue<Event> eventsRelated = new Queue<Event>();

        // ObservableCollection to store EventCard items
        public ObservableCollection<EventCard> EventCardItems { get; set; }
        // ObservableCollection to store EventCard items
        public ObservableCollection<EventCard> EventRelatedItems { get; set; }
      

        // Constructor for the RecControl class
        // Takes a RecommendationEngine instance as a parameter
        public RecControl(RecommendationEngine rec)
        {
            // Initialize the components defined in the XAML file
            InitializeComponent();

            // Ensure DBHelper.lang is set to a valid value before calling SetLanguage
            if (string.IsNullOrEmpty(DBHelper.lang))
            {
                DBHelper.lang = "en"; // Default to English if no language is set
            }

            // Set the language based on the user's preference
            SetLanguage(DBHelper.lang);
            recommendationEngine = rec;

            // Initialize the EventCardItems collection
            EventCardItems = new ObservableCollection<EventCard>();
            // Initialize the EventCardItems collection
            EventRelatedItems = new ObservableCollection<EventCard>();

            //----------------------------------------------------------------------------

            // Load recommended events
            LoadRecommendedEvents();

        }

        //----------------------------------------------------------------------------

        // Property to get or set the recommendation header text
        public string RecHeader
        {
            get { return (string)GetValue(RecHeaderProperty); }
            set { SetValue(RecHeaderProperty, value); }
        }

        // Define a dependency property for the RecHeader
        public static readonly DependencyProperty RecHeaderProperty =
            DependencyProperty.Register("RecHeader", typeof(string), typeof(RecControl));

        // Property to get or set the recommendation header text
        public string RelHeader
        {
            get { return (string)GetValue(RelHeaderProperty); }
            set { SetValue(RelHeaderProperty, value); }
        }

        // Define a dependency property for the RecHeader
        public static readonly DependencyProperty RelHeaderProperty =
            DependencyProperty.Register("RelHeader", typeof(string), typeof(RecControl));

        //----------------------------------------------------------------------------

        // Method to load recommended events
        private void LoadRecommendedEvents()
        {
            // Initialize header and preference strings
            var header = Application.Current.Resources["LE_REC"] as string;
            var catString = "";
            var dayString = "";
            var concat1 = Application.Current.Resources["LE_CAT1"] as string;
            var concat2 = Application.Current.Resources["LE_CAT2"] as string;
            var concat3 = Application.Current.Resources["LE_CAT3"] as string;
            // Get the recommended events from the recommendation engine
            events = recommendationEngine.ReturnRecommendations();

            // Get the related events from the recommendation engine
            eventsRelated = recommendationEngine.ReturnRelatedEvents();

            // Get the category preferences from the recommendation engine
            var categoryPreferences = recommendationEngine.categoryPreference;

            // Get the day preferences from the recommendation engine
            var dayPreferences = recommendationEngine.dayPreference;

            // Build the category string
            if (categoryPreferences != null && categoryPreferences.Any())
            {
                catString = concat1+" "+  string.Join(", ", categoryPreferences.Select(cat => cat.GetString()));
            }

            // Build the day string
            if (dayPreferences != null && dayPreferences.Any())
            {
                dayString = concat2 + " " + string.Join(", ", dayPreferences);
            }

            // Combine the category and day strings
            var combinedString = "";
            if (!string.IsNullOrEmpty(catString) && !string.IsNullOrEmpty(dayString))
            {
                combinedString = $"{catString} {concat3} {dayString}";
            }
            else if (!string.IsNullOrEmpty(catString))
            {
                combinedString = catString;
            }
            else if (!string.IsNullOrEmpty(dayString))
            {
                combinedString = dayString;
            }
            else
            {
                combinedString = Application.Current.Resources["LE_NON"] as string;
            }

            // Add each recommended event to the EventCardItems collection and EventViewList
            foreach (Event ev in events)
            {
                var eventCard = new EventCard(ev);
                EventCardItems.Add(eventCard);
                EventViewList.Items.Add(eventCard);
            }
            // Add each recommended event to the EventCardItems collection and EventViewList
            foreach (Event ev in eventsRelated)
            {
                var eventCard = new EventCard(ev);
                EventCardItems.Add(eventCard);
                EventRelatedList.Items.Add(eventCard);
            }

            // Set the RecHeader with the combined string
            RecHeader = $"{header}... {combinedString}.";
            RelHeader = Application.Current.Resources["LE_REL"] as string;
            relHeader.Text = RelHeader;
            recHeader.Text = RecHeader;
        }

        //----------------------------------------------------------------------------

        // Method to set the language based on the provided culture code
        private void SetLanguage(string cultureCode)
        {
            if (string.IsNullOrEmpty(cultureCode))
            {
                cultureCode = "en"; // Default to English if no culture code is provided
            }

            // Set the current UI culture to the provided culture code
            CultureInfo.CurrentUICulture = new CultureInfo(cultureCode);

            // Create a new ResourceDictionary
            ResourceDictionary dict = new ResourceDictionary();

            // Set the source of the ResourceDictionary based on the culture code
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

            // Add the ResourceDictionary to the merged dictionaries of the control
            Application.Current.Dispatcher.Invoke(() =>
            {
                this.Resources.MergedDictionaries.Clear();
                this.Resources.MergedDictionaries.Add(dict);
            });

        }
    }
}//__---____---____---____---____---____---____---__.ooo END OF FILE ooo.__---____---____---____---____---____---____---__\\

