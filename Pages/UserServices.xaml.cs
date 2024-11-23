using MVA_poe.Classes.SearchManagment;
using MVA_poe.Classes;
using MVA_poe.Data;
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
using MVA_Poe.Classes;

namespace MVA_poe.Pages
{
    /// <summary>
    /// Interaction logic for UserServices.xaml
    /// </summary>
    public partial class UserServices : Page
    {    // Declare fields
        private AVLTree<ServiceRequest> requestTree = new AVLTree<ServiceRequest>();
        private List<ServiceRequest> serviceRequests;
        private ServiceRequestManager sm;

        // Constructor
        public UserServices()
        {
            InitializeComponent();
            // Set the language based on the user's preference
            SetLanguage(DBHelper.lang);
            // instialize the service request list
            serviceRequests = new List<ServiceRequest>();

            // Retrieve data from the database
            GetData();
        }
        //----------------------------------------------------------------------------//

        // Method: VisualizeDependencies_Click
        // Navigates to the DependencyVisual page when the button is clicked
        private void VisualizeDependencies_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new DependencyVisual());
        }
        //----------------------------------------------------------------------------//

        // Method: SetLanguage
        // Sets the language for the application based on the culture code
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

        // Method: GetData
        // Retrieves data from the database and populates the AVL tree 
        private void GetData()
        {
            sm = new ServiceRequestManager();
            sm.GetAVL();
            requestTree = ServiceRequestManager.avlTree; 
            serviceRequests = requestTree.GetSortedServiceRequests();
            PopulateServiceList();           
            DisplayServiceRequests();
        }
        private void PopulateServiceList()
        {
            List<ServiceRequest> toRemove = new List<ServiceRequest>();
            //foreach service create display object and add to display grid
            foreach (var request in serviceRequests)
            {                
                if (request.report.userId != DBHelper.userID)
                {
                    toRemove.Add(request); // Don't modify the collection here
                }

            }
            foreach (var request in toRemove)
            {
                serviceRequests.Remove(request);
            }
        }
        private List<ServiceRequest> GetServiceRequests(List<MVA_Poe.Classes.ServiceRequest> serviceRequests)
        {
            List<ServiceRequest> toRemove = new List<ServiceRequest>();
            //foreach service create display object and add to display grid
            foreach (var request in serviceRequests)
            {
                if (request.report.userId != DBHelper.userID)
                {
                    toRemove.Add(request); // Don't modify the collection here
                }

            }
            foreach (var request in toRemove)
            {
                serviceRequests.Remove(request);
            }
            return serviceRequests;
        }
        //----------------------------------------------------------------------------//

        // Method: SearchServiceRequestById
        // Searches for service requests by ID
        private IEnumerable<ServiceRequest> SearchServiceRequestById(int requestId)
        {
            // var serviceRequests = requestTree.InOrderTraversal();
            var serviceRequests = requestTree.GetSortedServiceRequests();
            return serviceRequests
               .Where(sr => sr.requestId.ToString().IndexOf(requestId.ToString()) >= 0)
               .ToList();
        }
        //----------------------------------------------------------------------------//

        // Method: SearchServiceRequestsByPriority
        // Searches for service requests by priority
        private IEnumerable<ServiceRequest> SearchServiceRequestsByPriority(string priority)
        {
            var serviceRequests = requestTree.GetSortedServiceRequests();
            return serviceRequests
                .Where(sr => sr.requestPri.GetString().IndexOf(priority, StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();
        }
        //----------------------------------------------------------------------------//

        // Method: SearchServiceRequestsByStatus
        // Searches for service requests by status
        private IEnumerable<ServiceRequest> SearchServiceRequestsByStatus(string status)
        {
            //  var serviceRequests = requestTree.InOrderTraversal();
            var serviceRequests = requestTree.GetSortedServiceRequests();
            return serviceRequests
                .Where(sr => sr.requestStat.ToString().IndexOf(status, StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();
        }
        //----------------------------------------------------------------------------//

        // Method: DisplayServiceRequests
        // Displays the service requests in the data grid
        private void DisplayServiceRequests()
        {
            dataGrid.Items.Clear();
            foreach (var request in serviceRequests)
            {
                var displayRequest = GetDisplayItem(request);
                dataGrid.Items.Add(displayRequest);
            }
        }
        //----------------------------------------------------------------------------//

        // Method: GetDisplayItem
        // Converts a ServiceRequest to a DisplayRequest for display in the data grid
        private DisplayRequest GetDisplayItem(ServiceRequest request)
        {
            var displayRequest = new DisplayRequest
            {
                requestId = request.requestId,
                reportTitle = request.report?.reportName ?? "No Report",
                status = request.requestStat.ToString(),
                priority = request.requestPri.GetString(),
                category = request.report?.reportCat.GetString() ?? "No Category",
                date = request.requestUpdate.ToShortDateString(),
                employee = GetEmployee(request.employeeId)
            };

            return displayRequest;
        }
        //----------------------------------------------------------------------------//

        // Method: GetEmployee
        // Retrieves the employee name based on the employee ID
        private string GetEmployee(int employeeId)
        {// Cast the employeeId to the Employee enum type
            Employee employeeEnumValue = (Employee)employeeId;

            // Check if the casted value is defined in the Employee enum
            if (Enum.IsDefined(typeof(Employee), employeeEnumValue))
            {
                // Get the description of the enum value
                return employeeEnumValue.GetString();
            }

            // Return null if the employeeId is not defined in the Employee enum
            return null;
        }
        //----------------------------------------------------------------------------//

        // Method: UserRequestsButton_Click
        // Displays service requests in priority order when the button is clicked
        private void ServiceButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Service());
        }
        //----------------------------------------------------------------------------//

        // Method: PriorizeButton_Click
        // Displays service requests in priority order when the button is clicked
        private void PriorizeButton_Click(object sender, RoutedEventArgs e)
        {
            var srList = sm.ToPriorityArrayUsingAVL();
            dataGrid.Items.Clear();
            serviceRequests = GetServiceRequests(srList.ToList());
            //foreach service create display object and add to display grid
            foreach (var request in serviceRequests)
            {
                var displayRequest = GetDisplayItem(request);
                dataGrid.Items.Add(displayRequest);
            }            
        }
        //----------------------------------------------------------------------------//

        // Method: OrderByStatusButton_Click
        // Displays service requests in priority order when the button is clicked
        private void StatusButton_Click(object sender, RoutedEventArgs e)
        {
            var srList = sm.ToStatusArrayUsingAVL();
            dataGrid.Items.Clear();
            serviceRequests = GetServiceRequests(srList.ToList());
            //foreach service create display object and add to display grid
            foreach (var request in serviceRequests)
            {
                var displayRequest = GetDisplayItem(request);
                dataGrid.Items.Add(displayRequest);
            }
        }
        //----------------------------------------------------------------------------//

        // Method: SearchButton_Click
        // Searches for service requests based on the selected search type and search text
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            var searchType = (searchTypeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            var searchText = searchTextBox.Text;

            if (string.IsNullOrEmpty(searchType) || string.IsNullOrEmpty(searchText))
            {
                MessageBox.Show("Please select a search type and enter a search value.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            dataGrid.Items.Clear();

            switch (searchType)
            {
                case "ID":
                    if (int.TryParse(searchText, out int requestId))
                    {
                        var results = SearchServiceRequestById(requestId);
                        if (results.Any())
                        {
                            serviceRequests = GetServiceRequests(results.ToList());
                            foreach (var request in serviceRequests)
                            {
                                var displayRequest = GetDisplayItem(request);
                                dataGrid.Items.Add(displayRequest);
                            }
                        }
                        else
                        {
                            DisplayServiceRequests();
                            MessageBox.Show("Service request not found.", "Search Result", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    else
                    {
                        DisplayServiceRequests();
                        MessageBox.Show("Please enter a valid request ID.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    break;

                case "Priority":
                    var priorityResults = SearchServiceRequestsByPriority(searchText);
                    if (priorityResults.Any())
                    {
                        serviceRequests = GetServiceRequests(priorityResults.ToList());
                        foreach (var request in serviceRequests)
                        {
                            var displayRequest = GetDisplayItem(request);
                            dataGrid.Items.Add(displayRequest);
                        }
                    }
                    else
                    {
                        DisplayServiceRequests();
                        MessageBox.Show("No service requests found with the specified priority.", "Search Result", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    break;

                case "Status":
                    var statusResults = SearchServiceRequestsByStatus(searchText);
                    if (statusResults.Any())
                    {
                        serviceRequests = GetServiceRequests(statusResults.ToList());
                        foreach (var request in serviceRequests)
                        {
                            var displayRequest = GetDisplayItem(request);
                            dataGrid.Items.Add(displayRequest);
                        }
                    }
                    else
                    {
                        DisplayServiceRequests();
                        MessageBox.Show("No service requests found with the specified status.", "Search Result", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    break;

                default:
                    DisplayServiceRequests();
                    MessageBox.Show("Invalid search type selected.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                    break;
            }
        }

    }

}
//__---____---____---____---____---____---____---__.ooo END OF FILE ooo.__---____---____---____---____---____---____---__\\