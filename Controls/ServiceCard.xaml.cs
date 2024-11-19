using MVA_poe.Classes;
using MVA_poe.Data;
using MVA_Poe.Classes;
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

namespace MVA_poe.Controls
{
    /// <summary>
    /// Interaction logic for ServiceCard.xaml
    /// </summary>
    public partial class ServiceCard : UserControl
    {
        // Property to hold the event data
        public ServiceRequest ServiceData { get; private set; }
        public Brush StatNode { get; private set; }
        public Brush PriorNode { get; private set; }
      

        // Constructor for the ServiceCard class
        // Takes an Event object as a parameter
        public ServiceCard(ServiceRequest serviceData)
        {
            // Initialize the components defined in the XAML file
            InitializeComponent();

            // Set the EventData property to the provided event data
            ServiceData = serviceData;

            // Set the ServiceId property 
            ServiceId = serviceData.requestId;

            // Set the Priority property 
            RequestPriority = serviceData.requestPri.GetString();

            // Set the Status property 
            RequestStatus = serviceData.requestStat.ToString();

            // Set the ServiceCategory property
            ServiceCategory = ServiceData.report.reportCat.GetString();

            GetNodeColor(serviceData.requestStat, serviceData.requestPri);
            statNode.Fill = StatNode;    
            
            //set 
            txtPrior.Text = RequestPriority;
            txtStat.Text = RequestStatus;

        }

        //----------------------------------------------------------------------------

        // Property:ServiceId
        public int ServiceId
        {
            get { return (int)GetValue(ServiceIdProperty); }
            set { SetValue(ServiceIdProperty, value); }
        }

        // Define a dependency property for the ServiceId
        public static readonly DependencyProperty ServiceIdProperty =
            DependencyProperty.Register("ServiceId", typeof(int), typeof(ServiceCard));

        //----------------------------------------------------------------------------

        // Property: ServiceCategory
        public string ServiceCategory
        {
            get { return (string)GetValue(ServiceCategoryProperty); }
            set { SetValue(ServiceCategoryProperty, value); }
        }
        // Define a dependency property for the ServiceCategory
        public static readonly DependencyProperty ServiceCategoryProperty =
            DependencyProperty.Register("ServiceCategory", typeof(string), typeof(ServiceCard));

        //----------------------------------------------------------------------------

        // Property: Status
        public string RequestStatus
        {
            get { return (string)GetValue(StatusProperty); }
            set { SetValue(StatusProperty, value); }
        }

        // Define a dependency property for the Status
        public static readonly DependencyProperty StatusProperty =
            DependencyProperty.Register("Status", typeof(string), typeof(ServiceCard));

        //----------------------------------------------------------------------------

        // Property: Priority
        public string RequestPriority
        {
            get { return (string)GetValue(PriorityProperty); }
            set { SetValue(PriorityProperty, value); }
        }

        // Define a dependency property for the Priority
        public static readonly DependencyProperty PriorityProperty =
            DependencyProperty.Register("Priority", typeof(string), typeof(ServiceCard));

        //----------------------------------------------------------------------------

        // Event handler for when the border is clicked
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Finalize the session and analyze data
            DBHelper.FinalizeSessionAndAnalyzeData();

            // If EventData is not null, navigate to the ViewEvent page
            if (ServiceData != null)
            {
                //  var viewEventPage = new ViewEvent(ServiceData, image);
                //  NavigationService.GetNavigationService(this)?.Navigate(viewEventPage);
            }
        }

        //----------------------------------------------------------------------------
        // Method to get the color based on the status of the service request
        private void GetNodeColor(Status status, Priority priority)
        {

            switch (status)
            {
                case Status.Pending:
                    StatNode = Brushes.Orange;
                    break;
                case Status.Active:
                    StatNode = Brushes.GreenYellow;
                    break;
                case Status.Completed:
                    StatNode = Brushes.Blue;
                    break;
                default:
                    StatNode = Brushes.Gray;
                    break;
            }

            // Determine PriorNode based on priority
            switch (priority)
            {
                case Priority.High:
                    PriorNode = Brushes.Red;
                    break;
                case Priority.Medium:
                    PriorNode = Brushes.Orange;
                    break;
                case Priority.Low:
                    PriorNode = Brushes.Yellow;
                    break;
                default:
                    PriorNode = Brushes.Gray;
                    break;
            }
        }

    }
}//__---____---____---____---____---____---____---__.ooo END OF FILE ooo.__---____---____---____---____---____---____---__\\

