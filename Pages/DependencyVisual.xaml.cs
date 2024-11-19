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
        //private List<ServiceRequest> allRequests;
        public DependencyVisual()
        {
            InitializeComponent();
            GetData();
            // Populate the category combo box
            sm = new ServiceRequestManager();
            PopulateCategoryComboBox();
        }

        // Retrieves data from the database
        private void GetData()
        {
            //// Use a database context to access the database
            //using (var db = new AppDbContext())
            //{
            //    // Retrieve all service requests from the database and include their associated reports
            //    var requestList = db.ServiceRequests.Include("report").ToList();
            //    requestHeap = new MaxHeap();
            //    // Clear the existing AVL tree
            //    requestTree = new AVLTree<ServiceRequest>();

            //    // Iterate through each service request in the list
            //    foreach (ServiceRequest request in requestList)
            //    {
            //        // Add each service request to the AVL tree
            //        requestTree.Insert(request);
            //        requestHeap.Insert(request);
            //        request.requestDate = request.report.reportDate;
            //    }            

            //}
            sm = new ServiceRequestManager();
           // sm.GetDependencyGraph();
         
        }
        //private void GetData()
        //{
        //    serviceManager = new ServiceRequestManager(); // Initialize the service request manager

        //    try
        //    {
        //        // Fetch data from the database
        //        using (var db = new AppDbContext())
        //        {
        //            allRequests = db.ServiceRequests.Include("Dependencies").Include("report").ToList();

        //            foreach (var request in allRequests)
        //            {
        //                serviceManager.AddServiceRequest(request);
        //            }

        //            // Manually add dependencies based on database data
        //            foreach (var request in allRequests)
        //            {
        //                foreach (var dependency in request.Dependencies)
        //                {
        //                    serviceManager.AddDependency(request.requestId, dependency.requestId);
        //                }
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Error fetching data: {ex.Message}");
        //    }
        //}

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
            //visualizationCanvas.Children.Clear();
            DrawDependencyGraph();
        }
        private void MTS_Click(object sender, RoutedEventArgs e)
        {
            visualizationCanvas.Children.Clear();
             //  DrawMTS();
           //MSTGraph graphMTS  = new MSTGraph();
         
          //  Draw(graphMTS.Fake());
        }
        private void DrawMTS()
        {
            // Get the MST from your service manager
            var mstGraph = sm.GetMST();
            var adjacencyList = ServiceRequestManager.graphMTS.GetList();
            // Create a dictionary to store the UI elements for each ServiceRequest
            var nodes = new Dictionary<ServiceRequest, Ellipse>();

            // Determine positions for nodes (this can be adjusted or automated)
            double xOffset = 50;
            double yOffset = 50;
            double nodeSpacing = 100;

            // Draw the nodes (service requests)
            foreach (var request in mstGraph)
            {
                // Create a new ellipse for each ServiceRequest
                var ellipse = new Ellipse
                {
                    Width = 50,
                    Height = 50,
                    Fill = Brushes.LightBlue,
                    Stroke = Brushes.Black,
                    StrokeThickness = 2
                };

                // Add text inside the ellipse (optional, e.g., request ID)
                var textBlock = new TextBlock
                {
                    Text = $"ID: {request.requestId}",
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Foreground = Brushes.Black
                };

                // Set positions for each node (simple linear arrangement)
                Canvas.SetLeft(ellipse, xOffset);
                Canvas.SetTop(ellipse, yOffset);

                // Add the ellipse to the canvas
                visualizationCanvas.Children.Add(ellipse);
                visualizationCanvas.Children.Add(textBlock);

                // Store the ellipse for later use (in case we need to connect it to others)
                nodes[request] = ellipse;

                // Increment the offset for the next node
                xOffset += nodeSpacing;
            }

            // Draw the edges (dependencies)
            foreach (var request in mstGraph)
            {
                var nodeEllipse = nodes[request];

                // Iterate through the dependencies (ServiceRequest -> dependent service request)
                foreach (var dependency in adjacencyList[request])
                {
                    var dependentRequest = dependency.Item1;
                    var edgeWeight = dependency.Item2;

                    // Get the ellipse of the dependent node
                    var dependentEllipse = nodes[dependentRequest];

                    // Draw a line between the two nodes
                    var line = new Line
                    {
                        X1 = Canvas.GetLeft(nodeEllipse) + nodeEllipse.Width / 2,
                        Y1 = Canvas.GetTop(nodeEllipse) + nodeEllipse.Height / 2,
                        X2 = Canvas.GetLeft(dependentEllipse) + dependentEllipse.Width / 2,
                        Y2 = Canvas.GetTop(dependentEllipse) + dependentEllipse.Height / 2,
                        Stroke = Brushes.Black,
                        StrokeThickness = 2
                    };

                    // Optionally, display the edge weight on the line
                    var edgeWeightText = new TextBlock
                    {
                        Text = edgeWeight.ToString("0.0"),
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Foreground = Brushes.Black
                    };
                    Canvas.SetLeft(edgeWeightText, (line.X1 + line.X2) / 2);
                    Canvas.SetTop(edgeWeightText, (line.Y1 + line.Y2) / 2);

                    // Add the line and the text to the canvas
                    visualizationCanvas.Children.Add(line);
                    visualizationCanvas.Children.Add(edgeWeightText);
                }
            }
        }
        private void Draw(List<ServiceRequest> mst)
        {
            // Define initial coordinates for the nodes
            double nodeRadius = 20;
            double angle = 0;
            double centerX = 200; // Center of the canvas
            double centerY = 200; // Center of the canvas
            double radius = 100;  // Distance between nodes
            Dictionary<ServiceRequest, Point> nodePositions = new Dictionary<ServiceRequest, Point>();

            // Draw the nodes (ServiceRequests)
            foreach (var request in mst)
            {
                // Calculate position for each node in a circular pattern
                double x = centerX + radius * Math.Cos(angle);
                double y = centerY + radius * Math.Sin(angle);

                // Add the position to the dictionary
                nodePositions[request] = new Point(x, y);

                // Create a circle (Ellipse) for each service request
                Ellipse node = new Ellipse
                {
                    Width = nodeRadius * 2,
                    Height = nodeRadius * 2,
                    Fill = Brushes.LightBlue,
                    Stroke = Brushes.Black,
                    StrokeThickness = 2
                };

                // Position the node on the canvas
                Canvas.SetLeft(node, x - nodeRadius);
                Canvas.SetTop(node, y - nodeRadius);

                // Add the node to the canvas
                visualizationCanvas.Children.Add(node);

                // Add text to the node (request ID)
                TextBlock text = new TextBlock
                {
                    Text = request.requestId.ToString(),
                    Foreground = Brushes.Black,
                    FontSize = 12,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };

                // Create a TextBlock for the request ID
                Canvas.SetLeft(text, x - nodeRadius / 2);
                Canvas.SetTop(text, y - nodeRadius / 2);
                visualizationCanvas.Children.Add(text);

                // Update the angle for the next node
                angle += 2 * Math.PI / mst.Count;
            }
            // Draw the edges (dependencies)
            foreach (var request in mst)
            {
                // Ensure that the request is present in the nodePositions dictionary
                if (!nodePositions.ContainsKey(request))
                {
                    Console.WriteLine($"Error: Node position for request {request.requestId} not found.");
                    continue;
                }

                var requestPosition = nodePositions[request];

                // Draw edges only if dependencies exist
                foreach (var dependency in request.Dependencies)
                {
                    // Ensure that the dependency is also in the dictionary
                    if (!nodePositions.ContainsKey(dependency))
                    {
                        Console.WriteLine($"Error: Node position for dependency {dependency.requestId} not found.");
                        continue;
                    }

                    var dependencyPosition = nodePositions[dependency];

                    // Draw a line (edge) between nodes
                    Line edge = new Line
                    {
                        X1 = requestPosition.X,
                        Y1 = requestPosition.Y,
                        X2 = dependencyPosition.X,
                        Y2 = dependencyPosition.Y,
                        Stroke = Brushes.Black,
                        StrokeThickness = 2
                    };

                    // Add the edge to the canvas
                    visualizationCanvas.Children.Add(edge);
                }
            }
        }

        //private void DrawDependencyGraph()
        //{
        //    visualizationCanvas.Children.Clear(); // Clear the canvas

        //    sm = new ServiceRequestManager();
        //    var requests = ServiceRequestManager.graph.serviceRequests;
        //    var dependencyGraph = ServiceRequestManager.graph;

        //    var nodes = new Dictionary<int, ServiceCard>();
        //    var positions = new Dictionary<int, Point>();
        //    var selectedCategory = GetSelectedCategory();

        //    // Filter requests based on the selected category
        //    if (selectedCategory != ReportCategory.All)
        //    {
        //        requests = new ConcurrentDictionary<int, ServiceRequest>(
        //            requests.Where(r => r.Value.report.reportCat == selectedCategory)
        //                    .ToDictionary(r => r.Key, r => r.Value)
        //        );
        //    }

        //    // Step 1: Lay out the nodes dynamically (with levels in DAG)
        //    int index = 0;
        //    var nodeLevels = new Dictionary<int, int>(); // Store levels of nodes in the DAG
        //    double maxX = 0, maxY = 0; // Variables to track the max position values

        //    foreach (var requestId in requests.Keys)
        //    {
        //        // Assign each node a level in the DAG (you might need to use a graph traversal)
        //        int level = GetNodeLevel(requestId, dependencyGraph); // You can calculate node levels based on dependencies

        //        nodeLevels[requestId] = level;
        //    }

        //    //  Define positions based on levels and index
        //    foreach (var requestId in requests.Keys)
        //    {
        //        int level = nodeLevels[requestId];
        //        positions[requestId] = new Point(100 + (index % 4) * 200, 100 + level * 200);
        //        index++;

        //        // Track max X and Y to adjust canvas size
        //        maxX = Math.Max(maxX, positions[requestId].X + 50); // 50 is the node width
        //        maxY = Math.Max(maxY, positions[requestId].Y + 50); // 50 is the node height
        //    }

        //    //  Draw nodes (ServiceRequest)
        //    foreach (var requestId in positions.Keys)
        //    {
        //        var request = requests[requestId];
        //        ServiceCard serviceCard = new ServiceCard(request);               

        //        Canvas.SetLeft(serviceCard, positions[requestId].X);
        //        Canvas.SetTop(serviceCard, positions[requestId].Y);
        //        visualizationCanvas.Children.Add(serviceCard);
        //        nodes[requestId] = serviceCard;
        //    }

        //    //Draw dependencies (arrows) between nodes
        //    foreach (var fromId in positions.Keys)
        //    {
        //        var dependencies = dependencyGraph.GetDependencies(fromId);
        //        foreach (var toId in dependencies)
        //        {
        //            // Ensure the dependent node exists
        //            if (positions.ContainsKey(toId)) 
        //            {
        //                var start = positions[fromId];
        //                var end = positions[toId];
        //                DrawArrow(end, start);
        //            }
        //        }
        //    }
        //    // Step 5: Resize the canvas to fit all nodes
        //    visualizationCanvas.Width = maxX + 50; // Add padding
        //    visualizationCanvas.Height = maxY + 50; // Add padding
        //}

        // Helper method to calculate node level (distance from the root node)
        private int GetNodeLevel(int nodeId, DependencyGraph graph)
        {
            var level = 0;
            // Perform a BFS or DFS to calculate levels based on dependencies
            var visited = new HashSet<int>();
            var queue = new Queue<int>();
            queue.Enqueue(nodeId);
            visited.Add(nodeId);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                var dependencies = graph.GetDependencies(current);
                foreach (var dependency in dependencies)
                {
                    if (!visited.Contains(dependency))
                    {
                        visited.Add(dependency);
                        queue.Enqueue(dependency);
                        level = Math.Max(level, GetNodeLevel(dependency, graph) + 1); // Increment level for dependency
                    }
                }
            }

            return level;
        }
        private void DrawDependencyGraph()
        {
            visualizationCanvas.Children.Clear(); // Clear the canvas

            sm = new ServiceRequestManager();
            var requests = ServiceRequestManager.graph.serviceRequests;
            var dependencyGraph = ServiceRequestManager.graph;

            var nodes = new Dictionary<int, ServiceCard>();
            var positions = new Dictionary<int, Point>();
            var selectedCategory = GetSelectedCategory();

            // Filter requests based on the selected category
            if (selectedCategory != ReportCategory.All)
            {
                requests = new ConcurrentDictionary<int, ServiceRequest>(
                    requests.Where(r => r.Value.report.reportCat == selectedCategory)
                            .ToDictionary(r => r.Key, r => r.Value)
                );
            }

            // Step 1: Lay out the nodes dynamically (with levels in DAG)
            int index = 0;
            var nodeLevels = new Dictionary<int, int>(); // Store levels of nodes in the DAG
            double maxX = 0, maxY = 0; // Variables to track the max position values

            foreach (var requestId in requests.Keys)
            {
                // Assign each node a level in the DAG (you might need to use a graph traversal)
                int level = GetNodeLevel(requestId, dependencyGraph); // You can calculate node levels based on dependencies

                nodeLevels[requestId] = level;
            }

            // Define positions based on levels and index
            foreach (var requestId in requests.Keys)
            {
                int level = nodeLevels[requestId];
                positions[requestId] = new Point(100 + (index % 4) * 200, 100 + level * 200);
                index++;

                // Track max X and Y to adjust canvas size
                maxX = Math.Max(maxX, positions[requestId].X + 50); // 50 is the node width
                maxY = Math.Max(maxY, positions[requestId].Y + 50); // 50 is the node height
            }

            // Step 2: Draw dependencies (arrows) between nodes first
            foreach (var fromId in positions.Keys)
            {
                var dependencies = dependencyGraph.GetDependencies(fromId);
                foreach (var toId in dependencies)
                {
                    // Ensure the dependent node exists
                    if (positions.ContainsKey(toId))
                    {
                        var start = positions[fromId];
                        var end = positions[toId];
                        DrawArrow(end, start); // Add arrows first
                    }
                }
            }

            // Step 3: Draw nodes (ServiceRequest) after arrows
            foreach (var requestId in positions.Keys)
            {
                var request = requests[requestId];
                ServiceCard serviceCard = new ServiceCard(request);

                Canvas.SetLeft(serviceCard, positions[requestId].X);
                Canvas.SetTop(serviceCard, positions[requestId].Y);
                visualizationCanvas.Children.Add(serviceCard); // Add service cards after arrows
                nodes[requestId] = serviceCard;
            }

            // Step 4: Resize the canvas to fit all nodes
            visualizationCanvas.Width = maxX + 50; // Add padding
            visualizationCanvas.Height = maxY + 50; // Add padding
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
}