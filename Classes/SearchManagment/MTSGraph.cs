using MVA_Poe.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVA_poe.Classes.SearchManagment
{
    public class MSTGraph
    {
        // Adjacency list to represent the graph
        private Dictionary<ServiceRequest, List<Tuple<ServiceRequest, double>>> adjacencyList;

        public MSTGraph()
        {
            adjacencyList = new Dictionary<ServiceRequest, List<Tuple<ServiceRequest, double>>>();
        }

        // Add a dependency between two service requests (directed edge)
        public void AddDependency(ServiceRequest request1, ServiceRequest request2)
        {
            double weight = request1.CalculateDependencyWeight(request2);
            if (!adjacencyList.ContainsKey(request1))
                adjacencyList[request1] = new List<Tuple<ServiceRequest, double>>();
            if (!adjacencyList.ContainsKey(request2))
                adjacencyList[request2] = new List<Tuple<ServiceRequest, double>>();

            adjacencyList[request1].Add(new Tuple<ServiceRequest, double>(request2, weight));
            adjacencyList[request2].Add(new Tuple<ServiceRequest, double>(request1, weight)); // Assuming undirected edges for MST
        }

        // Prim's algorithm to find the MST
        public List<ServiceRequest> FindMST()
        {
            var mst = new List<ServiceRequest>(); // Resulting MST
            var visited = new HashSet<ServiceRequest>(); // Keeps track of visited nodes
            var minHeap = new SortedList<double, ServiceRequest>(); // Min heap to get the edge with the smallest weight

            // Start with an arbitrary node (for example, the first key in the adjacency list)
            var startNode = adjacencyList.Keys.First();
            visited.Add(startNode);
            mst.Add(startNode);

            foreach (var neighbor in adjacencyList[startNode])
            {
                minHeap.Add(neighbor.Item2, neighbor.Item1); // Add neighbors to the heap with their edge weights
            }

            while (minHeap.Count > 0)
            {
                var edge = minHeap.First(); // Get the edge with the smallest weight
                minHeap.RemoveAt(0); // Remove it from the heap

                var node = edge.Value;
                if (!visited.Contains(node))
                {
                    visited.Add(node);
                    mst.Add(node);

                    // Add all the neighbors of the newly added node to the heap
                    foreach (var neighbor in adjacencyList[startNode])
                    {
                        // Check if the neighbor already exists in the minHeap before adding
                        if (!minHeap.ContainsKey(neighbor.Item2))
                        {
                            minHeap.Add(neighbor.Item2, neighbor.Item1); // Add neighbors to the heap with their edge weights
                        }
                    }
                }
            }

            return mst;
        }       
        public Dictionary<ServiceRequest, List<Tuple<ServiceRequest, double>>> GetList()
        {
            return  adjacencyList;
        }
        public List<ServiceRequest> Fake()
        {
            // Create ServiceRequest objects
            var request1 = new ServiceRequest { requestId = 21, requestDate = DateTime.Now.AddDays(-3) };
            var request2 = new ServiceRequest { requestId = 22, requestDate = DateTime.Now.AddDays(-4) };
            var request3 = new ServiceRequest { requestId = 23, requestDate = DateTime.Now.AddDays(-2) };
            var request4 = new ServiceRequest { requestId = 24, requestDate = DateTime.Now.AddDays(-1) };
            var request5 = new ServiceRequest { requestId = 25, requestDate = DateTime.Now.AddDays(-6) };
            var request6 = new ServiceRequest { requestId = 26, requestDate = DateTime.Now.AddDays(-5) };

            // Set up dependencies
            request1.Dependencies = new List<ServiceRequest> { request2 }; // request1 depends on request2
            request2.Dependencies = new List<ServiceRequest> { request3 }; // request2 depends on request3
            request3.Dependencies = new List<ServiceRequest> { request4 }; // request3 depends on request4
            request4.Dependencies = new List<ServiceRequest> { request5 }; // request4 depends on request5
            request5.Dependencies = new List<ServiceRequest> { request6 }; // request5 depends on request6
            request6.Dependencies = new List<ServiceRequest> { request1 }; // request6 depends on request1 (creating a cycle)

            // Create a new MSTGraph and add dependencies between service requests
            MSTGraph graph = new MSTGraph();
            graph.AddDependency(request1, request2);
            graph.AddDependency(request2, request3);
            graph.AddDependency(request3, request4);
            graph.AddDependency(request4, request5);
            graph.AddDependency(request5, request6);
            graph.AddDependency(request6, request1); // Add cycle dependency

            // Test the MST algorithm
            List<ServiceRequest> mst = graph.FindMST();

            // Output the MST nodes (service requests)
            Console.WriteLine("Minimum Spanning Tree (MST) consists of the following requests:");
            foreach (var request in mst)
            {
                Console.WriteLine($"Request ID: {request.requestId}, Request Date: {request.requestDate}");
            }
            return mst;
        }
    }
}