// Import necessary namespaces
using MVA_poe.Classes;
using MVA_poe.Data;
using MVA_poe.Pages;
using MVA_Poe.Controls;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace MVA_poe.Controls
{
    /// <summary>
    /// Interaction logic for EventCard.xaml
    /// </summary>
    public partial class EventCard : UserControl
    {
        // Property to hold the event data
        public Event EventData { get; private set; }

        // Property to hold the image for the event
        private BitmapImage image = null;

        // Static list of event categories
        private static readonly List<string> EventCategories = new List<string>
        {
            "MusicFestivals",
            "CommunityMeetings",
            "SportsEvents",
            "CulturalExhibitions",
            "HealthWellnessWorkshops",
            "CharityEvents",
            "EducationalSeminars",
            "FoodCraftMarkets",
            "LocalGovernmentAnnouncements",
            "Others"
        };

        // Constructor for the EventCard class
        // Takes an Event object as a parameter
        public EventCard(Event eventData)
        {
            // Initialize the components defined in the XAML file
            InitializeComponent();

            // Set the EventData property to the provided event data
            EventData = eventData;

            // Set the EventName property to the event's name
            EventName = eventData.EventName;

            // Set the EventDescription property to the event's description
            EventDescription = eventData.EventDesc;

            // Set the EventDate property to the event's date, formatted as "Day, Month Date"
            EventDate = eventData.EventDate.ToString("dddd, MMMM dd");

            // Set the EventCategory property to the event's category as a string
            EventCategory = EventData.EventCat.GetString();

            //----------------------------------------------------------------------------

            // Get the display settings for the event card based on the event category
            GetCardDisplay(EventCategory);
        }

        //----------------------------------------------------------------------------

        // Property: EventName
        public string EventName
        {
            get { return (string)GetValue(EventNameProperty); }
            set { SetValue(EventNameProperty, value); }
        }

        // Define a dependency property for the EventName
        public static readonly DependencyProperty EventNameProperty =
            DependencyProperty.Register("EventName", typeof(string), typeof(EventCard));

        //----------------------------------------------------------------------------

        // Property: EventCategory
        public string EventCategory
        {
            get { return (string)GetValue(EventCategoryProperty); }
            set { SetValue(EventCategoryProperty, value); }
        }

        // Define a dependency property for the EventCategory
        public static readonly DependencyProperty EventCategoryProperty =
            DependencyProperty.Register("EventCategory", typeof(string), typeof(EventCard));

        //----------------------------------------------------------------------------

        // Property: EventDate
        public string EventDate
        {
            get { return (string)GetValue(EventDateProperty); }
            set { SetValue(EventDateProperty, value); }
        }

        // Define a dependency property for the EventDate
        public static readonly DependencyProperty EventDateProperty =
            DependencyProperty.Register("EventDate", typeof(string), typeof(EventCard));

        //----------------------------------------------------------------------------

        // Property: EventDescription
        public string EventDescription
        {
            get { return (string)GetValue(EventDescriptionProperty); }
            set { SetValue(EventDescriptionProperty, value); }
        }

        // Define a dependency property for the EventDescription
        public static readonly DependencyProperty EventDescriptionProperty =
            DependencyProperty.Register("EventDescription", typeof(string), typeof(EventCard));

        //----------------------------------------------------------------------------

        // Property: BorderImageSource
        public ImageSource BorderImageSource
        {
            get { return (ImageSource)GetValue(BorderImageSourceProperty); }
            set { SetValue(BorderImageSourceProperty, value); }
        }

        // Define a dependency property for the BorderImageSource
        public static readonly DependencyProperty BorderImageSourceProperty =
            DependencyProperty.Register("BorderImageSource", typeof(ImageSource), typeof(EventCard));

        //----------------------------------------------------------------------------

        // Event handler for when the border is clicked
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Finalize the session and analyze data
            DBHelper.FinalizeSessionAndAnalyzeData();

            // If EventData is not null, navigate to the ViewEvent page
            if (EventData != null)
            {
                var viewEventPage = new ViewEvent(EventData, image);
                NavigationService.GetNavigationService(this)?.Navigate(viewEventPage);
            }
        }

        //----------------------------------------------------------------------------

        // Method to get the display settings for the event card based on the event category
        private void GetCardDisplay(string eventCategory)
        {
            // Declare a BitmapImage variable
            image = null;

            // Check the event category and set the corresponding image
            switch (eventCategory)
            {
                case "Music Festivals":
                    image = new BitmapImage(new Uri("pack://application:,,,/Resources/eve1.png"));
                    break;
                case "Community Meetings":
                    image = new BitmapImage(new Uri("pack://application:,,,/Resources/eve2.png"));
                    break;
                case "Sports Events":
                    image = new BitmapImage(new Uri("pack://application:,,,/Resources/eve3.png"));
                    break;
                case "Cultural Exhibitions":
                    image = new BitmapImage(new Uri("pack://application:,,,/Resources/eve4.png"));
                    break;
                case "Health & Wellness Workshops":
                    image = new BitmapImage(new Uri("pack://application:,,,/Resources/eve5.png"));
                    break;
                case "Charity Events":
                    image = new BitmapImage(new Uri("pack://application:,,,/Resources/eve6.png"));
                    break;
                case "Educational Seminars":
                    image = new BitmapImage(new Uri("pack://application:,,,/Resources/eve7.png"));
                    break;
                case "Food Craft Markets":
                    image = new BitmapImage(new Uri("pack://application:,,,/Resources/eve8.png"));
                    break;
                case "Local Government Announcements":
                    image = new BitmapImage(new Uri("pack://application:,,,/Resources/eve0.png"));
                    break;
                case "Others":
                    image = new BitmapImage(new Uri("pack://application:,,,/Resources/eve3.png"));
                    break;
                default:
                    image = new BitmapImage(new Uri("pack://application:,,,/Resources/eve1.png"));
                    break;
            }

            // Set the BorderImageSource property
            BorderImageSource = image;
        }
    }
}//__---____---____---____---____---____---____---__.ooo END OF FILE ooo.__---____---____---____---____---____---____---__\\

