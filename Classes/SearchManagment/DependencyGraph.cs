using MVA_Poe.Classes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace MVA_poe.Classes.SearchManagment
{
    // DependencyGraph class represents a directed graph to manage dependencies between service requests
    public class DependencyGraph
    {
        // Adjacency list to represent the graph
        private ConcurrentDictionary<int, List<int>> adjacencyList;

        // Map to store the ServiceRequest objects by their requestId
        public ConcurrentDictionary<int, ServiceRequest> serviceRequests;

        //----------------------------------------------------------------------------//

        // Constructor: DependencyGraph
        // Initializes a new instance of the DependencyGraph class
        public DependencyGraph()
        {
            adjacencyList = new ConcurrentDictionary<int, List<int>>();
            serviceRequests = new ConcurrentDictionary<int, ServiceRequest>();
        }

        //----------------------------------------------------------------------------//

        // Method: AddServiceRequest
        // Adds a new service request to the graph
        public void AddServiceRequest(ServiceRequest request)
        {
            // Ensure the request is added to the serviceRequests dictionary
            serviceRequests.TryAdd(request.requestId, request);

            // Initialize an empty list of dependencies for the request in the adjacency list
            adjacencyList.TryAdd(request.requestId, new List<int>());
        }

        //----------------------------------------------------------------------------//

        // Method: AddDependency
        // Adds a dependency between two requests
        public bool AddDependency(int fromRequestId, int toRequestId)
        {
            // Ensure both requests exist in the graph
            if (!serviceRequests.ContainsKey(fromRequestId) || !serviceRequests.ContainsKey(toRequestId))
            {
                throw new ArgumentException("One or both of the specified requests do not exist in the graph.");
            }

            // Prevent adding a self-loop
            if (fromRequestId == toRequestId)
            {
                throw new InvalidOperationException("A service request cannot depend on itself.");
            }

            // Check for cycles before adding the dependency
            if (HasCycle(fromRequestId, toRequestId))
            {
                throw new InvalidOperationException("Adding this dependency would create a cycle.");
            }

            // Add the dependency
            adjacencyList[fromRequestId].Add(toRequestId);
            return true;
        }
        //----------------------------------------------------------------------------//

        // Method: GetDependencies
        // Gets all dependencies of a service request
        public List<int> GetDependencies(int requestId)
        {
            return adjacencyList.TryGetValue(requestId, out var dependencies) ? dependencies : new List<int>();
        }

        //----------------------------------------------------------------------------//

        // Method: ResolveOrder
        // Performs a topological sort to resolve tasks in dependency order
        public List<int> ResolveOrder()
        {
            var visited = new HashSet<int>();
            var resultStack = new Stack<int>();

            foreach (var requestId in adjacencyList.Keys)
            {
                if (!visited.Contains(requestId))
                {
                    TopologicalSortHelper(requestId, visited, resultStack);
                }
            }

            return resultStack.Reverse().ToList(); 
        }

        //----------------------------------------------------------------------------//

        // Method: TopologicalSortHelper
        // Helper method for topological sort
        private void TopologicalSortHelper(int requestId, HashSet<int> visited, Stack<int> resultStack)
        {
            visited.Add(requestId);

            foreach (var dependentId in adjacencyList[requestId])
            {
                if (!visited.Contains(dependentId))
                {
                    TopologicalSortHelper(dependentId, visited, resultStack);
                }
            }

            resultStack.Push(requestId);
        }

        //----------------------------------------------------------------------------//

        // Method: TopologicalSort
        // Performs a topological sort to resolve tasks in dependency order
        public List<int> TopologicalSort()
        {
            // Dictionary to track the number of incoming edges for each node.
            var inDegree = new Dictionary<int, int>(); 

            // Initialize in-degree for all nodes in the graph as 0.
            foreach (var node in adjacencyList.Keys)
            {
                inDegree[node] = 0;
            }

            // Calculate in-degrees by iterating through the adjacency list.
            foreach (var edges in adjacencyList.Values)
            {
                foreach (var toId in edges)
                {
                    if (inDegree.ContainsKey(toId))
                    {// Increment in-degree for each target node.
                        inDegree[toId]++; 
                    }
                }
            }

            // Queue to store all nodes with in-degree 0 (no dependencies).
            var queue = new Queue<int>(inDegree.Where(kv => kv.Value == 0).Select(kv => kv.Key));
            var sortedOrder = new List<int>(); // List to store the topological order.

            // Process the nodes in the queue.
            while (queue.Count > 0)
            {
                var current = queue.Dequeue(); // Dequeue a node with in-degree 0.
                sortedOrder.Add(current); // Add it to the topological order.

                // Decrease the in-degree of its neighbors.
                foreach (var neighbor in adjacencyList[current])
                {
                    inDegree[neighbor]--;
                    // If the in-degree of a neighbor becomes 0, enqueue it.
                    if (inDegree[neighbor] == 0)
                    {
                        queue.Enqueue(neighbor);
                    }
                }
            }

            // If the sorted order does not contain all nodes, the graph contains a cycle.
            if (sortedOrder.Count != adjacencyList.Keys.Count)
            {
                throw new InvalidOperationException("Graph contains a cycle, topological sorting is not possible.");
            }
            // Return the topological order of the nodes.
            return sortedOrder; 
        }
        //----------------------------------------------------------------------------//

        // Method: HasCycle
        // Checks for cycles in the graph
        private bool HasCycle(int fromRequestId, int toRequestId)
        {
            var visited = new HashSet<int>();
            var stack = new HashSet<int>();

            return HasCycleHelper(fromRequestId, toRequestId, visited, stack);
        }
        //----------------------------------------------------------------------------//

        // Method: HasCycleHelper
        // Helper method to check for cycles in the graph
        private bool HasCycleHelper(int current, int target, HashSet<int> visited, HashSet<int> stack)
        {
            if (stack.Contains(current)) return true;

            if (visited.Contains(current)) return false;

            visited.Add(current);
            stack.Add(current);

            foreach (var dependent in adjacencyList[current])
            {
                if (dependent == target || HasCycleHelper(dependent, target, visited, stack))
                {
                    return true;
                }
            }

            stack.Remove(current);
            return false;
        }
    }
}
//__---____---____---____---____---____---____---__.ooo END OF FILE ooo.__---____---____---____---____---____---____---__\\