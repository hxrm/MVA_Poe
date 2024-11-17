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
                        foreach (var neighbor in adjacencyList[node])
                        {
                            if (!visited.Contains(neighbor.Item1)) // Only consider unvisited nodes
                                minHeap.Add(neighbor.Item2, neighbor.Item1);
                        }
                    }
                }

                return mst;
            }
        }
    }
