using MVA_poe.Classes;
using MVA_poe.Classes.SearchManagment;
using MVA_poe.Data;
using MVA_Poe.Classes;
using MVA_Poe.Data;
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
    /// Interaction logic for Service.xaml
    /// </summary>
    public partial class Service : Page
    {
        private AVLTree<ServiceRequest> requestTree = new AVLTree<ServiceRequest>();
        private MaxHeap requestHeap = new MaxHeap();

        public Service()
        {
            InitializeComponent();
            SetLanguage(DBHelper.lang);
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

        // Retrieves data from the database
        private void GetData()
        {
            // Use a database context to access the database
            using (var db = new AppDbContext())
            {
                // Retrieve all service requests from the database and include their associated reports
                var requestList = db.ServiceRequests.Include("report").ToList();

                // Clear the existing AVL tree
                requestTree = new AVLTree<ServiceRequest>();

                // Iterate through each service request in the list
                foreach (ServiceRequest request in requestList)
                {
                    // Add each service request to the AVL tree
                    requestTree.Insert(request);
                }

                requestHeap = new MaxHeap();

                foreach (ServiceRequest request in requestList)
                {
                    requestHeap.Insert(request);
                }
            }
            DisplayServiceRequests();
        }

        private IEnumerable<DisplayRequest> SearchServiceRequestById(int requestId)
        {
            var serviceRequests = requestTree.InOrderTraversal();
            return serviceRequests
               .Where(sr => sr.requestId.ToString().IndexOf(requestId.ToString()) >= 0)
               .Select(sr => GetDisplayItem(sr))
               .ToList();
        }
        private IEnumerable<DisplayRequest> SearchServiceRequestsByPriority(string priority)
        {
            var serviceRequests = requestHeap.ToArray();
            return serviceRequests
                .Where(sr => sr.requestPri.GetString().IndexOf(priority, StringComparison.OrdinalIgnoreCase) >= 0)
                .Select(sr => GetDisplayItem(sr))
                .ToList();
        }

        //private IEnumerable<DisplayRequest> SearchServiceRequestsByPriority(string priority)
        //{
        //    var serviceRequests = requestTree.InOrderTraversal();
        //    return serviceRequests
        //        .Where(sr => sr.requestPri.GetString().IndexOf(priority, StringComparison.OrdinalIgnoreCase) >= 0)
        //        .Select(sr => GetDisplayItem(sr))
        //        .ToList();
        //}

        private IEnumerable<DisplayRequest> SearchServiceRequestsByStatus(string status)
        {
            var serviceRequests = requestTree.InOrderTraversal();
            return serviceRequests
                .Where(sr => sr.requestStat.ToString().IndexOf(status, StringComparison.OrdinalIgnoreCase) >= 0)
                .OrderBy(sr => sr.requestStat) // Sort by status
                .Select(sr => GetDisplayItem(sr))
                .ToList();
        }


        private void DisplayServiceRequests()
        {
            var serviceRequests = requestTree.InOrderTraversal();
            dataGrid.Items.Clear();
            foreach (var request in serviceRequests)
            {                
                var displayRequest = GetDisplayItem(request);    
                dataGrid.Items.Add(displayRequest);
            }
        }        
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
        private void PriorizeButton_Click(object sender, RoutedEventArgs e)
        {
            var serviceRequests = requestHeap.ToArray();
            dataGrid.Items.Clear();
            foreach (var request in serviceRequests)
            {
                var displayRequest = GetDisplayItem(request);
                dataGrid.Items.Add(displayRequest);
            }
        }
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
                            foreach (var result in results)
                            {
                                dataGrid.Items.Add(result);
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
                        foreach (var result in priorityResults)
                        {
                            dataGrid.Items.Add(result);
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
                        foreach (var result in statusResults)
                        {
                            dataGrid.Items.Add(result);
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
        private ServiceRequest SearchServiceRequest(int requestId)
        {
            var serviceRequests = requestTree.InOrderTraversal();
            return serviceRequests.FirstOrDefault(sr => sr.requestId == requestId);
        }
        private void UpdateServiceRequest(ServiceRequest updatedRequest)
        {
            // Find the existing request in the AVL tree
            var existingRequest = SearchServiceRequest(updatedRequest.requestId);
            if (existingRequest != null)
            {
                // Update the properties of the existing request
                existingRequest.requestStat = updatedRequest.requestStat;
                existingRequest.requestPri = updatedRequest.requestPri;
                existingRequest.requestUpdate = updatedRequest.requestUpdate;
                existingRequest.employeeId = updatedRequest.employeeId;

                // Update the database
                using (var db = new AppDbContext())
                {
                   // db.ServiceRequests.Update(existingRequest);
                    db.SaveChanges();
                }

                // Refresh the AVL tree
                GetData();
            }
            else
            {
                MessageBox.Show("Service request not found.", "Update Result", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

    }

}
