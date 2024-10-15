// Import necessary namespaces
using MVA_poe.Classes;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for ViewEvent.xaml
    /// </summary>
    public partial class ViewEvent : Page
    {
        // Constructor for the ViewEvent class
        // Takes an Event object and a BitmapImage as parameters
        public ViewEvent(Event selectedEvent, BitmapImage image)
        {
            // Initialize the components defined in the XAML file
            InitializeComponent();

            //----------------------------------------------------------------------------

            // Use the selectedEvent to populate the UI elements
            // Set the text of EventNameTextBlock to the name of the event
            EventNameTextBlock.Text = selectedEvent.EventName;

            // Set the text of EventDescriptionTextBlock to the description of the event
            EventDescriptionTextBlock.Text = selectedEvent.EventDesc;

            // Set the text of EventDateTextBlock to the date of the event, formatted as a general date/time pattern
            EventDateTextBlock.Text = selectedEvent.EventDate.ToString("g");

            // Set the text of EventLocationTextBlock to the location of the event
            EventLocationTextBlock.Text = selectedEvent.EventLoc;

            // Set the text of EventCategoryTextBlock to the category of the event, converted to a string
            EventCategoryTextBlock.Text = selectedEvent.EventCat.ToString();

            // Set the source of EventImage to the provided BitmapImage
            EventImage.Source = image;
        }
    }
}
