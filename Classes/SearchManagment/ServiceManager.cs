using MVA_poe.Data; // Importing the namespace for data access
using MVA_Poe.Classes; // Importing the namespace for classes used in the project
using System; // Importing the System namespace for basic functionalities
using System.Collections.Generic; // Importing the namespace for generic collections
using System.Linq; // Importing the namespace for LINQ functionalities
using System.Text; // Importing the namespace for text-related functionalities
using System.Threading.Tasks;
using System.Windows; // Importing the namespace for asynchronous programming

namespace MVA_poe.Classes.SearchManagment
{
    public class ServiceRequestManager
    {
        // DependencyGraph to manage dependencies between service requests
        public static DependencyGraph graph;
        // MinHeap to manage the priority of service requests
        public static MinHeap minHeap;
        // Dictionary to store service requests by their ID
        public static Dictionary<int, ServiceRequest> serviceRequests;
        public static MaxHeap maxHeap;
        public static MSTGraph graphMTS;
        public static AVLTree<ServiceRequest> avlTree;

        // Constructor to initialize the ServiceRequestManager
        public ServiceRequestManager()
        {
            graph = new DependencyGraph(); // Initialize the dependency graph
            minHeap = new MinHeap(); // Initialize the min heap
            serviceRequests = new Dictionary<int, ServiceRequest>(); // Initialize the dictionary for service requests
            maxHeap = new MaxHeap(); 
            graphMTS = new MSTGraph(); // Initialize the MST graph
            avlTree = new AVLTree<ServiceRequest>();
            PullData();
         
        }
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

