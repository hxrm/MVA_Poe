using MVA_poe.Classes;
using MVA_poe.Classes.SearchManagment;
using MVA_poe.Controls;
using MVA_poe.Data;
using MVA_Poe.Classes;
using MVA_Poe.Data;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        private ServiceRequestManager sm;
        // Dictionary to store the levels of nodes
        private Dictionary<int, int> nodeLevels = new Dictionary<int, int>();
        // Variables to track the maximum X and Y coordinates for canvas resizing
        private double maxX = 0, maxY = 0;
        // Index to help with node positioning
        private int index = 0;
        // Dictionary to store the positions of nodes
        private Dictionary<int, Point> positions = new Dictionary<int, Point>();
        //----------------------------------------------------------------------------//
        // Constructor: DependencyVisual
        // Initializes a new instance of the DependencyVisual class
        public DependencyVisual()
        {
            InitializeComponent();
            // Initialize the ServiceRequestManager
            sm = new ServiceRequestManager();
            // Populate the category combo box
            PopulateCategoryComboBox();
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


                // Skip adding the "All" and "Other" categories
                if (catItem.Equals("All", StringComparison.OrdinalIgnoreCase) || catItem.Equals("Other", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                // Add the category to the combo box
                cmbCategory.Items.Add(catItem);
            }
        }

        //----------------------------------------------------------------------------//

        // Method: GetSelectedCategory
        // Returns the selected category from the combo box
        private ReportCategory GetSelectedCategory()
        {
            ReportCategory selectedCategory = ReportCategory.All; // Default category
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

        //----------------------------------------------------------------------------//
        // Method: ShowDependencyGraph_Click
        // Event handler for the "Show Dependency Graph" button click
        private void ShowDependencyGraph_Click(object sender, RoutedEventArgs e)
        {
            // Clear the canvas before drawing the graph
            visualizationCanvas.Children.Clear();
            // Draw the dependency graph
            DrawDependencyGraph();
        }
        //----------------------------------------------------------------------------//
        // Method: DrawDependencyGraph
        // Draws the dependency graph on the canvas
        private void DrawDependencyGraph()
        {
            // Reinitialize variables
            sm = new ServiceRequestManager();
            index = 0;
            positions.Clear();
            nodeLevels.Clear();
            var requests = ServiceRequestManager.graph.serviceRequests;

            var nodes = new Dictionary<int, ServiceCard>();
            var selectedCategory = GetSelectedCategory();

            // Filter requests based on the selected category
            if (selectedCategory != ReportCategory.All)
            {
                requests = new ConcurrentDictionary<int, ServiceRequest>(
                    requests.Where(r => r.Value.report.reportCat == selectedCategory)
                            .ToDictionary(r => r.Key, r => r.Value)
                );
            }

            // Lay out the nodes dynamically (with levels in DAG)
            GetLevels(requests);

            // Check if nodeLevels is empty
            if (!nodeLevels.Any())
            {
                MessageBox.Show("No node levels found. Cannot draw the graph.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            GetPositions(requests);

            // Draw dependencies (arrows) between nodes 
            foreach (var fromId in positions.Keys)
            {
                var dependencies = ServiceRequestManager.graph.GetDependencies(fromId);
                foreach (var toId in dependencies)
                {
                    // Ensure the dependent node exists
                    if (positions.ContainsKey(toId))
                    {
                        var start = positions[fromId];
                        var end = positions[toId];
                        // Add the arrow
                        DrawArrow(start, end);
                    }
                }
            }

            // Draw nodes (ServiceRequest) 
            foreach (var requestId in positions.Keys)
            {
                var request = requests[requestId];
                ServiceCard serviceCard = new ServiceCard(request);

                Canvas.SetLeft(serviceCard, positions[requestId].X);
                // Add service cards after arrows
                Canvas.SetTop(serviceCard, positions[requestId].Y);
                visualizationCanvas.Children.Add(serviceCard); 
                nodes[requestId] = serviceCard;
            }

            // Resize the canvas 
            visualizationCanvas.Width = maxX + 50;
            visualizationCanvas.Height = maxY + 50;
        }

        //----------------------------------------------------------------------------//
        // Method: GetLevels
        // Calculates the levels of nodes in the graph
        private void GetLevels(ConcurrentDictionary<int, ServiceRequest> requests)
        {
            foreach (var requestId in requests.Keys)
            {
                int level = sm.BFS(requestId);
                nodeLevels[requestId] = level;
            }
        }

        //----------------------------------------------------------------------------//
        // Method: GetPositions
        // Calculates the positions of nodes based on their levels
        private void GetPositions(ConcurrentDictionary<int, ServiceRequest> requests)
        {
            // Find the maximum level to invert the Y-coordinates
            int maxLevel = nodeLevels.Values.Max();

            foreach (var requestId in requests.Keys)
            {
                int level = nodeLevels[requestId];
                // Invert the Y-coordinate based on the maximum level
                //positions[requestId] = new Point(100 + (index % 4) * 200, 100 + (maxLevel - level) * 200);
                // index++;
                positions[requestId] = new Point(100 + (index % 4) * 200, 100 + level * 200);
                index++;

                // Track max X and Y to adjust canvas size
                maxX = Math.Max(maxX, positions[requestId].X + 50);
                maxY = Math.Max(maxY, positions[requestId].Y + 50);
            }
        }

        //----------------------------------------------------------------------------//
        // Method: DrawArrow
        // Helper method to draw an arrow between two points
        private void DrawArrow(Point start, Point end)
        {
            // Calculate the midpoint of the line
            var midX = (start.X + end.X) / 2 + 25;
            var midY = (start.Y + end.Y) / 2 + 25;

            // Draw the main line
            var line = new Line
            {
                X1 = start.X + 25,
                Y1 = start.Y + 25,
                X2 = end.X + 25,
                Y2 = end.Y + 25,
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };
            visualizationCanvas.Children.Add(line);

            // Calculate the angle of the line
            var angle = Math.Atan2(start.Y - end.Y, start.X - end.X);

            // Define arrowhead points
            var arrowSize = 10;
            // 30 degrees
            var arrowAngle = Math.PI / 6; 

            var arrowPoint1 = new Point(
                midX - arrowSize * Math.Cos(angle - arrowAngle),
                midY - arrowSize * Math.Sin(angle - arrowAngle)
            );
            var arrowPoint2 = new Point(
                midX - arrowSize * Math.Cos(angle + arrowAngle),
                midY - arrowSize * Math.Sin(angle + arrowAngle)
            );

            // Draw the arrowhead
            var arrow = new Polygon
            {
                Points = new PointCollection { new Point(midX, midY), arrowPoint1, arrowPoint2 },
                Fill = Brushes.Black
            };
            visualizationCanvas.Children.Add(arrow);
        }
    }
}//__---____---____---____---____---____---____---__.ooo END OF FILE ooo.__---____---____---____---____---____---____---__\\