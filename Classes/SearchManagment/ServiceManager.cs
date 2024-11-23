
using MVA_Poe.Classes; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Windows; 

namespace MVA_poe.Classes.SearchManagment
{
    public class ServiceRequestManager
    {
        // Dictionary to store service requests by their ID
        public static Dictionary<int, ServiceRequest> serviceRequests;
        // DependencyGraph to manage dependencies between service requests
        public static DependencyGraph graph;
        public static AVLTree<ServiceRequest> avlTree;

        // Constructor to initialize the ServiceRequestManager
        public ServiceRequestManager()
        {
            graph = new DependencyGraph();
            serviceRequests = new Dictionary<int, ServiceRequest>();
            avlTree = new AVLTree<ServiceRequest>();
            PullData();

        }
        //----------------------------------------------------------------------------//

        // Method: PullData
        // Fetches data from the database and updates the serviceRequests dictionary
        private void PullData()
        {
            try
            {
                // Fetch data from the database
                using (var db = new AppDbContext())
                {
                    // Convert the list to a dictionary with requestId as the key
                    var newServiceRequests = db.ServiceRequests
                                               .Include("Dependencies")
                                               .Include("report")
                                               .ToList()
                                               .ToDictionary(request => request.requestId);

                    // Check if the new dictionary is the same as the current one
                    if (serviceRequests != null && serviceRequests.Count == newServiceRequests.Count &&
                        !serviceRequests.Except(newServiceRequests).Any())
                    {
                        // If the dictionaries are the same, return early
                        Console.WriteLine("No changes in service requests. Exiting PullData.");
                        return;
                    }

                    // Update the current serviceRequests dictionary
                    serviceRequests = newServiceRequests;
                    foreach (var request in serviceRequests.Values)
                    {  // Assign service request status priority 
                        request.AssignPriority();
                    }
                    AddCategoryBasedDependencies();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching data: {ex.Message}");
            }
        }
        //----------------------------------------------------------------------------//

        // Method: GetAVL
        // Inserts service requests into AVL tree 

        public void GetAVL()
        {
            foreach (var kvp in serviceRequests)
            {
                ServiceRequest request = kvp.Value;

               // avlTree.Insert(request, request.requestId);
                avlTree.AddServiceRequest(request,request.requestId,request);
                request.requestDate = request.report.reportDate;
            }
        }
        //----------------------------------------------------------------------------//

        // Method: BFS
        // Performs a Breadth-First Search to calculate the level of a node in the graph
        public int BFS(int nodeId)
        {
            var level = 0;
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
                        level = Math.Max(level, BFS(dependency) + 1);
                    }
                }
            }

            return level;
        }
        //----------------------------------------------------------------------------//
        // Method: ToStatusArrayUsingAVL
        // Method to convert the avl into an array, sorted by status using an AVL tree
        public ServiceRequest[] ToStatusArrayUsingAVL()
        {
            // Create an AVL Tree to store the service requests, using the integer value of Status for comparisons
             AVLTree<int> avlTree = new AVLTree<int>();          
            
            foreach(var kvp in serviceRequests)
            {
                // Access the ServiceRequest object from the dictionary
                ServiceRequest request = kvp.Value; 
               // Convert the Status enum to its integer value
                int status = (int)request.requestStat;
                avlTree.status = true;

                //    // Add the service request to the AVL tree with the status as the key
                avlTree.AddServiceRequest(status, request.requestId, request);

                // Optional: Output to the console for debugging purposes
                Console.WriteLine($"Inserted request with ID: {request.requestId} and Status: {status}");
            }


            // Perform an in-order traversal of the AVL Tree to get the sorted list of service requests
            var sortedList = avlTree.GetSortedServiceRequests();
            avlTree.status = false;

            // Convert the sorted list to an array and return it
            return sortedList.ToArray();
        }
        //----------------------------------------------------------------------------//
        // Method: ToStatusArrayUsingAVL
        // Method to convert the avl to into an array, sorted by status using an AVL tree
        public ServiceRequest[] ToPriorityArrayUsingAVL()
        {
            // Create an AVL Tree to store the service requests, using the integer value of Status for comparisons
            AVLTree<int> avlTree = new AVLTree<int>();

            foreach (var kvp in serviceRequests)
            {
                // Access the ServiceRequest object from the dictionary
                ServiceRequest request = kvp.Value; 
                // Convert the Status enum to its integer value
                int prior = (int)request.requestPri;
                avlTree.priority= true;

                //Add the service request to the AVL tree with the status as the key
                avlTree.AddServiceRequest(prior, request.requestId, request);
            }
            // Perform an in-order traversal of the AVL Tree to get the sorted list of service requests
            var sortedList = avlTree.GetSortedServiceRequests();
            avlTree.priority = false;

            // Convert the sorted list to an array and return it
            return sortedList.ToArray();
        }
        //----------------------------------------------------------------------------//

        // Method: AddCategoryBasedDependencies
        // Adds dependencies between service requests based on their categories and priorities
        public void AddCategoryBasedDependencies()
        {
            GetAVL();
            var categoryGroups = serviceRequests.Values.GroupBy(r => r.report.reportCat);           

            foreach (var group in categoryGroups)
            {
                // Sort requests by priority within each category
                var sortedRequests = group.OrderBy(r => r.requestPri).ToList();

                for (int i = 0; i < sortedRequests.Count - 1; i++)
                {
                    var currentRequest = sortedRequests[i];
                    var nextRequest = sortedRequests[i + 1];

                    // Add the requests to the DependencyGraph if not already present
                    graph.AddServiceRequest(currentRequest);
                    graph.AddServiceRequest(nextRequest);

                    // Add the dependency to the DependencyGraph
                    try
                    {
                        if (graph.AddDependency(currentRequest.requestId, nextRequest.requestId))
                        {
                            Console.WriteLine($"Added dependency: Request {currentRequest.requestId} -> Request {nextRequest.requestId}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to add dependency: {ex.Message}");
                    }

                    // Also update the self-referencing collections for UI or logic purposes
                    if (!currentRequest.Dependencies.Contains(nextRequest))
                    {
                        currentRequest.Dependencies.Add(nextRequest);
                        nextRequest.DependentOn.Add(currentRequest);
                    }
                }
            }
        }

        ////----------------------------------------------------------------------------//

        //// Method: TraverseGraph
        //// Traverses the graph to resolve task order using topological sort
        //public List<int> TraverseGraph()
        //{
        //    return graph.TopologicalSort();
        //}      

    }
}//__---____---____---____---____---____---____---__.ooo END OF FILE ooo.__---____---____---____---____---____---____---__\\