                    // Add all service requests to the graph
                    //foreach (var request in serviceRequests.Values)
                    //{
                    //    AddServiceRequest(request);                       
                    //}
                    // Automatically add dependencies based on category and priority
                    AddCategoryBasedDependencies();
                    UpdateMST();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching data: {ex.Message}");
            }
        }


        public void GetAVL()
        {
            foreach (var kvp in serviceRequests)
            {
                ServiceRequest request = kvp.Value;

                // Add each service request to the AVL tree
                avlTree.Insert(request);
                maxHeap.Insert(request);
                request.requestDate = request.report.reportDate;
            }

            // Retrieve all service requests from the database and include their associated reports
            // var serviceRequests = db.ServiceRequests.Include("report").ToList();
            // requestHeap = new MaxHeap();
            // Clear the existing AVL tree
            // requestTree = new AVLTree<ServiceRequest>();

            // Iterate through each service request in the list
        }


        // Add a service request to the manager      
        // Method to add a service request
        //public void AddServiceRequest(ServiceRequest request)
        //{
        //    //AVL             
        //    request.AssignPriority();
        //    graph.AddServiceRequest(request);
        //    //MTS
        //    minHeap.Enqueue(request);
        //    if (request.Dependencies.Count < 0)
        //    {
        //        Console.WriteLine($"No Depency from {request.requestId}to another.?");
        //        return;
        //    }
        //    else { 
        //    foreach (var dependency in request.Dependencies)
        //    {
        //        AddDependency(request.requestId, dependency.requestId);
        //        graphMTS.AddDependency(request, dependency);
        //    }
        //}
        //}

        // Add a dependency between service requests        
        // Method to add a dependency between two service requests
        public void AddDependency(int fromId, int toId)
        {
            graph.AddServiceRequest(serviceRequests[fromId]);
            // Add the dependency to the graph
            graph.AddDependency(fromId, toId);           
           
        }
        // Method to automatically add dependencies based on category and priority
        public void AddCategoryBasedDependencies()
        {
            var categoryGroups = serviceRequests.Values.GroupBy(r => r.report.reportCat); // Group requests by category
            foreach (var group in categoryGroups)
            {
                // Sort requests by priority
                var sortedRequests = group.OrderBy(r => r.requestPri).ToList();
                for (int i = 0; i < sortedRequests.Count - 1; i++)
                {
                    var currentRequest = sortedRequests[i];
                    var nextRequest = sortedRequests[i + 1];

                    // Check and add dependency if not already present
                    if (!currentRequest.Dependencies.Contains(nextRequest))
                    {
                        currentRequest.Dependencies.Add(nextRequest);
                        nextRequest.DependentOn.Add(currentRequest);

                        // Optionally log or track the dependency creation
                        Console.WriteLine($"Added dependency: Request {currentRequest.requestId} -> Request {nextRequest.requestId}");
                    }
                }
            }
        }

        // Traverse the graph to resolve task order
        public List<int> TraverseGraph()
        {
            return graph.TopologicalSort();
        }

        // Prioritize tasks using MinHeap and return a Queue
        public Queue<ServiceRequest> PrioritizeTasks(IEnumerable<int> taskOrder)
        {
            // Enqueue requests in the minHeap based on task order
            foreach (var taskId in taskOrder)
            {
                var request = serviceRequests[taskId];
                minHeap.Enqueue(request);
            }

            // Create a queue to hold the prioritized tasks
            var prioritizedTasksQueue = new Queue<ServiceRequest>();

            // Dequeue tasks from the MinHeap and enqueue them into the prioritized queue
            while (!minHeap.IsEmpty())
            {
                prioritizedTasksQueue.Enqueue(minHeap.Dequeue());
            }

            return prioritizedTasksQueue;
        }
        public bool IsGraphConnected(HashSet<int> visited, Queue<ServiceRequest> toProcess, ServiceRequest startingRequest)
        {
            if (startingRequest == null)
                return false;

            // Enqueue the starting service request and mark it as visited
            toProcess.Enqueue(startingRequest);
            visited.Add(startingRequest.requestId); // Adding requestId to visited

            while (toProcess.Count > 0)
            {
                var currentRequest = toProcess.Dequeue(); // Dequeue the actual ServiceRequest

                // Add all unvisited dependencies to the queue
                foreach (var dependencyId in graph.GetDependencies(currentRequest.requestId))
                {
                    // Ensure that the dependency exists in the serviceRequests dictionary
                    if (serviceRequests.ContainsKey(dependencyId) && !visited.Contains(dependencyId))
                    {
                        var dependentRequest = serviceRequests[dependencyId];
                        visited.Add(dependencyId); // Mark this dependency as visited
                        toProcess.Enqueue(dependentRequest); // Enqueue the dependent ServiceRequest
                    }
                }
            }

            // If the visited count is not equal to the total number of requests, graph is disconnected
            return visited.Count == serviceRequests.Count;
        }



        // Add the GetMST method to calculate the Minimum Spanning Tree (MST)
        // Method to get the Minimum Spanning Tree (MST) and return it as a Queue
        public Queue<ServiceRequest> GetMST()
        {
            // Get the list of service requests forming the MST
            List<ServiceRequest> list = graphMTS.FindMST();

            // Create a new queue and enqueue each service request from the list
            Queue<ServiceRequest> mstQueue = new Queue<ServiceRequest>();
            foreach (var request in list)
            {
                mstQueue.Enqueue(request);
            }

            return mstQueue;
            //var mstQueue = new Queue<ServiceRequest>();
            //var visited = new HashSet<int>();
            //var toProcess = new Queue<ServiceRequest>();

            //// Start with the first service request
            //var startingRequest = serviceRequests.Values.FirstOrDefault();
            //foreach (var request in serviceRequests)
            //{
            //    Console.WriteLine($"Request ID: {request.Value.requestId}, Dependencies: {string.Join(", ", request.Value.Dependencies.Select(d => d.requestId))}");
            //}

            //if (startingRequest == null) throw new InvalidOperationException("No service requests available.");

            //visited.Add(startingRequest.requestId);
            //toProcess.Enqueue(startingRequest);

            //while (toProcess.Count > 0)
            //{
            //    var currentRequest = toProcess.Dequeue();
            //    mstQueue.Enqueue(currentRequest);

            //    foreach (var dependencyId in graph.GetDependencies(currentRequest.requestId))
            //    {
            //        if (!serviceRequests.ContainsKey(dependencyId))
            //            throw new KeyNotFoundException($"Dependency ID {dependencyId} not found in serviceRequests.");

            //        var dependentRequest = serviceRequests[dependencyId];
            //        if (!visited.Contains(dependentRequest.requestId))
            //        {
            //            visited.Add(dependentRequest.requestId);
            //            toProcess.Enqueue(dependentRequest);
            //        }
            //    }
            //}
            //bool valid = IsGraphConnected(visited, toProcess, startingRequest);
            //// Verify all nodes were visited
            //if (visited.Count != serviceRequests.Count)
            //    throw new InvalidOperationException("Graph is disconnected. MST cannot include all nodes. bool - "+valid);

            //return mstQueue;
        }

        public Dictionary<int, ServiceRequest> GetAllServiceRequests()
        {
            //ValidateGraph();
            var taskOrder = TraverseGraph();
            PrioritizeTasks(taskOrder);
            return serviceRequests;
        }
        public ServiceRequest GetServiceRequest()
        {

            return maxHeap.GetTopPriority();
        }

        public DependencyGraph GetDependencyGraph()
        {
            return graph;
        }
        private void UpdateMST()
        {
            try
            {
                
                // Populate the MSTGraph with current service requests and dependencies
                foreach (var request in GetAllServiceRequests().Values)
                {
                    foreach (var dependency in graph.GetDependencies(request.requestId))
                    {
                        var dependentRequest = GetAllServiceRequests()[dependency];
                        graphMTS.AddDependency(request, dependentRequest);
                    }
                }
                graphMTS.FindMST();

                // Log or use the MST as needed
                Console.WriteLine("MST updated successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating MST: {ex.Message}");
            }
        }       
       



    }
}