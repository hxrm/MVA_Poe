using MVA_poe.Classes;
using MVA_poe.Pages;
using MVA_Poe.Controls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace MVA_poe.Controls
{
    /// <summary>
    /// Interaction logic for EventCard.xaml
    /// </summary>
    public partial class EventCard : UserControl
    {
        public Event EventData { get; private set; }

        public EventCard(Event eventData)
        {
            InitializeComponent();
            EventData = eventData;

            EventName = eventData.EventName;
            EventCategory = eventData.EventCat.ToString();
            EventDescription = eventData.EventDesc;
        }

        // Property: EventName
        public string EventName
        {
            get { return (string)GetValue(EventNameProperty); }
            set { SetValue(EventNameProperty, value); }
        }

        public static readonly DependencyProperty EventNameProperty =
            DependencyProperty.Register("EventName", typeof(string), typeof(EventCard));

        // Property: EventCategory
        public string EventCategory
        {
            get { return (string)GetValue(EventCategoryProperty); }
            set { SetValue(EventCategoryProperty, value); }
        }

        public static readonly DependencyProperty EventCategoryProperty =
            DependencyProperty.Register("EventCategory", typeof(string), typeof(EventCard));

        // Property: EventDescription
        public string EventDescription
        {
            get { return (string)GetValue(EventDescriptionProperty); }
            set { SetValue(EventDescriptionProperty, value); }
        }

        public static readonly DependencyProperty EventDescriptionProperty =
            DependencyProperty.Register("EventDescription", typeof(string), typeof(EventCard));

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DBHelper.FinalizeSessionAndAnalyzeData();

            if (EventData != null)
            {
                var viewEventPage = new ViewEvent(EventData);
                NavigationService.GetNavigationService(this)?.Navigate(viewEventPage);
            }
        }
    }
}
