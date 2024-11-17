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

        // Constructor to initialize the ServiceRequestManager
        public ServiceRequestManager()
        {
            graph = new DependencyGraph(); // Initialize the dependency graph
            minHeap = new MinHeap(); // Initialize the min heap
            serviceRequests = new Dictionary<int, ServiceRequest>(); // Initialize the dictionary for service requests
            
        }
              
        // Method to add a dependency between two service requests
        public void AddDependency(int fromId, int toId)
        {
            // Add the dependency to the dependency graph
            graph.AddDependency(fromId, toId); 
        }

        // Method to resolve service requests
        public void ResolveRequests()
        {
            while (!minHeap.IsEmpty())
            {
                var request = minHeap.Dequeue(); // Dequeue the highest priority request

                // Check if all dependencies are resolved
                var dependencies = graph.GetDependencies(request.requestId);
                bool allDependenciesResolved = dependencies.All(depId => !serviceRequests.ContainsKey(depId));

                if (allDependenciesResolved)
                {
                    // Resolve the request
                    Console.WriteLine($"Resolving request {request.requestId} in category {request.report.reportCat}");
                    serviceRequests.Remove(request.requestId); // Remove the resolved request from the dictionary
                }
                else
                {
                    // Re-enqueue the request if dependencies are not resolved
                    minHeap.Enqueue(request);
                }
            }
        }
        // Method to add a service request
        public void AddServiceRequest(ServiceRequest request)
        {
            serviceRequests[request.requestId] = request; // Add the request to the dictionary
            graph.AddServiceRequest(request); // Add the request to the dependency graph
            minHeap.Enqueue(request); // Add the request to the min heap
        }
        private bool AreDependenciesResolved(int requestId)
        {
            var dependencies = graph.GetDependencies(requestId);
            return dependencies.All(depId => !serviceRequests.ContainsKey(depId));
        }

        // Method to automatically add dependencies based on category and priority
        public void AddCategoryBasedDependencies()
        {
            var categoryGroups = serviceRequests.Values.GroupBy(r => r.report.reportCat); // Group requests by category
            foreach (var group in categoryGroups)
            {
                var sortedRequests = group.OrderBy(r => r.requestPri).ToList(); // Sort requests by priority
                for (int i = 0; i < sortedRequests.Count - 1; i++)
                {
                    AddDependency(sortedRequests[i].requestId, sortedRequests[i + 1].requestId); // Add dependencies between consecutive requests
                }
            }
        }

        // Method to perform DFS to identify the order of task resolution
        public List<int> PerformDFS(int startId)
        {
            var visited = new HashSet<int>();
            var result = new List<int>();
            DFS(startId, visited, result);
            return result;
        }

        private void DFS(int requestId, HashSet<int> visited, List<int> result)
        {
            if (visited.Contains(requestId))
                return;

            visited.Add(requestId);
            result.Add(requestId);

            var dependencies = graph.GetDependencies(requestId);
            foreach (var depId in dependencies)
            {
                DFS(depId, visited, result);
            }
        }

        // Method to perform BFS to identify the order of task resolution
        public List<int> PerformBFS(int startId)
        {
            var visited = new HashSet<int>();
            var queue = new Queue<int>();
            var result = new List<int>();

            queue.Enqueue(startId);
            visited.Add(startId);

            while (queue.Count > 0)
            {
                var requestId = queue.Dequeue();
                result.Add(requestId);

                var dependencies = graph.GetDependencies(requestId);
                foreach (var depId in dependencies)
                {
                    if (!visited.Contains(depId))
                    {
                        queue.Enqueue(depId);
                        visited.Add(depId);
                    }
                }
            }

            return result;
        }

        // Method to check for cycles in the dependency graph
        public bool HasCycles()
        {
            var visited = new HashSet<int>();
            var recStack = new HashSet<int>();

            foreach (var requestId in serviceRequests.Keys)
            {
                if (HasCyclesUtil(requestId, visited, recStack))
                    return true;
            }

            return false;
        }

        private bool HasCyclesUtil(int requestId, HashSet<int> visited, HashSet<int> recStack)
        {
            if (recStack.Contains(requestId))
                return true;

            if (visited.Contains(requestId))
                return false;

            visited.Add(requestId);
            recStack.Add(requestId);

            var dependencies = graph.GetDependencies(requestId);
            foreach (var depId in dependencies)
            {
                if (HasCyclesUtil(depId, visited, recStack))
                    return true;
            }

            recStack.Remove(requestId);
            return false;
        }
        public void DisplayDependencies()
        {
            foreach (var request in serviceRequests.Values)
            {
                Console.WriteLine($"Request {request.requestId} ({request.report.reportCat}):");
                PrintDependencies(request.requestId, 0);
            }
        }

        private void PrintDependencies(int requestId, int indent)
        {
            var dependencies = graph.GetDependencies(requestId);
            foreach (var depId in dependencies)
            {
                Console.WriteLine($"{new string(' ', indent * 2)}-> Request {depId}");
                PrintDependencies(depId, indent + 1);
            }
        }
        public Dictionary<int, ServiceRequest> GetAllServiceRequests()
        {
            return serviceRequests;
        }

        public DependencyGraph GetDependencyGraph()
        {
            return graph;
        }
    }
}
