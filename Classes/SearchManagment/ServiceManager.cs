using MVA_poe.Data; // Importing the namespace for data access
using MVA_Poe.Classes; // Importing the namespace for classes used in the project
using System; // Importing the System namespace for basic functionalities
using System.Collections.Generic; // Importing the namespace for generic collections
using System.Linq; // Importing the namespace for LINQ functionalities
using System.Text; // Importing the namespace for text-related functionalities
using System.Threading.Tasks; // Importing the namespace for asynchronous programming

namespace MVA_poe.Classes.SearchManagment
{
    public class ServiceRequestManager
    {
        // DependencyGraph to manage dependencies between service requests
        private DependencyGraph graph;
        // MinHeap to manage the priority of service requests
        private MinHeap minHeap;
        // Dictionary to store service requests by their ID
        private Dictionary<int, ServiceRequest> serviceRequests;

        private MaxHeap maxHeap;
        private Dictionary<ServiceRequest, List<Tuple<ServiceRequest, double>>> mtsAdjacency;

        // Constructor to initialize the ServiceRequestManager
        public ServiceRequestManager()
        {
            graph = new DependencyGraph(); // Initialize the dependency graph
            minHeap = new MinHeap(); // Initialize the min heap
            serviceRequests = new Dictionary<int, ServiceRequest>(); // Initialize the dictionary for service requests
            minHeap = new MinHeap(); // Initialize the min heap
            mtsAdjacency = new Dictionary<ServiceRequest, List<Tuple<ServiceRequest, double>>>();

            // Automatically add dependencies based on category and priority
            //  AddCategoryBasedDependencies();
        }
        // Add a service request to the manager      
        // Method to add a service request
        public void AddServiceRequest(ServiceRequest request)
        {
            // Check if the request already exists
            if (!serviceRequests.ContainsKey(request.requestId))
            {
                request.AssignPriority();  // Assign priority dynamically
                serviceRequests[request.requestId] = request; // Add the request to the dictionary
                graph.AddServiceRequest(request); // Add the request to the dependency graph
                minHeap.Enqueue(request); // Add the request to the min heap for prioritization
                maxHeap.Insert(request);

            }
            else
            {
                // Optionally, log that the service request is being updated
                Console.WriteLine($"Service Request with ID {request.requestId} already exists. It is being updated.");
                request.AssignPriority();  // Assign priority dynamically
                serviceRequests[request.requestId] = request; // Update existing entry
                graph.AddServiceRequest(request); // Ensure it's updated in the dependency graph
                                                  // Re-enqueue the request to the heap if needed
                minHeap.Enqueue(request); // Optionally, you may want to dequeue and enqueue if priority changes
            }
        }

        // Add a dependency between service requests        
        // Method to add a dependency between two service requests
        public void AddDependency(int fromId, int toId)
        {
            if (graph.HasDependency(fromId, toId))
            {
                Console.WriteLine($"Dependency between {fromId} and {toId} already exists.");
            }
            else
            {
                graph.AddDependency(fromId, toId); // Add the dependency to the graph
            }           
            ValidateGraph();
        }
        // Validate the graph for cyclic dependencies
        public void ValidateGraph()
        {
            if (graph.HasCycle())
            {
                throw new InvalidOperationException("Cyclic dependencies detected in the graph.");
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

        // Add the GetMST method to calculate the Minimum Spanning Tree (MST)
        // Method to get the Minimum Spanning Tree (MST) and return it as a Queue
        public Queue<ServiceRequest> GetMST()
        {
            var mstQueue = new Queue<ServiceRequest>(); // This will hold the final MST in order
            var visited = new HashSet<int>(); // To track visited service requests
            var toProcess = new Queue<ServiceRequest>(); // Temporary queue for processing nodes

            // Start with any service request (e.g., the one with the lowest priority or the first one)
            var startingRequest = serviceRequests.Values.FirstOrDefault();

            if (startingRequest != null)
            {
                visited.Add(startingRequest.requestId); // Mark the starting request as visited
                toProcess.Enqueue(startingRequest); // Add it to the queue to begin processing
            }

            // While there are requests left to process in the queue
            while (toProcess.Count > 0)
            {
                var currentRequest = toProcess.Dequeue(); // Dequeue the next request to process
                mstQueue.Enqueue(currentRequest); // Add it to the MST queue

                // Explore all dependencies of the current request
                foreach (var dependency in graph.GetDependencies(currentRequest.requestId))
                {
                    var dependentRequest = serviceRequests[dependency]; 

                    // If the dependent request hasn't been visited, add it to the processing queue
                    if (!visited.Contains(dependentRequest.requestId))
                    {
                        visited.Add(dependentRequest.requestId); // Mark as visited
                        toProcess.Enqueue(dependentRequest); // Add to the processing queue
                    }
                }
            }

            // If the MST includes all requests, return it, else throw an error indicating disconnected components
            if (mstQueue.Count == serviceRequests.Count)
            {
                return mstQueue; // Successfully computed MST
            }
            else
            {
                throw new InvalidOperationException("MST could not be formed, possibly due to disconnected components.");
            }
        }
        public Dictionary<ServiceRequest, List<Tuple<ServiceRequest, double>>> GetAdjancey()
        {
            return mtsAdjacency;
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

        //// Method to automatically add dependencies based on category and priority
        //public void AddCategoryBasedDependencies()
        //{
        //    var categoryGroups = serviceRequests.Values.GroupBy(r => r.report.reportCat); // Group requests by category
        //    foreach (var group in categoryGroups)
        //    {
        //        var sortedRequests = group.OrderBy(r => r.requestPri).ToList(); // Sort requests by priority
        //        for (int i = 0; i < sortedRequests.Count - 1; i++)
        //        {
        //            AddDependency(sortedRequests[i].requestId, sortedRequests[i + 1].requestId); // Add dependencies between consecutive requests
        //        }
        //    }
        //}

       
    }
}
