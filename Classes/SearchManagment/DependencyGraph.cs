using MVA_Poe.Classes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace MVA_poe.Classes.SearchManagment
{
    public class DependencyGraph
    {
        // Thread-safe dictionaries for adjacency list and service requests
        private readonly ConcurrentDictionary<int, List<int>> adjacencyList;
        private readonly ConcurrentDictionary<int, ServiceRequest> serviceRequests;

        // Constructor to initialize the dictionaries
        public DependencyGraph()
        {
            adjacencyList = new ConcurrentDictionary<int, List<int>>();
            serviceRequests = new ConcurrentDictionary<int, ServiceRequest>();
        }

        // Method to add a service request to the graph
        public void AddServiceRequest(ServiceRequest request)
        {
            if (request == null || request.requestId <= 0)
            {
                throw new ArgumentException("Invalid service request or request ID.");
            }

            adjacencyList.TryAdd(request.requestId, new List<int>());
            serviceRequests.TryAdd(request.requestId, request);
        }

        // Method to add a dependency between two service requests
        public void AddDependency(int fromId, int toId)
        {
            if (!adjacencyList.ContainsKey(fromId))
            {
                throw new KeyNotFoundException($"Service request with ID {fromId} does not exist.");
            }
            if (!adjacencyList.ContainsKey(toId))
            {
                throw new KeyNotFoundException($"Service request with ID {toId} does not exist.");
            }

            // Check if the dependency already exists before adding
            if (HasDependency(fromId, toId))
            {
                Console.WriteLine($"Dependency between {fromId} and {toId} already exists.");
            }
            else
            {
                adjacencyList[fromId].Add(toId);

                // Optional: Check for cycles after adding a dependency
                if (HasCycle())
                {
                    adjacencyList[fromId].Remove(toId); // Rollback the change
                    throw new InvalidOperationException("Adding this dependency creates a cyclic dependency.");
                }
            }
        }
        // Method to get the dependencies of a service request
        public List<int> GetDependencies(int requestId)
        {
            if (!adjacencyList.ContainsKey(requestId))
            {
                throw new KeyNotFoundException($"Service request with ID {requestId} does not exist.");
            }
            return adjacencyList[requestId];
        }

        // Cyclic Dependency Detection
        // This method checks if the graph contains any cycles.
        public bool HasCycle()
        {
            var visited = new HashSet<int>(); // Tracks nodes that have been fully processed.
            var recursionStack = new HashSet<int>(); // Tracks nodes in the current recursion stack to detect cycles.

            // Iterate through all nodes in the graph.
            foreach (var node in adjacencyList.Keys)
            {
                // If a cycle is detected starting from any node, return true.
                if (IsCyclic(node, visited, recursionStack))
                {
                    return true;
                }
            }
            // If no cycles are found, return false.
            return false;
        }
        // Method to check if a dependency already exists between two service requests
        public bool HasDependency(int fromId, int toId)
        {
            if (!adjacencyList.ContainsKey(fromId))
            {
                throw new KeyNotFoundException($"Service request with ID {fromId} does not exist.");
            }

            // Check if 'toId' is in the list of dependencies for 'fromId'
            return adjacencyList[fromId].Contains(toId);
        }

        // Recursive helper method to check for cycles in the graph.
        private bool IsCyclic(int node, HashSet<int> visited, HashSet<int> recursionStack)
        {
            // If the node has not been visited yet.
            if (!visited.Contains(node))
            {
                visited.Add(node); // Mark the node as visited.
                recursionStack.Add(node); // Add the node to the recursion stack.

                // Iterate through all neighbors (dependencies) of the current node.
                foreach (var neighbor in adjacencyList[node])
                {
                    // If the neighbor has not been visited, recursively check it for cycles.
                    if (!visited.Contains(neighbor) && IsCyclic(neighbor, visited, recursionStack))
                    {
                        return true; // Cycle detected.
                    }
                    // If the neighbor is in the recursion stack, a cycle is detected.
                    else if (recursionStack.Contains(neighbor))
                    {
                        return true;
                    }
                }
            }
            // Remove the node from the recursion stack after processing all neighbors.
            recursionStack.Remove(node);
            return false; // No cycle detected for this node.
        }

        // Topological Sorting using Kahn's Algorithm
        // This method generates a topological order of nodes in the graph.
        public List<int> TopologicalSort()
        {
            var inDegree = new Dictionary<int, int>(); // Dictionary to track the number of incoming edges for each node.

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
                    {
                        inDegree[toId]++; // Increment in-degree for each target node.
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

            return sortedOrder; // Return the topological order of the nodes.
        }

        //// Method to get a service request by its ID
        //public ServiceRequest GetServiceRequest(int requestId)
        //{
        //    serviceRequests.TryGetValue(requestId, out var request);
        //    return request;
        //}

        //// Method to get all service requests
        //public IEnumerable<ServiceRequest> GetAllServiceRequests()
        //{
        //    return serviceRequests.Values;
        //}
    }
}

