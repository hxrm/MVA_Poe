using MVA_poe.Classes;
using MVA_poe.Classes.SearchManagment;
using MVA_poe.Data;
using MVA_Poe.Classes;
using MVA_Poe.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MVA_poe.Pages
{
    /// <summary>
    /// Interaction logic for DependencyVisual.xaml
    /// </summary>
    public partial class DependencyVisual : Page
    {
        // ServiceRequestManager to manage service requests and dependencies
        private ServiceRequestManager serviceManager;
        private List<ServiceRequest> allRequests;

        public DependencyVisual()
        {
            InitializeComponent();
            GetData();
            // Populate the category combo box
            PopulateCategoryComboBox();
        }        
        private void GetData()
        {
            serviceManager = new ServiceRequestManager(); // Initialize the service request manager

            // Fetch data from the database
            using (var db = new AppDbContext())
            {
                allRequests = db.ServiceRequests.Include("Dependencies").Include("report").ToList();

                foreach (var request in allRequests)
                {
                    serviceManager.AddServiceRequest(request); 
                }

                // Manually add dependencies based on database data
                foreach (var request in allRequests)
                {
                    foreach (var dependency in request.Dependencies)
                    {
                        serviceManager.AddDependency(request.requestId, dependency.requestId);
                    }
                }
            }
        }
        //----------------------------------------------------------------------------//

        // Method: PopulateCategoryComboBox
        // Populates the category combo box with values from the ReportCategory enum
        private void PopulateCategoryComboBox()
        {
            // Iterate through each value in the ReportCategory enum
            foreach (ReportCategory category in Enum.GetValues(typeof(ReportCategory)))
            {
                // Use the GetString extension method to get the description
                string catItem = category.GetString();
                cmbCategory.Items.Add(catItem);
            }
        }
        //----------------------------------------------------------------------------//

        // Method: GetSelectedCategory
        // Returns the selected category from the combo box
        private ReportCategory GetSelectedCategory()
        {
            ReportCategory selectedCategory = ReportCategory.Other; // Default category
            // Check if an item is selected in the combo box
            if (cmbCategory.SelectedItem != null)
            {
                // Get the selected category description
                string selectedCategoryDescription = cmbCategory.SelectedItem.ToString();

                // Find the enum value that matches the description
                foreach (ReportCategory category in Enum.GetValues(typeof(ReportCategory)))
                {
                    if (category.GetString() == selectedCategoryDescription)
                    {
                        return category;
                    }
                }
            }
            return selectedCategory;
        }

        private void ShowDependencyGraph_Click(object sender, RoutedEventArgs e)
        {
            visualizationCanvas.Children.Clear();
            DrawDependencyGraph();
        }

        private void DrawDependencyGraph()
        {
            visualizationCanvas.Children.Clear(); // Clear the canvas

            var nodes = new Dictionary<int, Ellipse>();
            var positions = new Dictionary<int, Point>();
            var selectedCategory = GetSelectedCategory();
            var requests = serviceManager.GetAllServiceRequests(); // Fetch all service requests
            var dependencyGraph = serviceManager.GetDependencyGraph(); // Fetch the dependency graph

            // Filter requests based on the selected category
            if (selectedCategory != ReportCategory.Other)
            {
                requests = requests.Where(r => r.Value.report.reportCat == selectedCategory).ToDictionary(r => r.Key, r => r.Value);
            }

            // Define positions for the nodes (dynamic positioning)
            int index = 0;
            foreach (var requestId in requests.Keys)
            {
                positions[requestId] = new Point(100 + (index % 4) * 200, 100 + (index / 4) * 200);
                index++;
            }

            // Draw nodes
            foreach (var requestId in positions.Keys)
            {
                var request = requests[requestId];
                var ellipse = new Ellipse
                {
                    Width = 50,
                    Height = 50,
                    Fill = GetStatusColor(request.requestStat),
                    Stroke = Brushes.Black,
                    StrokeThickness = 2
                };

                Canvas.SetLeft(ellipse, positions[requestId].X);
                Canvas.SetTop(ellipse, positions[requestId].Y);
                visualizationCanvas.Children.Add(ellipse);

                var textBlock = new TextBlock
                {
                    Text = $"{requestId}\n{request.report.reportCat}\n{request.requestPri}",
                    FontSize = 12,
                    FontWeight = FontWeights.Bold,
                    Foreground = Brushes.Black,
                    TextAlignment = TextAlignment.Center
                };

                Canvas.SetLeft(textBlock, positions[requestId].X + 5);
                Canvas.SetTop(textBlock, positions[requestId].Y + 5);
                visualizationCanvas.Children.Add(textBlock);

                nodes[requestId] = ellipse;
            }

            // Draw dependencies
            foreach (var fromId in positions.Keys)
            {
                var dependencies = dependencyGraph.GetDependencies(fromId);
                foreach (var toId in dependencies)
                {
                    if (positions.ContainsKey(toId)) // Ensure the dependent node exists
                    {
                        var line = new Line
                        {
                            X1 = positions[fromId].X + 25,
                            Y1 = positions[fromId].Y + 25,
                            X2 = positions[toId].X + 25,
                            Y2 = positions[toId].Y + 25,
                            Stroke = Brushes.Black,
                            StrokeThickness = 2
                        };

                        visualizationCanvas.Children.Add(line);
                    }
                }
            }
        }

        private void CategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
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
            };

            return displayRequest;
        }

        // Method to get the color based on the status of the service request
        private Brush GetStatusColor(Status status)
        {
            switch (status)
            {
                case Status.Pending:
                    return Brushes.Blue;
                case Status.Active:
                    return Brushes.Purple;
                case Status.Completed:
                    return Brushes.Green;
                default:
                    return Brushes.Gray;
            }
        }

        // Method to get the color based on the priority of the service request
        private Brush GetPriorityColor(Priority priority)
        {
            switch (priority)
            {
                case Priority.High:
                    return Brushes.Red;
                case Priority.Medium:
                    return Brushes.Orange;
                case Priority.Low:
                    return Brushes.Yellow;
                default:
                    return Brushes.Gray;
            }
        }
    }
}
