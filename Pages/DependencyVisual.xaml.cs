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
        private Dictionary<int, int> nodeLevels = new Dictionary<int, int>(); 
        private double maxX = 0, maxY = 0;
        private int index = 0;
       private Dictionary<int, Point> positions = new Dictionary<int, Point>();
        //private List<ServiceRequest> allRequests;
        public DependencyVisual()
        {
            InitializeComponent();
            // Populate the category combo box
            sm = new ServiceRequestManager();
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

                // Skip adding the "All" category
                if (catItem.Equals("All", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }
                if (catItem.Equals("Other", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

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

        private void ShowDependencyGraph_Click(object sender, RoutedEventArgs e)
        {
            visualizationCanvas.Children.Clear();
            DrawDependencyGraph();
        }      
       
        private void DrawDependencyGraph()
        {
            sm = new ServiceRequestManager();
            index = 0;
            positions.Clear();
            nodeLevels.Clear();
            var requests = ServiceRequestManager.graph.serviceRequests;

            var nodes = new Dictionary<int, ServiceCard>();
            var selectedCategory = GetSelectedCategory();

            // Filter requests based on the selected category
            if (GetSelectedCategory() != ReportCategory.All)
            {
                requests = new ConcurrentDictionary<int, ServiceRequest>(
                    requests.Where(r => r.Value.report.reportCat == selectedCategory)
                            .ToDictionary(r => r.Key, r => r.Value)
                );
            }

            //Lay out the nodes dynamically (with levels in DAG)
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
                        //Add the arrow
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
                Canvas.SetTop(serviceCard, positions[requestId].Y);
                visualizationCanvas.Children.Add(serviceCard); // Add service cards after arrows
                nodes[requestId] = serviceCard;
            }

            // Resize the canvas 
            visualizationCanvas.Width = maxX + 50;
            visualizationCanvas.Height = maxY + 50;
        }        
        
        private void GetLevels(ConcurrentDictionary<int, ServiceRequest> requests)
        {
            foreach (var requestId in requests.Keys)
            {
                int level = sm.BFS(requestId);
                nodeLevels[requestId] = level;
            }
        }
        private void GetPositions(ConcurrentDictionary<int, ServiceRequest> requests)
        {
            // Find the maximum level to invert the Y-coordinates
            int maxLevel = nodeLevels.Values.Max();

            foreach (var requestId in requests.Keys)
            {
                int level = nodeLevels[requestId];
                // Invert the Y-coordinate based on the maximum level
                positions[requestId] = new Point(100 + (index % 4) * 200, 100 + (maxLevel - level) * 200);
                index++;
                // Track max X and Y to adjust canvas size
                maxX = Math.Max(maxX, positions[requestId].X + 50);
                maxY = Math.Max(maxY, positions[requestId].Y + 50);
            }
        }

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
            var angle = Math.Atan2(end.Y - start.Y, end.X - start.X);

            // Define arrowhead points
            var arrowSize = 10;
            var arrowAngle = Math.PI / 6; // 30 degrees

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