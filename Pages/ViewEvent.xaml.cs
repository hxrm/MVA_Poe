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
        public ViewEvent(Event selectedEvent)
        {
            InitializeComponent();
            // Use the selectedEvent to populate the UI elements
            // For example:
            EventNameTextBlock.Text = selectedEvent.EventName;
            EventDescriptionTextBlock.Text = selectedEvent.EventDesc;
            EventDateTextBlock.Text = selectedEvent.EventDate.ToString("g");
            EventLocationTextBlock.Text = selectedEvent.EventLoc;
            EventCategoryTextBlock.Text = selectedEvent.EventCat.ToString();
        }
    }
}
